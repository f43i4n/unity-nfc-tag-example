using UnityEngine;
using System.Threading;
using System.Collections.Concurrent;
using UnityEngine.UI;
using uFCoderMulti;
using System;
using System.Threading.Tasks;

enum WriteResult { Success, Failure }

public class ReadTextTagExample : MonoBehaviour
{
    #region NfcThread
    private ConcurrentQueue<string> readQueue = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> writeQueue = new ConcurrentQueue<string>();
    private ConcurrentQueue<WriteResult> writeResultQueue = new ConcurrentQueue<WriteResult>();

    private Thread t;

    bool stopped = false;

    private DL_STATUS reader_open()
    {
        DL_STATUS status;

        uint reader_type;
        byte[] reader_sn = new byte[8];
        byte fw_major_ver;
        byte fw_minor_ver;
        byte fw_build;
        byte hw_major;
        byte hw_minor;

        //-------------------------------------------------------
        status = ufCoder.ReaderOpenEx(1, "", 0, "");
        if (status != DL_STATUS.UFR_OK)
        {
            return status;
        }

        //-------------------------------------------------------
        unsafe
        {
            fixed (byte* f_rdsn = reader_sn)
                status = ufCoder.GetReaderSerialDescription(f_rdsn);
        }

        unsafe
        {
            status |= ufCoder.GetReaderType(&reader_type);

            status |= ufCoder.GetReaderHardwareVersion(&hw_major, &hw_minor);

            status |= ufCoder.GetReaderFirmwareVersion(&fw_major_ver, &fw_minor_ver);

            status |= ufCoder.GetBuildNumber(&fw_build);
        }

        if (status != DL_STATUS.UFR_OK)
        {
            return status;
        }

        //-------------------------------------------------------

        Debug.Log(" SN : " + System.Text.Encoding.UTF8.GetString(reader_sn));

        Debug.Log(" HW : " + (int)hw_major + "." + hw_minor);

        Debug.Log(" FW : " + (fw_major_ver) + "." +
                                (fw_minor_ver) + "." +
                                (fw_build));

        return DL_STATUS.UFR_OK;
    }

    private DLOGIC_CARD_TYPE getcardtype()
    {
        DL_STATUS status;
        byte cardtype_val = 0;
        DLOGIC_CARD_TYPE cardtype;

        unsafe
        {
            status = ufCoder.GetDlogicCardType(&cardtype_val);
        }

        if (status != DL_STATUS.UFR_OK)
        {
            cardtype_val = 0;
        }

        cardtype = (DLOGIC_CARD_TYPE)cardtype_val;

        return cardtype;
    }

    private byte[] SubByteArray(byte[] sourceArray, int out_len)
    {
        byte[] truncArray = new byte[out_len];
        Array.Copy(sourceArray, truncArray, truncArray.Length);
        return truncArray;
    }

    private string read_ndef()
    {
        DL_STATUS result = DL_STATUS.UNKNOWN_ERROR;
        //byte tlv_type;
        //uint record_length;
        //ushort bytes_read;

        byte[] type = new byte[256];
        byte[] id = new byte[256];
        byte[] payload = new byte[1000];
        byte type_length, id_length, tnf;
        byte record_nr;
        byte message_cnt, record_cnt, empty_record_cnt;
        byte[] record_cnt_array = new byte[100];
        DLOGIC_CARD_TYPE cardtype;
        string ct;


        cardtype = getcardtype();
        // trim DL
        // _ to spc
        ct = String.Format("[{0:X}]", (int)cardtype);
        ct += " " + cardtype.ToString();




        if (cardtype == DLOGIC_CARD_TYPE.DL_NO_CARD)
        {
            return null;
        }

        unsafe
        {
            fixed (byte* pData = record_cnt_array)
                result = ufCoder.get_ndef_record_count(&message_cnt, &record_cnt, pData, &empty_record_cnt);
        }

        if (result != DL_STATUS.UFR_OK)
        {
            Debug.Log("Card is not initialized");
            return null;
        }

        uint payload_length;

        for (record_nr = 1; record_nr < record_cnt + 1; record_nr++)
        {
            unsafe
            {
                fixed (byte* f_type = type)
                fixed (byte* f_id = id)
                fixed (byte* f_payload = payload)

                    result = ufCoder.read_ndef_record(1, (byte)record_nr, &tnf, f_type, &type_length, f_id, &id_length, f_payload, &payload_length);
            }

            if (result != DL_STATUS.UFR_OK)
            {
                if (result == DL_STATUS.UFR_WRONG_NDEF_CARD_FORMAT)
                    Debug.Log(" NDEF format error");
                else if (result == DL_STATUS.UFR_NDEF_MESSAGE_NOT_FOUND)
                    Debug.Log(" NDEF message not found");
                else
                    Debug.Log(" Error: " + result);

                return null;
            }

            string str_payload = System.Text.Encoding.UTF8.GetString(SubByteArray(payload, (int)payload_length));
            string str_type = System.Text.Encoding.UTF8.GetString(SubByteArray(type, (int)type_length));
            string str_tnf = "TNF: " + System.Convert.ToString(tnf);
            //---------------------------------------------------------------------------

            string[] row = { record_nr.ToString(), str_type.ToString(), payload_length.ToString(), str_payload };

            // This a bit hacky, just to get this working
            if (str_payload.Contains("en"))
            {
                return str_payload.Trim().Substring(3);
            }

            return str_payload;
        }

        return null;
    }

    private void NfcThread()
    {
        string last_read = null;
        var null_reads = 0;

        void ReadTags()
        {
            var payload = read_ndef();
            if (payload == null)
            {
                // filter out some occasional errors
                if (++null_reads >= 5 && last_read != null)
                {
                    readQueue.Enqueue(payload);
                    last_read = payload;
                }
            }
            else if (payload != last_read)
            {
                readQueue.Enqueue(payload);
                last_read = payload;
                null_reads = 0;
            }
        }

        void WriteTags()
        {
            WriteResult TryWrite(string text)
            {
                try
                {
                    if (ufCoder.erase_all_ndef_records(1) != DL_STATUS.UFR_OK)
                    {
                        return WriteResult.Failure;
                    }
                    if (ufCoder.WriteNdefRecord_Text(1, text) != DL_STATUS.UFR_OK)
                    {
                        return WriteResult.Failure;
                    }
                    
                    return WriteResult.Success;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                    return WriteResult.Failure;
                }
            }

            if (writeQueue.TryDequeue(out var text))
            {
                var result = WriteResult.Failure;
                bool timeout = false;
                Task.Delay(10000).ContinueWith(t => timeout = true);
                while (!timeout && result == WriteResult.Failure)
                {
                    Thread.Sleep(100);

                    result = TryWrite(text);
                }
                writeResultQueue.Enqueue(result);
            }
        }

        try
        {
            if (reader_open() != DL_STATUS.UFR_OK)
            {
                Thread.Sleep(3000);

                // retry one time
                if (reader_open() != DL_STATUS.UFR_OK)
                {
                    Debug.Log("Could not open reader, please try again.");
                    return;
                }
            }

            while (!stopped)
            {
                ReadTags();

                WriteTags();

                Thread.Sleep(100);
            }
        }
        finally
        {
            ufCoder.ReaderClose();
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
        Debug.Log("on destroy");
        t.Interrupt();
        stopped = true;
    }

    void Update()
    {
        if (readQueue.TryDequeue(out var text))
        {
            text = text ?? "null";

            var newObject = Instantiate(ReadTagGameobject);

            var textComponent = newObject.GetComponent<Text>();
            if (textComponent != null)
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
