using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uFCoderMulti
{
    using System.Runtime.InteropServices;
    using UFR_HANDLE = System.UIntPtr;

    enum CARD_SAK
    {
        UNKNOWN = 0x00,
        MIFARE_CLASSIC_1k = 0x08,
        MF1ICS50 = 0x08,
        SLE66R35 = 0x88,
        MIFARE_CLASSIC_4k = 0x18,
        MF1ICS70 = 0x18,
        MIFARE_CLASSIC_MINI = 0x09,
        MF1ICS20 = 0x09,
    }

    enum DLOGIC_CARD_TYPE
    {
        DL_NO_CARD = 0x00,
        DL_MIFARE_ULTRALIGHT = 0x01,
        DL_MIFARE_ULTRALIGHT_EV1_11 = 0x02,
        DL_MIFARE_ULTRALIGHT_EV1_21 = 0x03,
        DL_MIFARE_ULTRALIGHT_C = 0x04,
        DL_NTAG_203 = 0x05,
        DL_NTAG_210 = 0x06,
        DL_NTAG_212 = 0x07,
        DL_NTAG_213 = 0x08,
        DL_NTAG_215 = 0x09,
        DL_NTAG_216 = 0x0A,
        DL_MIKRON_MIK640D = 0x0B,
        NFC_T2T_GENERIC = 0x0C,
        DL_NT3H_1101 = 0x0D,
        DL_NT3H_1201 = 0x0E,
        DL_NT3H_2111 = 0x0F,
        DL_NT3H_2211 = 0x10,

        DL_MIFARE_MINI = 0x20,
        DL_MIFARE_CLASSIC_1K = 0x21,
        DL_MIFARE_CLASSIC_4K = 0x22,
        DL_MIFARE_PLUS_S_2K_SL0 = 0x23,
        DL_MIFARE_PLUS_S_4K_SL0 = 0x24,
        DL_MIFARE_PLUS_X_2K_SL0 = 0x25,
        DL_MIFARE_PLUS_X_4K_SL0 = 0x26,
        DL_MIFARE_DESFIRE = 0x27,
        DL_MIFARE_DESFIRE_EV1_2K = 0x28,
        DL_MIFARE_DESFIRE_EV1_4K = 0x29,
        DL_MIFARE_DESFIRE_EV1_8K = 0x2A,
        DL_MIFARE_DESFIRE_EV2_2K = 0x2B,
        DL_MIFARE_DESFIRE_EV2_4K = 0x2C,
        DL_MIFARE_DESFIRE_EV2_8K = 0x2D,
        DL_MIFARE_PLUS_S_2K_SL1 = 0x2E,
        DL_MIFARE_PLUS_X_2K_SL1	= 0x2F,
        DL_MIFARE_PLUS_EV1_2K_SL1 = 0x30,       
        DL_MIFARE_PLUS_X_2K_SL2 = 0x31,
        DL_MIFARE_PLUS_S_2K_SL3	= 0x32,
        DL_MIFARE_PLUS_X_2K_SL3	= 0x33,
        DL_MIFARE_PLUS_EV1_2K_SL3 = 0x34,
        DL_MIFARE_PLUS_S_4K_SL1 = 0x35,
        DL_MIFARE_PLUS_X_4K_SL1	= 0x36,
        DL_MIFARE_PLUS_EV1_4K_SL1 = 0x37,
        DL_MIFARE_PLUS_X_4K_SL2	= 0x38,
        DL_MIFARE_PLUS_S_4K_SL3	= 0x39,
        DL_MIFARE_PLUS_X_4K_SL3	= 0x3A,
        DL_MIFARE_PLUS_EV1_4K_SL3 = 0x3B,

        // Special card type
        DL_GENERIC_ISO14443_4 = 0x40,
        DL_GENERIC_ISO14443_4_TYPE_B = 0x41,
        DL_GENERIC_ISO14443_3_TYPE_B = 0x42,

        DL_UNKNOWN_ISO_14443_4 = 0x40
    }

    // MIFARE CLASSIC Authentication Modes:
    enum MIFARE_AUTHENTICATION
    {
        MIFARE_AUTHENT1A = 0x60,
        MIFARE_AUTHENT1B = 0x61,
    }

    enum T2T_AUTHENTICATION
    {
        T2T_NO_PWD_AUTH = 0,
        T2T_RKA_PWD_AUTH = 1,
        T2T_PK_PWD_AUTH = 3,
        T2T_WITHOUT_PWD_AUTH = 0x60,
        T2T_WITH_PWD_AUTH = 0x61,
    };

    // API Status Codes Type:
    public enum DL_STATUS
    {
        UFR_OK = 0x00,

        UFR_COMMUNICATION_ERROR = 0x01,
        UFR_CHKSUM_ERROR = 0x02,
        UFR_READING_ERROR = 0x03,
        UFR_WRITING_ERROR = 0x04,
        UFR_BUFFER_OVERFLOW = 0x05,
        UFR_MAX_ADDRESS_EXCEEDED = 0x06,
        UFR_MAX_KEY_INDEX_EXCEEDED = 0x07,
        UFR_NO_CARD = 0x08,
        UFR_COMMAND_NOT_SUPPORTED = 0x09,
        UFR_FORBIDEN_DIRECT_WRITE_IN_SECTOR_TRAILER = 0x0A,
        UFR_ADDRESSED_BLOCK_IS_NOT_SECTOR_TRAILER = 0x0B,
        UFR_WRONG_ADDRESS_MODE = 0x0C,
        UFR_WRONG_ACCESS_BITS_VALUES = 0x0D,
        UFR_AUTH_ERROR = 0x0E,
        UFR_PARAMETERS_ERROR = 0x0F, // ToDo, tačka 5.
        UFR_MAX_SIZE_EXCEEDED = 0x10,

        UFR_WRITE_VERIFICATION_ERROR = 0x70,
        UFR_BUFFER_SIZE_EXCEEDED = 0x71,
        UFR_VALUE_BLOCK_INVALID = 0x72,
        UFR_VALUE_BLOCK_ADDR_INVALID = 0x73,
        UFR_VALUE_BLOCK_MANIPULATION_ERROR = 0x74,
        UFR_WRONG_UI_MODE = 0x75,
        UFR_KEYS_LOCKED = 0x76,
        UFR_KEYS_UNLOCKED = 0x77,
        UFR_WRONG_PASSWORD = 0x78,
        UFR_CAN_NOT_LOCK_DEVICE = 0x79,
        UFR_CAN_NOT_UNLOCK_DEVICE = 0x7A,
        UFR_DEVICE_EEPROM_BUSY = 0x7B,
        UFR_RTC_SET_ERROR = 0x7C,

        UFR_COMMUNICATION_BREAK = 0x50,
        UFR_NO_MEMORY_ERROR = 0x51,
        UFR_CAN_NOT_OPEN_READER = 0x52,
        UFR_READER_NOT_SUPPORTED = 0x53,
        UFR_READER_OPENING_ERROR = 0x54,
        UFR_READER_PORT_NOT_OPENED = 0x55,
        UFR_CANT_CLOSE_READER_PORT = 0x56,

        UFR_FT_STATUS_ERROR_1 = 0xA0,
        UFR_FT_STATUS_ERROR_2 = 0xA1,
        UFR_FT_STATUS_ERROR_3 = 0xA2,
        UFR_FT_STATUS_ERROR_4 = 0xA3,
        UFR_FT_STATUS_ERROR_5 = 0xA4,
        UFR_FT_STATUS_ERROR_6 = 0xA5,
        UFR_FT_STATUS_ERROR_7 = 0xA6,
        UFR_FT_STATUS_ERROR_8 = 0xA7,
        UFR_FT_STATUS_ERROR_9 = 0xA8,

        //NDEF error codes
        UFR_WRONG_NDEF_CARD_FORMAT = 0x80,
        UFR_NDEF_MESSAGE_NOT_FOUND = 0x81,
        UFR_NDEF_UNSUPPORTED_CARD_TYPE = 0x82,
        UFR_NDEF_CARD_FORMAT_ERROR = 0x83,
        UFR_MAD_NOT_ENABLED = 0x84,
        UFR_MAD_VERSION_NOT_SUPPORTED = 0x85,

        // multi units
        UFR_DEVICE_WRONG_HANDLE = 0x100,
        UFR_DEVICE_INDEX_OUT_OF_BOUND,
        UFR_DEVICE_ALREADY_OPENED,
        UFR_DEVICE_ALREADY_CLOSED,

        MAX_UFR_STATUS = 10000000,
        UNKNOWN_ERROR = 2147483647 // 0x7FFFFFFF
    };

    public static unsafe class ufCoder
    {
        //--------------------------------------------------------------------------------------------------
        public const string DLL_NAME = "uFCoder-x86_64.dll"; // for x64 target

        //--------------------------------------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpen")]
        public static extern DL_STATUS ReaderOpen();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpenEx")]
        private static extern DL_STATUS ReaderOpenEx(UInt32 reader_type, [In] byte[] port_name, UInt32 port_interface, [In] byte[] arg);
        public static DL_STATUS ReaderOpenEx(UInt32 reader_type, string port_name, UInt32 port_interface, string arg)
        {

            byte[] port_name_p = Encoding.ASCII.GetBytes(port_name);
            byte[] port_name_param = new byte[port_name_p.Length + 1];
            Array.Copy(port_name_p, 0, port_name_param, 0, port_name_p.Length);
            port_name_param[port_name_p.Length] = 0;

            byte[] arg_p = Encoding.ASCII.GetBytes(arg);
            byte[] arg_param = new byte[arg_p.Length + 1];
            Array.Copy(arg_p, 0, arg_param, 0, arg_p.Length);
            arg_param[arg_p.Length] = 0;

            return ReaderOpenEx(reader_type, port_name_param, port_interface, arg_param);
        }
        //--------------------------------------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderClose")]
        public static extern DL_STATUS ReaderClose();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderReset")]
        public static extern DL_STATUS ReaderReset();

        //--------------------------------------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetCardIdEx")]
        public static extern DL_STATUS GetCardIdEx(byte* bSak,
                                              byte* bCardUID,
                                              byte* bUidSize);
        //--------------------------------------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderUISignal")]
        public static extern DL_STATUS ReaderUISignal(byte light_signal_mode, byte beep_signal_mode); 

         [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderType")]
        public static extern DL_STATUS GetReaderType(uint* lpulReaderType);


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderSerialDescription")]
        public static extern DL_STATUS GetReaderSerialDescription(byte* pSerialDescription);

        //--------------------------------------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "read_ndef_record")]
        public static extern DL_STATUS read_ndef_record(byte message_nr, byte record_nr, byte* tnf, byte* type_record, byte* type_length, byte* id, byte* id_length, byte* payload, uint* payload_length);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "write_ndef_record")]
        public static extern DL_STATUS write_ndef_record(byte message_nr, out byte tnf, byte[] type_record, out byte type_length, byte[] id, out byte id_length,
           byte[] payload, out uint payload_length, out byte card_formated);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "write_ndef_record_mirroring")]
        public static extern DL_STATUS write_ndef_record_mirroring(byte message_nr,
                  byte* tnf, byte* type_record, byte* type_length, byte* id,
                  byte* id_length, byte* payload, UInt32* payload_length,
                  byte* card_formated, int use_uid_ascii_mirror, int use_counter_ascii_mirror, UInt32 payload_mirroring_pos);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "get_ndef_record_count")]
        public static extern DL_STATUS get_ndef_record_count(byte* ndef_message_cnt, byte* ndef_record_cnt, byte* ndef_record_array, byte* empty_ndef_message_cnt);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "erase_last_ndef_record")]
        public static extern DL_STATUS erase_last_ndef_record(byte message_nr);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "erase_all_ndef_records")]
        public static extern DL_STATUS erase_all_ndef_records(byte message_nr);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ndef_card_initialization")]
        public static extern DL_STATUS ndef_card_initialization();

        //---------------------------------------------------------------------
        // Card emulation:
        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteEmulationNdef")]
        public static extern DL_STATUS WriteEmulationNdef(byte tnf, byte* type_record, byte type_length, byte* id, byte id_length,
                                                            byte* payload, uint payload_length);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteEmulationNdefWithAAR")]
        public static extern DL_STATUS WriteEmulationNdefWithAAR(byte tnf, byte* type_record, byte type_length, byte* id, byte id_length,
                                                                 byte* payload, uint payload_length, byte* aar, byte aar_length);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "TagEmulationStart")]
        public static extern DL_STATUS TagEmulationStart();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "TagEmulationStop")]
        public static extern DL_STATUS TagEmulationStop();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "CombinedModeEmulationStart")]
        public static extern DL_STATUS CombinedModeEmulationStart();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "TagEmulationStartRam")]
        public static extern DL_STATUS TagEmulationStartRam();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "TagEmulationStopRam")]
        public static extern DL_STATUS TagEmulationStopRam();




        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDlogicCardType")]
        public static extern DL_STATUS GetDlogicCardType(byte* lpucCardType);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_UpdateAndGetCount")]
        public static extern DL_STATUS ReaderList_UpdateAndGetCount(Int32* NumberOfDevices);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetSerialByIndex")]
        public static extern DL_STATUS ReaderList_GetSerialByIndex(Int32 DeviceIndex, UInt32* lpulSerialNumber);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetSerialDesByIndex")]
        public static extern DL_STATUS ReaderList_GetSerialDescriptionByIndex(Int32 DeviceIndex, char* pSerialDescription);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetTypeByIndex")]
        public static extern DL_STATUS ReaderList_GetTypeByIndex(Int32 DeviceIndex, UInt32* lpulReaderType);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetFTDISerialByIndex")]
        public static extern DL_STATUS ReaderList_GetFTDISerialByIndex(Int32 DeviceIndex, char** Device_Serial);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_GetFTDIDescriptionByIndex")]
        public static extern DL_STATUS ReaderList_GetFTDIDescriptionByIndex(Int32 DeviceIndex, char** Device_Description);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderList_OpenByIndex")]
        public static extern DL_STATUS ReaderList_OpenByIndex(Int32 DeviceIndex, UFR_HANDLE* hndUFR);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpenM")]
        public static extern DL_STATUS ReaderOpenM(UFR_HANDLE hndUFR);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderCloseM")]
        public static extern DL_STATUS ReaderCloseM(UFR_HANDLE hndUFR);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetCardIdExM")]
        public static extern DL_STATUS GetCardIdExM(UFR_HANDLE hndUFR,
                                                    byte* bCardType,
                                                    byte* bCardUID,
                                                    byte* bUidSize);

        //[DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderTypeM")]
        //public static extern DL_STATUS GetReaderType(UInt32* get_reader_type);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "BlockRead_PKM")]
        public static extern DL_STATUS BlockRead_PKM(UFR_HANDLE hndUFR,
                                                  byte* data,
                                                  byte block_address,
                                                  byte auth_mode,
                                                  byte* key);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "BlockWrite_PKM")]
        public static extern DL_STATUS BlockWrite_PKM(UFR_HANDLE hndUFR,
                                                    byte* data,
                                                  byte block_address,
                                                  byte auth_mode,
                                                  byte* key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDllVersion")]
        public static extern uint GetDllVersion();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderHardwareVersion")]
        public static extern DL_STATUS GetReaderHardwareVersion(byte* version_major, byte* version_minor);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderFirmwareVersion")]
        public static extern DL_STATUS GetReaderFirmwareVersion(byte* version_major, byte* version_minor);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetBuildNumber")]
        public static extern DL_STATUS GetBuildNumber(byte* build);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "SectorTrailerWriteUnsafe_PK")]
        public static extern DL_STATUS SectorTrailerWriteUnsafe_PK(byte addressing_mode, byte address, byte* sector_trailer,
                                                  byte auth_mode, byte* key);
        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "LinearWrite_PK")]
        public static extern DL_STATUS LinearWrite_PK(byte* data, ushort linear_address, ushort length, ushort* bytes_written,
                                     byte auth_mode, byte* key);
        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "LinearWrite_AK1")]
        public static extern DL_STATUS LinearWrite_AK1(byte* data, ushort linear_address, ushort length, ushort* bytes_written,
                                     byte auth_mode);
        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "EE_Lock")]
        private static extern DL_STATUS Linkage_EE_Lock(StringBuilder password, UInt32 locked);
        public static DL_STATUS EE_Lock(String password, UInt32 locked)
        {
            if (password.Length != 8)
                return DL_STATUS.UFR_PARAMETERS_ERROR;

            StringBuilder ptr_password = new StringBuilder(password);
            return Linkage_EE_Lock(ptr_password, locked);
        }

        //---------------------------------------------------------------------
        // New WiFi Read Functions
        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_WiFi")]
        private static extern DL_STATUS WriteNdefRecord_WiFi(byte ndef_storage, [In] byte[] ssid, byte auth_type, byte encryption_type, [In] byte[] password);
        public static DL_STATUS WriteNdefRecord_WiFi(byte ndef_storage, string ssid, byte auth_type, byte encryption_type, string password)
        {
            byte[] ssid_p = Encoding.ASCII.GetBytes(ssid);
            byte[] ssid_param = new byte[ssid_p.Length + 1];
            Array.Copy(ssid_p, 0, ssid_param, 0, ssid_p.Length);
            ssid_param[ssid_p.Length] = 0;

            byte[] pw_p = Encoding.ASCII.GetBytes(password);
            byte[] pw_param = new byte[pw_p.Length + 1];
            Array.Copy(pw_p, 0, pw_param, 0, pw_p.Length);
            pw_param[pw_p.Length] = 0;

            return WriteNdefRecord_WiFi(ndef_storage, ssid_param, auth_type, encryption_type, pw_param);

        }

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_SMS")]
        private static extern DL_STATUS WriteNdefRecord_SMS(byte ndef_storage, [In] byte[] phone_number, [In] byte[] message);
        public static DL_STATUS WriteNdefRecord_SMS(byte ndef_storage, string phone_number, string message)
        {
            byte[] nr_p = Encoding.ASCII.GetBytes(phone_number);
            byte[] nr_param = new byte[nr_p.Length + 1];
            Array.Copy(nr_p, 0, nr_param, 0, nr_p.Length);
            nr_param[nr_p.Length] = 0;

            byte[] message_p = Encoding.ASCII.GetBytes(message);
            byte[] message_param = new byte[message_p.Length + 1];
            Array.Copy(message_p, 0, message_param, 0, message_p.Length);
            message_param[message_p.Length] = 0;
            
            return WriteNdefRecord_SMS(ndef_storage, nr_param, message_param);

        }

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_BT")]
        private static extern DL_STATUS WriteNdefRecord_BT(byte ndef_storage, [In] byte[] bt_mac_address);
        public static DL_STATUS WriteNdefRecord_BT(byte ndef_storage, string bt_mac_address)
        {
            byte[] bt_p = Encoding.ASCII.GetBytes(bt_mac_address);
            byte[] bt_param = new byte[bt_p.Length + 1];
            Array.Copy(bt_p, 0, bt_param, 0, bt_p.Length);
            bt_param[bt_p.Length] = 0;

            return WriteNdefRecord_BT(ndef_storage, bt_param);
        }
        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Bitcoin")]
        private static extern DL_STATUS WriteNdefRecord_Bitcoin(byte ndef_storage, [In] byte[] bitcoin_address, [In] byte[] ammount, [In] byte[] message);
        public static DL_STATUS WriteNdefRecord_Bitcoin(byte ndef_storage, string bitcoin_address, string ammount, string message)
        {
            byte[] bt_p = Encoding.ASCII.GetBytes(bitcoin_address);
            byte[] bt_param = new byte[bt_p.Length + 1];
            Array.Copy(bt_p, 0, bt_param, 0, bt_p.Length);
            bt_param[bt_p.Length] = 0;

            byte[] ammount_p = Encoding.ASCII.GetBytes(ammount);
            byte[] ammount_param = new byte[ammount_p.Length + 1];
            Array.Copy(ammount_p, 0, ammount_param, 0, ammount_p.Length);
            ammount_param[ammount_p.Length] = 0;

            byte[] message_p = Encoding.ASCII.GetBytes(message);
            byte[] message_param = new byte[message_p.Length + 1];
            Array.Copy(message_p, 0, message_param, 0, message_p.Length);
            message_param[message_p.Length] = 0;

            return WriteNdefRecord_Bitcoin(ndef_storage, bt_param, ammount_param, message_param);

        }

        //---------------------------------------------------------------------


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_GeoLocation")]
        private static extern DL_STATUS WriteNdefRecord_GeoLocation(byte ndef_storage, [In] byte[] latitude, [In] byte[] longitude);
        public static DL_STATUS WriteNdefRecord_GeoLocation(byte ndef_storage, string latitude, string longitude)
        {
            byte[] latitude_p = Encoding.ASCII.GetBytes(latitude);
            byte[] latitude_param = new byte[latitude_p.Length + 1];
            Array.Copy(latitude_p, 0, latitude_param, 0, latitude_p.Length);
            latitude_param[latitude_p.Length] = 0;

            byte[] longitude_p = Encoding.ASCII.GetBytes(longitude);
            byte[] longitude_param = new byte[longitude_p.Length + 1];
            Array.Copy(longitude_p, 0, longitude_param, 0, longitude_p.Length);
            longitude_param[longitude_p.Length] = 0;

            return WriteNdefRecord_GeoLocation(ndef_storage, latitude_param, longitude_param);

        }

        //---------------------------------------------------------------------


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_NaviDestination")]
        private static extern DL_STATUS WriteNdefRecord_NaviDestination(byte ndef_storage, [In] byte[] destination);
        public static DL_STATUS WriteNdefRecord_NaviDestination(byte ndef_storage, string destination)
    {
            byte[] destination_p = Encoding.ASCII.GetBytes(destination);
            byte[] destination_param = new byte[destination_p.Length + 1];
            Array.Copy(destination_p, 0, destination_param, 0, destination_p.Length);
            destination_param[destination_p.Length] = 0;

            return WriteNdefRecord_NaviDestination(ndef_storage, destination_param);

        }

        //---------------------------------------------------------------------


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Email")]
        private static extern DL_STATUS WriteNdefRecord_Email(byte ndef_storage, [In] byte[] email_address, [In] byte[] subject, [In] byte[] message);
        public static DL_STATUS WriteNdefRecord_Email(byte ndef_storage, string email_address, string subject, string message)
        {
            byte[] email_p = Encoding.ASCII.GetBytes(email_address);
            byte[] email_param = new byte[email_p.Length + 1];
            Array.Copy(email_p, 0, email_param, 0, email_p.Length);
            email_param[email_p.Length] = 0;

            byte[] subject_p = Encoding.ASCII.GetBytes(subject);
            byte[] subject_param = new byte[subject_p.Length + 1];
            Array.Copy(subject_p, 0, subject_param, 0, subject_p.Length);
            subject_param[subject_p.Length] = 0;

            byte[] message_p = Encoding.ASCII.GetBytes(message);
            byte[] message_param = new byte[message_p.Length + 1];
            Array.Copy(message_p, 0, message_param, 0, message_p.Length);
            message_param[message_p.Length] = 0;

            return WriteNdefRecord_Email(ndef_storage, email_param, subject_param, message_param);

        }

        //---------------------------------------------------------------------


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Address")]
        private static extern DL_STATUS WriteNdefRecord_Address(byte ndef_storage, [In] byte[] address);
        public static DL_STATUS WriteNdefRecord_Address(byte ndef_storage, string address)
        {
            byte[] address_p = Encoding.ASCII.GetBytes(address);
            byte[] address_param = new byte[address_p.Length + 1];
            Array.Copy(address_p, 0, address_param, 0, address_p.Length);
            address_param[address_p.Length] = 0;


            return WriteNdefRecord_Address(ndef_storage, address_param);

        }

        //---------------------------------------------------------------------


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_AndroidApp")]
        private static extern DL_STATUS WriteNdefRecord_AndroidApp(byte ndef_storage, [In] byte[] app_name);
        public static DL_STATUS WriteNdefRecord_AndroidApp(byte ndef_storage, string app_name)
        {
            byte[] app_name_p = Encoding.ASCII.GetBytes(app_name);
            byte[] app_name_param = new byte[app_name_p.Length + 1];
            Array.Copy(app_name_p, 0, app_name_param, 0, app_name_p.Length);
            app_name_param[app_name_p.Length] = 0;

            return WriteNdefRecord_AndroidApp(ndef_storage, app_name_param);

        }

        //---------------------------------------------------------------------


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Text")]
        private static extern DL_STATUS WriteNdefRecord_Text(byte ndef_storage, [In] byte[] text);
        public static DL_STATUS WriteNdefRecord_Text(byte ndef_storage, string text)
        {
            byte[] text_p = Encoding.ASCII.GetBytes(text);
            byte[] text_param = new byte[text_p.Length + 1];
            Array.Copy(text_p, 0, text_param, 0, text_p.Length);
            text_param[text_p.Length] = 0;

            return WriteNdefRecord_Text(ndef_storage, text_param);

        }

        //---------------------------------------------------------------------


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_StreetView")]
        private static extern DL_STATUS WriteNdefRecord_StreetView(byte ndef_storage, [In] byte[] latitude, [In] byte[] longitude);
        public static DL_STATUS WriteNdefRecord_StreetView(byte ndef_storage, string latitude, string longitude)
        {
            byte[] latitude_p = Encoding.ASCII.GetBytes(latitude);
            byte[] latitude_param = new byte[latitude_p.Length + 1];
            Array.Copy(latitude_p, 0, latitude_param, 0, latitude_p.Length);
            latitude_param[latitude_p.Length] = 0;

            byte[] longitude_p = Encoding.ASCII.GetBytes(longitude);
            byte[] longitude_param = new byte[longitude_p.Length + 1];
            Array.Copy(longitude_p, 0, longitude_param, 0, longitude_p.Length);
            longitude_param[longitude_p.Length] = 0;

            return WriteNdefRecord_StreetView(ndef_storage, latitude_param, longitude_param);

        }

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Skype")]
        private static extern DL_STATUS WriteNdefRecord_Skype(byte ndef_storage, [In] byte[] username, byte action);
        public static DL_STATUS WriteNdefRecord_Skype(byte ndef_storage, string username, byte action)
        {
            byte[] username_p = Encoding.ASCII.GetBytes(username);
            byte[] username_param = new byte[username_p.Length + 1];
            Array.Copy(username_p, 0, username_param, 0, username_p.Length);
            username_param[username_p.Length] = 0;

            return WriteNdefRecord_Skype(ndef_storage, username_param, action);

        }

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Whatsapp")]
        private static extern DL_STATUS WriteNdefRecord_Whatsapp(byte ndef_storage, [In] byte[] message);
        public static DL_STATUS WriteNdefRecord_Whatsapp(byte ndef_storage, string message)
        {
            byte[] message_p = Encoding.ASCII.GetBytes(message);
            byte[] message_param = new byte[message_p.Length + 1];
            Array.Copy(message_p, 0, message_param, 0, message_p.Length);
            message_param[message_p.Length] = 0;

            return WriteNdefRecord_Whatsapp(ndef_storage, message_param);

        }

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Viber")]
        private static extern DL_STATUS WriteNdefRecord_Viber(byte ndef_storage, [In] byte[] message);
        public static DL_STATUS WriteNdefRecord_Viber(byte ndef_storage, string message)
        {
            byte[] message_p = Encoding.ASCII.GetBytes(message);
            byte[] message_param = new byte[message_p.Length + 1];
            Array.Copy(message_p, 0, message_param, 0, message_p.Length);
            message_param[message_p.Length] = 0;

            return WriteNdefRecord_Viber(ndef_storage, message_param);

        }

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Contact")]
        private static extern DL_STATUS WriteNdefRecord_Contact(byte ndef_storage, [In] byte[] name, [In] byte[] company, [In] byte[] address, [In] byte[] phone, [In] byte[] email, [In] byte[] website);
        public static DL_STATUS WriteNdefRecord_Contact(byte ndef_storage, string name, string company, string address, string phone, string email, string website)
        {
            byte[] name_p = Encoding.ASCII.GetBytes(name);
            byte[] name_param = new byte[name_p.Length + 1];
            Array.Copy(name_p, 0, name_param, 0, name_p.Length);
            name_param[name_p.Length] = 0;

            byte[] company_p = Encoding.ASCII.GetBytes(company);
            byte[] company_param = new byte[company_p.Length + 1];
            Array.Copy(company_p, 0, company_param, 0, company_p.Length);
            company_param[company_p.Length] = 0;

            byte[] address_p = Encoding.ASCII.GetBytes(address);
            byte[] address_param = new byte[address_p.Length + 1];
            Array.Copy(address_p, 0, address_param, 0, address_p.Length);
            address_param[address_p.Length] = 0;

            byte[] phone_p = Encoding.ASCII.GetBytes(phone);
            byte[] phone_param = new byte[phone_p.Length + 1];
            Array.Copy(phone_p, 0, phone_param, 0, phone_p.Length);
            phone_param[phone_p.Length] = 0;

            byte[] email_p = Encoding.ASCII.GetBytes(email);
            byte[] email_param = new byte[email_p.Length + 1];
            Array.Copy(email_p, 0, email_param, 0, email_p.Length);
            email_param[email_p.Length] = 0;

            byte[] website_p = Encoding.ASCII.GetBytes(website);
            byte[] website_param = new byte[website_p.Length + 1];
            Array.Copy(website_p, 0, website_param, 0, website_p.Length);
            website_param[website_p.Length] = 0;

            return WriteNdefRecord_Contact(ndef_storage, name_param, company_param, address_param, phone_param, email_param, website_param);

        }

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteNdefRecord_Phone")]
        private static extern DL_STATUS WriteNdefRecord_Phone(byte ndef_storage, [In] byte[] phone_number);
        public static DL_STATUS WriteNdefRecord_Phone(byte ndef_storage, string phone_number)
        {
            byte[] phone_p = Encoding.ASCII.GetBytes(phone_number);
            byte[] phone_param = new byte[phone_p.Length + 1];
            Array.Copy(phone_p, 0, phone_param, 0, phone_p.Length);
            phone_param[phone_p.Length] = 0;

            return WriteNdefRecord_Phone(ndef_storage, phone_param);

        }

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "WriteEmulationNdefRam")]
        public static extern DL_STATUS WriteEmulationNdefRam(byte tnf, byte[] type_record, byte type_length, byte[] id, byte id_length, byte[] payload, uint payload_length);

        //---------------------------------------------------------------------
        // New WiFi Read Functions
        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_WiFi")]
        public static extern DL_STATUS ReadNdefRecord_WiFi(byte[] ssid, byte[] auth_type, byte[] encryption_type, byte[] password);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Bitcoin")]
        public static extern DL_STATUS ReadNdefRecord_Bitcoin(byte[] bitcoin_address, byte[] ammount, byte[] message);


        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_GeoLocation")]
        public static extern DL_STATUS ReadNdefRecord_GeoLocation(byte[] latitude, byte[] longitude);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_NaviDestination")]
        public static extern DL_STATUS ReadNdefRecord_NaviDestination(byte[] destination);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Email")]
        public static extern DL_STATUS ReadNdefRecord_Email(byte[] email_address, byte[] subject, byte[] message);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Address")]
        public static extern DL_STATUS ReadNdefRecord_Address(byte[] address);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_AndroidApp")]
        public static extern DL_STATUS ReadNdefRecord_AndroidApp(byte[] pkg_name);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Text")]
        public static extern DL_STATUS ReadNdefRecord_Text(byte[] text);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_StreetView")]
        public static extern DL_STATUS ReadNdefRecord_StreetView(byte[] latitude, byte[] longitude);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Skype")]
        public static extern DL_STATUS ReadNdefRecord_Skype(byte[] username, byte[] action);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Whatsapp")]
        public static extern DL_STATUS ReadNdefRecord_Whatsapp(byte[] message);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Viber")]
        public static extern DL_STATUS ReadNdefRecord_Viber(byte[] message);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Contact")]
        public static extern DL_STATUS ReadNdefRecord_Contact(byte[] vCard);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_Phone")]
        public static extern DL_STATUS ReadNdefRecord_Phone(byte[] phone);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_SMS")]
        public static extern DL_STATUS ReadNdefRecord_SMS(byte[] phone_number, byte[] message);

        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReadNdefRecord_BT")]
        public static extern DL_STATUS ReadNdefRecord_BT(byte[] bt_mac_address);


        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "BlockRead_PK")]
        public static extern DL_STATUS BlockRead_PK(byte[] data, byte block_address, byte auth_mode, byte[] key);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "BlockWrite_PK")]
        public static extern DL_STATUS BlockWrite_PK(byte[] data, byte block_address, byte auth_mode, byte[] key);

    }
}

