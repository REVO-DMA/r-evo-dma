using SharedMemory;
using System.Collections.Frozen;
using static TerraFX.Interop.Windows.Windows;

namespace Tarkov_DMA_Backend.Misc
{
    public static unsafe class SMBIOS
    {
        #region Class Data

        private static readonly FrozenDictionary<DMI_Memory_Device_Type, string> MemoryDeviceTypes = new Dictionary<DMI_Memory_Device_Type, string>()
        {
            { DMI_Memory_Device_Type.DMT_OTHER, "Other" },
            { DMI_Memory_Device_Type.DMT_UNKNOWN, "Unknown" },
            { DMI_Memory_Device_Type.DMT_DRAM, "DRAM" },
            { DMI_Memory_Device_Type.DMT_EDRAM, "EDRAM" },
            { DMI_Memory_Device_Type.DMT_VRAM, "VRAM" },
            { DMI_Memory_Device_Type.DMT_SRAM, "SRAM" },
            { DMI_Memory_Device_Type.DMT_RAM, "RAM" },
            { DMI_Memory_Device_Type.DMT_ROM, "ROM" },
            { DMI_Memory_Device_Type.DMT_FLASH, "Flash" },
            { DMI_Memory_Device_Type.DMT_EEPROM, "EEPROM" },
            { DMI_Memory_Device_Type.DMT_FEPROM, "FEPROM" },
            { DMI_Memory_Device_Type.DMT_EPROM, "EPROM" },
            { DMI_Memory_Device_Type.DMT_CDRAM, "CDRAM" },
            { DMI_Memory_Device_Type.DMT_3DRAM, "3DRAM" },
            { DMI_Memory_Device_Type.DMT_SDRAM, "SDRAM" },
            { DMI_Memory_Device_Type.DMT_SGRAM, "SGRAM" },
            { DMI_Memory_Device_Type.DMT_RDRAM, "RDRAM" },
            { DMI_Memory_Device_Type.DMT_DDR, "DDR" },
            { DMI_Memory_Device_Type.DMT_DDR2, "DDR2" },
            { DMI_Memory_Device_Type.DMT_DDR2_FB_DIMM, "DDR2 FB-DIMM" },
            { DMI_Memory_Device_Type.DMT_RESERVED1, "Reserved" },
            { DMI_Memory_Device_Type.DMT_RESERVED2, "Reserved" },
            { DMI_Memory_Device_Type.DMT_RESERVED3, "Reserved" },
            { DMI_Memory_Device_Type.DMT_DDR3, "DDR3" },
            { DMI_Memory_Device_Type.DMT_FBD2, "FBD2" },
            { DMI_Memory_Device_Type.DMT_DDR4, "DDR4" },
            { DMI_Memory_Device_Type.DMT_LPDDR, "LPDDR" },
            { DMI_Memory_Device_Type.DMT_LPDDR2, "LPDDR2" },
            { DMI_Memory_Device_Type.DMT_LPDDR3, "LPDDR3" },
            { DMI_Memory_Device_Type.DMT_LPDDR4, "LPDDR4" },
            { DMI_Memory_Device_Type.DMT_LOGICAL_NONVOLATILE_DEVICE, "Logical non-volatile device" },
            { DMI_Memory_Device_Type.DMT_HBM, "HBM" },
            { DMI_Memory_Device_Type.DMT_HBM2, "HBM2" },
        }.ToFrozenDictionary();

        public readonly struct BaseboardIDs(string serialNumber, string uuid)
        {
            public readonly string SerialNumber = serialNumber;
            public readonly string UUID = uuid;
        }

        public struct Raw_SMBIOS_Data
        {
            public byte Used20CallingMethod;
            public byte SMBIOSMajorVersion;
            public byte SMBIOSMinorVersion;
            public byte DmiRevision;
            public uint Length;
            public fixed byte SMBIOSTableData[0x10000];
        }

        private struct DMI_Header
        {
            public byte type;
            public byte length;
            public ushort handle;
        }

