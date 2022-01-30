using UnityEngine;
using PCSC;
using System.Threading;
using PCSC.Monitoring;
using PcscNfc;
using NdefLibrary.Ndef;
using System;
using System.Collections.Concurrent;
using UnityEngine.UI;
using PCSC.Iso7816;

enum WriteResult { Success, Failure }

public class ReadTextTagExample : MonoBehaviour
{
    #region NfcThread
    private ConcurrentQueue<string> readQueue = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> writeQueue = new ConcurrentQueue<string>();
    private ConcurrentQueue<WriteResult> writeResultQueue = new ConcurrentQueue<WriteResult>();

    private Thread t;

    bool buzzerDisabled;

    ResponseApdu SendCommandApdu(ICardReader reader, CommandApdu apdu)
    {
        var sendPci = SCardPCI.GetPci(reader.Protocol);
        var receivePci = new SCardPCI();

        var receiveBuffer = new byte[256];
        var command = apdu.ToArray();

        var bytesReceived = reader.Transmit(
            sendPci,
            command,
            command.Length,
            receivePci,
            receiveBuffer,
            receiveBuffer.Length);

        var responseApdu =
            new ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case3Short, reader.Protocol);

        if (responseApdu.SW1 != (byte)SW1Code.Normal)
        {
            throw new Exception($"SW1 is {responseApdu.SW1}");
        }

        return responseApdu;
    }

    void Acr122uDisableBuzzerOutput(ICardReader reader)
    {
        // see 6.7. https://www.acs.com.hk/download-manual/419/API-ACR122U-2.04.pdf
        // for documentation of the command

        // command to read out blocks (up to 16 bytes)
        var apdu = new CommandApdu(IsoCase.Case2Short, reader.Protocol)
        {
            CLA = 0xFF,
            Instruction = 0,
            P1 = 0x52,
            P2 = 0x00,
        };

        SendCommandApdu(reader, apdu);
    }

    private void NfcThread()
    {
        string[] readerNames;
        using (var context = ContextFactory.Instance.Establish(SCardScope.System))
        {
            readerNames = context.GetReaders();
        }

        using (var monitor = MonitorFactory.Instance.Create(SCardScope.System))
        {
            monitor.CardInserted += (sender, args) => CardInserted(args);
            monitor.CardRemoved += (sender, args) => CardRemoved(args);

            monitor.Start(readerNames);

            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
                // thread interrupted
            }
        }

        void ReadOutTextTag(NfcTagReader nfcReader)
        {
            var rawMsg = nfcReader.ReadNdefMessage();
            var ndefMessage = NdefMessage.FromByteArray(rawMsg);

            foreach (NdefRecord record in ndefMessage)
            {
                if (record.CheckSpecializedType(false) == typeof(NdefTextRecord))
                {
                    var textRecord = new NdefTextRecord(record);
                    readQueue.Enqueue(textRecord.Text);
                }
            }
        }

        void WriteTextTag(NfcTagReader nfcReader, string text)
        {
            var record = new NdefTextRecord();
            record.Text = text;
            record.LanguageCode = "en";

            var message = new NdefMessage();
            message.Add(record);

            nfcReader.WriteNdefMessage(message.ToByteArray());
        }

        void CardInserted(CardStatusEventArgs args)
        {
            try
            {
                using var context = ContextFactory.Instance.Establish(SCardScope.System);
                using var rfidReader = context.ConnectReader(args.ReaderName, SCardShareMode.Shared, SCardProtocol.Any);

                // we disable the buzzer on first use, as the pcsc library has no clear other way
                if (!buzzerDisabled)
                {
                    Acr122uDisableBuzzerOutput(rfidReader);
                    buzzerDisabled = true;
                }

                var nfcReader = new NfcTagReader(rfidReader);

                if (writeQueue.TryDequeue(out var text))
                {
                    var result = WriteResult.Success;
                    try
                    {
                        WriteTextTag(nfcReader, text);
                    }
                    catch (Exception ex)
                    {
                        result = WriteResult.Failure;
                        Debug.Log(ex);
                    }
                    writeResultQueue.Enqueue(result);
                } // this also reads out the tag just written here

                ReadOutTextTag(nfcReader);

            }
            catch (Exception)
            {
                // catch error
            }

        }

        void CardRemoved(CardStatusEventArgs args)
        {
            readQueue.Enqueue(null);
        }
    }

    #endregion

    public InputField WriteInputText;
    public Text WriteTextOutput;

    /// <summary>
    ///  Gameobject with Text element to duplicate for showing tag read
    /// </summary>
    public GameObject ReadTagGameobject;

    void Start()
    {
        t = new Thread(NfcThread);
        t.Start();
    }

    void OnDestroy()
    {
        t.Interrupt();
    }

    void Update()
    {
        if (readQueue.TryDequeue(out var text) && text != null)
        {
            var newObject = Instantiate(ReadTagGameobject);

            var textComponent = newObject.GetComponent<Text>();
            if (textComponent != null )
            {
                textComponent.text = text;
            }

            newObject.AddComponent<Rigidbody2D>();

            newObject.transform.SetParent(ReadTagGameobject.transform.parent);
        }

        if (writeResultQueue.TryDequeue(out var result) && this.WriteTextOutput != null)
        {
            this.WriteTextOutput.text = result == WriteResult.Success ?
                "Tag successfully written" : "Tag write failed";
        }
    }

    public void WriteTextTag()
    {
        // already in progress of writing tag
        if (!writeQueue.IsEmpty)
        {
            return;
        }

        WriteTextOutput.text = "waiting for card";
        writeQueue.Enqueue(WriteInputText.text);
    }
}
