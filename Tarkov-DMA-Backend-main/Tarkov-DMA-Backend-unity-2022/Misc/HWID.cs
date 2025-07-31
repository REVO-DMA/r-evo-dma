using SharedMemory;
using System.Security.Cryptography;
using Tarkov_DMA_Backend.LowLevel;
using static TerraFX.Interop.Windows.Windows;

namespace Tarkov_DMA_Backend.Misc
{
    public static unsafe class HWID
    {
        public static string GetHWID()
        {
            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

            HWID_Components components;

            string driveSerial = GetDriveSerial();
            string cpuID = GetCpuId();

            var data = SMBIOS.GetData();
            if (data)
            {
                using var smbiosDataRaw = data.Value;
                var smbiosData = (SMBIOS.Raw_SMBIOS_Data*)smbiosDataRaw.Address;
                var baseboardIDs = SMBIOS.GetBaseboardIDs(smbiosData);
                if (baseboardIDs)
                {
                    var baseboard = baseboardIDs.Value;
                    components = new(driveSerial, cpuID, baseboard.SerialNumber, baseboard.UUID);
                }
                else
                    components = new(driveSerial, cpuID);
            }
            else
                components = new(driveSerial, cpuID);

            string hwid = HashHWID(components);

            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();

            return hwid;
        }

        private readonly struct HWID_Components(
            string driveSerial,
            string cpuID,
            string baseboardSerial = null,
            string baseboardUUID = null)
        {
            public readonly string DriveSerial = driveSerial;
            public readonly string CPU_ID = cpuID;
            public readonly string BaseboardSerial = baseboardSerial;
            public readonly string BaseboardUUID = baseboardUUID;
        }

        private static string HashHWID(HWID_Components components)
        {
            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

            StringBuilder sb = new();
            sb.Append(components.DriveSerial);
            sb.Append(components.CPU_ID);
            if (components.BaseboardSerial != null)
                sb.Append(components.BaseboardSerial);
            if (components.BaseboardUUID != null)
                sb.Append(components.BaseboardUUID);

            string combinedComponents = sb.ToString();
            if (combinedComponents.Length < 4)
                return "";

            byte[] combinedHWID = Encoding.UTF8.GetBytes(combinedComponents);
            byte[] hwidHash = MD5.HashData(combinedHWID);

            string base4HWID = Convert.ToBase64String(hwidHash);

            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();

            return base4HWID;
        }

        private static string GetDriveSerial()
        {
            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

            uint hddSerial;

            string pDriveLetter = Environment.GetFolderPath(Environment.SpecialFolder.System).Split(':')[0];
            using PChar rootPathName = new($"{pDriveLetter}://");
            GetVolumeInformationW(rootPathName, null, NULL, &hddSerial, null, null, null, NULL);

            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();

            return $"{hddSerial:X}";
        }

        private static string GetCpuId()
        {
            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

            if (!X86Base.IsSupported)
                return "";

            var (eax, ebx, ecx, edx) = X86Base.CpuId(0, 0);
            using SharedPinned<int> cpuId = new(4);
            cpuId.Span[0] = eax;
            cpuId.Span[1] = ebx;
            cpuId.Span[2] = ecx;
            cpuId.Span[3] = edx;

            ushort final = 0;
            for (int i = 0; i < 8; i++)
            {
                var p = (ushort*)cpuId.Address;
                final += p[i];
            }

            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();

            return $"{final:X}";
        }

        
    }
}