        private enum DMI_Entry_Type
        {
            DMI_ENTRY_BIOS = 0,
            DMI_ENTRY_SYSTEM,
            DMI_ENTRY_BASEBOARD,
            DMI_ENTRY_CHASSIS,
            DMI_ENTRY_PROCESSOR,
            DMI_ENTRY_MEM_CONTROLLER,
            DMI_ENTRY_MEM_MODULE,
            DMI_ENTRY_CACHE,
            DMI_ENTRY_PORT_CONNECTOR,
            DMI_ENTRY_SYSTEM_SLOT,
            DMI_ENTRY_ONBOARD_DEVICE,
            DMI_ENTRY_OEMSTRINGS,
            DMI_ENTRY_SYSCONF,
            DMI_ENTRY_BIOS_LANG,
            DMI_ENTRY_GROUP_ASSOC,
            DMI_ENTRY_SYSTEM_EVENT_LOG,
            DMI_ENTRY_PHYS_MEM_ARRAY,
            DMI_ENTRY_MEM_DEVICE,
            DMI_ENTRY_32_MEM_ERROR,
            DMI_ENTRY_MEM_ARRAY_MAPPED_ADDR,
            DMI_ENTRY_MEM_DEV_MAPPED_ADDR,
            DMI_ENTRY_BUILTIN_POINTING_DEV,
            DMI_ENTRY_PORTABLE_BATTERY,
            DMI_ENTRY_SYSTEM_RESET,
            DMI_ENTRY_HW_SECURITY,
            DMI_ENTRY_SYSTEM_POWER_CONTROLS,
            DMI_ENTRY_VOLTAGE_PROBE,
            DMI_ENTRY_COOLING_DEV,
            DMI_ENTRY_TEMP_PROBE,
            DMI_ENTRY_ELECTRICAL_CURRENT_PROBE,
            DMI_ENTRY_OOB_REMOTE_ACCESS,
            DMI_ENTRY_BIS_ENTRY,
            DMI_ENTRY_SYSTEM_BOOT,
            DMI_ENTRY_MGMT_DEV,
            DMI_ENTRY_MGMT_DEV_COMPONENT,
            DMI_ENTRY_MGMT_DEV_THRES,
            DMI_ENTRY_MEM_CHANNEL,
            DMI_ENTRY_IPMI_DEV,
            DMI_ENTRY_SYS_POWER_SUPPLY,
            DMI_ENTRY_ADDITIONAL,
            DMI_ENTRY_ONBOARD_DEV_EXT,
            DMI_ENTRY_MGMT_CONTROLLER_HOST,
            DMI_ENTRY_INACTIVE = 126,
            DMI_ENTRY_END_OF_TABLE = 127,
        }

        private enum DMI_Memory_Device_Type
        {
            DMT_OTHER = 0x01, /* 0x01 */
            DMT_UNKNOWN,
            DMT_DRAM,
            DMT_EDRAM,
            DMT_VRAM,
            DMT_SRAM,
            DMT_RAM,
            DMT_ROM,
            DMT_FLASH,
            DMT_EEPROM,
            DMT_FEPROM,
            DMT_EPROM,
            DMT_CDRAM,
            DMT_3DRAM,
            DMT_SDRAM,
            DMT_SGRAM,
            DMT_RDRAM,
            DMT_DDR,
            DMT_DDR2,
            DMT_DDR2_FB_DIMM,
            DMT_RESERVED1,
            DMT_RESERVED2,
            DMT_RESERVED3,
            DMT_DDR3,
            DMT_FBD2,
            DMT_DDR4,
            DMT_LPDDR,
            DMT_LPDDR2,
            DMT_LPDDR3,
            DMT_LPDDR4,
            DMT_LOGICAL_NONVOLATILE_DEVICE,
            DMT_HBM,
            DMT_HBM2 /* 0x21 */
        }

        #endregion

        public static Result<SharedPinned<byte>> GetData()
        {
            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

            uint smbios_data_size;
            SharedPinned<byte> smbios_data = new(sizeof(Raw_SMBIOS_Data));

            smbios_data_size = GetSystemFirmwareTable(new XDWORD("RSMB"), 0, null, 0);
            if (smbios_data_size == 0)
                return Result<SharedPinned<byte>>.Fail;

            smbios_data_size = GetSystemFirmwareTable(new XDWORD("RSMB"), 0, smbios_data.Address, smbios_data_size);
            if (smbios_data_size == 0)
                return Result<SharedPinned<byte>>.Fail;

            if (((Raw_SMBIOS_Data*)smbios_data.Address)->Length != smbios_data_size - 8)
                return Result<SharedPinned<byte>>.Fail;

            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();

            return new(true, smbios_data);
        }

        public static Result<BaseboardIDs> GetBaseboardIDs(Raw_SMBIOS_Data* smbiosData)
        {
            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

            byte* b_ = smbiosData->SMBIOSTableData;
            for (uint index = 0; index < smbiosData->Length; index++)
            {
                var header = (DMI_Header*)b_;

                if (header->type == (byte)DMI_Entry_Type.DMI_ENTRY_BASEBOARD)
                {
                    string serialNumber = GetString(header, b_[0x7]);

                    var version = (short)(smbiosData->SMBIOSMajorVersion * 0x100 + smbiosData->SMBIOSMinorVersion);
                    string uuid = GetUUID(b_ + 0x8, version);

                    return new(true, new(serialNumber, uuid));
                }

                b_ += header->length;
                while ((*(ushort*)b_) != 0) { b_++; }
                b_ += 2;
            }

            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();

            return Result<BaseboardIDs>.Fail;
        }

        public static Result<BaseboardIDs> GetMemoryIDs(Raw_SMBIOS_Data* smbiosData)
        {
            bool flag = true;
            byte* b_ = smbiosData->SMBIOSTableData;
            for (uint index = 0; index < smbiosData->Length; index++)
            {
                var header = (DMI_Header*)b_;

                if (header->type == (byte)DMI_Entry_Type.DMI_ENTRY_BIOS && flag)
                {
                    Logger.WriteLine($"\n\tType  {header->type} - [Bios Device type]\n");
                    Logger.WriteLine($"\t\tBIOS Vendor:    {GetString(header, b_[0x4])}");
                    Logger.WriteLine($"\t\tBIOS Version:   {GetString(header, b_[0x5])}");
                    Logger.WriteLine($"\t\tRelease data:   {GetString(header, b_[0x8])}");

                    flag = false;
                }
                else if (header->type == (byte)DMI_Entry_Type.DMI_ENTRY_BASEBOARD)
                {
                    Logger.WriteLine($"\n\tType  {header->type} - [Baseboard Device type]\n");
                    Logger.WriteLine($"\t\tManufacturer:   {GetString(header, b_[0x4])}");
                    Logger.WriteLine($"\t\tProduct Number: {GetString(header, b_[0x5])}");
                    Logger.WriteLine($"\t\tVersion:        {GetString(header, b_[0x6])}");
                    Logger.WriteLine($"\t\tSerial Number:  {GetString(header, b_[0x7])}");
                    Logger.Write($"\t\tUUID:           ");
                    var version = (short)(smbiosData->SMBIOSMajorVersion * 0x100 + smbiosData->SMBIOSMinorVersion);
                    GetUUID(b_ + 0x8, version);
                }
                else if (header->type == (byte)DMI_Entry_Type.DMI_ENTRY_MEM_DEVICE)
                {
                    Logger.WriteLine($"\n\tType  {header->type} - [Memory Device type]\n");
                    Logger.WriteLine($"\t\tMemory Type:    {GetMemoryType(b_[0x12])}");
                    Logger.WriteLine($"\t\tSize of RAM:    {GetString(header, b_[0xC])}");
                    Logger.WriteLine($"\t\tManufacturer:   {GetString(header, b_[0x17])}");
                    Logger.WriteLine($"\t\tSerial Number:  {GetString(header, b_[0x18])}");
                }

                b_ += header->length;
                while ((*(ushort*)b_) != 0) { b_++; }
                b_ += 2;
            }

            return Result<BaseboardIDs>.Fail;
        }

        private static int StrLen(byte* str)
        {
            int length = 0;
            while (*str != 0)
            {
                length++;
                str++;
            }

            return length;
        }

        private static string GetString(DMI_Header* dm, byte src)
        {
            byte* bp = (byte*)dm;

            if (src == 0)
                return "Not specified";

            bp += dm->length;

            while (src > 1 && *bp != 0)
            {
                bp += StrLen(bp);
                bp++;
                src--;
            }

            if (*bp == 0)
                return "BAD_INDEXING";

            int length = StrLen(bp);
            for (int i = 0; i < length; i++)
            {
                if (bp[i] < 32 || bp[i] == 127)
                {
                    bp[i] = (byte)'.';
                }
            }

            return new string((sbyte*)bp);
        }

        private static string GetUUID(byte* p, short version)
        {
            bool only0xff = true;
            bool only0x00 = true;

            for (int i = 0; i < 16 && (only0x00 || only0xff); i++)
            {
                if (p[i] != 0x00) only0x00 = false;
                if (p[i] != 0xFF) only0xff = false;
            }

            if (only0x00 || only0xff)
                return null;

            if (version >= 0x0206)
                return $"{p[3]:X2}{p[2]:X2}{p[1]:X2}{p[0]:X2}-{p[5]:X2}{p[4]:X2}-{p[7]:X2}{p[6]:X2}-{p[8]:X2}{p[9]:X2}-{p[10]:X2}{p[11]:X2}{p[12]:X2}{p[13]:X2}{p[14]:X2}{p[15]:X2}";
            else
                return $"-{p[0]:X2}{p[1]:X2}{p[2]:X2}{p[3]:X2}-{p[4]:X2}{p[5]:X2}-{p[6]:X2}{p[7]:X2}-{p[8]:X2}{p[9]:X2}-{p[10]:X2}{p[11]:X2}{p[12]:X2}{p[13]:X2}{p[14]:X2}{p[15]:X2}";
        }

        private static string GetMemoryType(byte code)
        {
            if (code >= (byte)DMI_Memory_Device_Type.DMT_OTHER &&
                code <= (byte)DMI_Memory_Device_Type.DMT_HBM2)
            {
                if (MemoryDeviceTypes.TryGetValue((DMI_Memory_Device_Type)(code - 0x1), out string memoryType))
                    return memoryType;
            }

            return "OUT OF SPEC";
        }
    }
}
