#include <stdio.h>
#include <conio.h>
#include <Windows.h>

#include "xorstr.h"

#define VERSION							xorstr_("1.2")

#define CmResourceTypeMemory            3   // ResType_Mem (0x0001)
#define CmResourceTypeMemoryLarge       7   // ResType_MemLarge (0x0007)

#define CM_RESOURCE_MEMORY_LARGE        0x0E00
#define CM_RESOURCE_MEMORY_LARGE_40     0x0200
#define CM_RESOURCE_MEMORY_LARGE_48     0x0400
#define CM_RESOURCE_MEMORY_LARGE_64     0x0800

typedef LARGE_INTEGER PHYSICAL_ADDRESS, * PPHYSICAL_ADDRESS;

#pragma pack(push,4)
typedef struct _CM_PARTIAL_RESOURCE_DESCRIPTOR {
	UCHAR Type;
	UCHAR ShareDisposition;
	USHORT Flags;
	union {
		struct {
			PHYSICAL_ADDRESS Start;
			ULONG Length;
		} Generic;

		struct {
			PHYSICAL_ADDRESS Start;
			ULONG Length;
		} Port;

		struct {
#if defined(NT_PROCESSOR_GROUPS)
			USHORT Level;
			USHORT Group;
#else
			ULONG Level;
#endif
			ULONG Vector;
			KAFFINITY Affinity;
		} Interrupt;

		struct {
			union {
				struct {
#if defined(NT_PROCESSOR_GROUPS)
					USHORT Group;
#else
					USHORT Reserved;
#endif
					USHORT MessageCount;
					ULONG Vector;
					KAFFINITY Affinity;
				} Raw;

				struct {
#if defined(NT_PROCESSOR_GROUPS)
					USHORT Level;
					USHORT Group;
#else
					ULONG Level;
#endif
					ULONG Vector;
					KAFFINITY Affinity;
				} Translated;
			} DUMMYUNIONNAME;
		} MessageInterrupt;

		struct {
			PHYSICAL_ADDRESS Start;
			ULONG Length;
		} Memory;

		struct {
			ULONG Channel;
			ULONG Port;
			ULONG Reserved1;
		} Dma;

		struct {
			ULONG Channel;
			ULONG RequestLine;
			UCHAR TransferWidth;
			UCHAR Reserved1;
			UCHAR Reserved2;
			UCHAR Reserved3;
		} DmaV3;

		struct {
			ULONG Data[3];
		} DevicePrivate;

		struct {
			ULONG Start;
			ULONG Length;
			ULONG Reserved;
		} BusNumber;

		struct {
			ULONG DataSize;
			ULONG Reserved1;
			ULONG Reserved2;
		} DeviceSpecificData;

		struct {
			PHYSICAL_ADDRESS Start;
			ULONG Length40;
		} Memory40;

		struct {
			PHYSICAL_ADDRESS Start;
			ULONG Length48;
		} Memory48;

		struct {
			PHYSICAL_ADDRESS Start;
			ULONG Length64;
		} Memory64;

		struct {
			UCHAR Class;
			UCHAR Type;
			UCHAR Reserved1;
			UCHAR Reserved2;
			ULONG IdLowPart;
			ULONG IdHighPart;
		} Connection;

	} u;
} CM_PARTIAL_RESOURCE_DESCRIPTOR, * PCM_PARTIAL_RESOURCE_DESCRIPTOR;
#pragma pack(pop,4)

typedef enum _INTERFACE_TYPE {
	InterfaceTypeUndefined,
	Internal,
	Isa,
	Eisa,
	MicroChannel,
	TurboChannel,
	PCIBus,
	VMEBus,
	NuBus,
	PCMCIABus,
	CBus,
	MPIBus,
	MPSABus,
	ProcessorInternal,
	InternalPowerBus,
	PNPISABus,
	PNPBus,
	Vmcs,
	ACPIBus,
	MaximumInterfaceType
} INTERFACE_TYPE, * PINTERFACE_TYPE;

typedef struct _CM_PARTIAL_RESOURCE_LIST {
	USHORT                         Version;
	USHORT                         Revision;
	ULONG                          Count;
	CM_PARTIAL_RESOURCE_DESCRIPTOR PartialDescriptors[1];
} CM_PARTIAL_RESOURCE_LIST, * PCM_PARTIAL_RESOURCE_LIST;

typedef struct _CM_FULL_RESOURCE_DESCRIPTOR {
	INTERFACE_TYPE           InterfaceType;
	ULONG                    BusNumber;
	CM_PARTIAL_RESOURCE_LIST PartialResourceList;
} *PCM_FULL_RESOURCE_DESCRIPTOR, CM_FULL_RESOURCE_DESCRIPTOR;

typedef struct _CM_RESOURCE_LIST {
	ULONG                       Count;
	CM_FULL_RESOURCE_DESCRIPTOR List[1];
} *PCM_RESOURCE_LIST, CM_RESOURCE_LIST;

BOOL Memory(PCM_RESOURCE_LIST pcrl, ULONG size, FILE* fp)
{
	if (size < FIELD_OFFSET(CM_RESOURCE_LIST, List))
		return FALSE;

	size -= FIELD_OFFSET(CM_RESOURCE_LIST, List);

	if (ULONG Count = pcrl->Count)
	{
		PCM_FULL_RESOURCE_DESCRIPTOR List = pcrl->List;

		do
		{
			if (size < FIELD_OFFSET(CM_FULL_RESOURCE_DESCRIPTOR, PartialResourceList.PartialDescriptors))
				return FALSE;

			size -= FIELD_OFFSET(CM_FULL_RESOURCE_DESCRIPTOR, PartialResourceList.PartialDescriptors);

			if (ULONG n = List->PartialResourceList.Count)
			{
				PCM_PARTIAL_RESOURCE_DESCRIPTOR PartialDescriptors = List->PartialResourceList.PartialDescriptors;

				do
				{
					if (size < sizeof(CM_PARTIAL_RESOURCE_DESCRIPTOR))
						return FALSE;

					size -= sizeof(CM_PARTIAL_RESOURCE_DESCRIPTOR);

					ULONG64 Length = PartialDescriptors->u.Memory.Length;

					switch (PartialDescriptors->Type)
					{
						case CmResourceTypeMemoryLarge:
							switch (PartialDescriptors->Flags & (CM_RESOURCE_MEMORY_LARGE_40 | CM_RESOURCE_MEMORY_LARGE_48 | CM_RESOURCE_MEMORY_LARGE_64))
							{
								case CM_RESOURCE_MEMORY_LARGE_40:
									Length <<= 8;
									break;
								case CM_RESOURCE_MEMORY_LARGE_48:
									Length <<= 16;
									break;
								case CM_RESOURCE_MEMORY_LARGE_64:
									Length <<= 32;
									break;
								default:
									printf(xorstr_("[!] Unknown memory type!\n"));
									continue;
							}
						case CmResourceTypeMemory:
						{
							auto start = PartialDescriptors->u.Memory.Start.QuadPart;
							auto end = (PartialDescriptors->u.Memory.Start.QuadPart + Length) - 0x1;

							printf(xorstr_("[+] Added memory region: %.16llX - %.16llX\n"), start, end);
							fprintf(fp, xorstr_("%.16llX - %.16llX\n"), start, end);

							break;
						}
					}

				} while (PartialDescriptors++, --n);
			}

		} while (List++, --Count);
	}

	return size == 0;
}

ULONG Memory(FILE* fp)
{
	printf(xorstr_("[i] Getting physical memory regions...\n\n"));

	HKEY hKey;
	LSTATUS dwError = RegOpenKeyExW(HKEY_LOCAL_MACHINE, xorstr_(L"Hardware\\ResourceMap\\System Resources\\Physical Memory"), 0, KEY_READ, &hKey);

	if (dwError == NOERROR)
	{
		ULONG cb = 0x100;

		do
		{
			dwError = ERROR_NO_SYSTEM_RESOURCES;

			union {
				PVOID buf;
				PBYTE pb;
				PCM_RESOURCE_LIST pcrl;
			};

			if (buf = LocalAlloc(0, cb))
			{
				ULONG dwType;
				if ((dwError = RegQueryValueExW(hKey, xorstr_(L".Translated"), 0, &dwType, pb, &cb)) == NOERROR)
				{
					if (dwType == REG_RESOURCE_LIST)
					{
						if (!Memory(pcrl, cb, fp))
							printf(xorstr_("[!] Error parsing resource list!\n"));
						else
							dwError = ERROR_INVALID_DATATYPE;
					}
				}

				LocalFree(buf);
			}

		} while (dwError == ERROR_MORE_DATA);

		RegCloseKey(hKey);
	}

	return dwError;
}

int main()
{
	printf(xorstr_("[i] Starting EVO DMA Memory Mapper v")); printf(VERSION); printf(xorstr_("...\n"));

	printf(xorstr_("[i] Ensure all games are closed before proceeding.\n\n"));

	printf(xorstr_("Press any key to continue...\n\n"));
	_getch();

	FILE* fp = nullptr;
	errno_t err = fopen_s(&fp, xorstr_("mmap.txt"), xorstr_("w"));
	if (err != 0 || !fp) {
		printf(xorstr_("\n[!] Failed to create mmap.txt. Please make sure you have permission to write to this folder.\n"));
		return 1;
	}

	Memory(fp);
	
	fclose(fp);

	printf(xorstr_("\n[i] mmap.txt generation complete!\n\n"));

	printf(xorstr_("Next steps:\n"));
	printf(xorstr_("1. Copy the generated mmap.txt to the Radar PC.\n"));
	printf(xorstr_("2. Place the mmap.txt file in the \"C:\\Users\\YOUR_USER\\EVO DMA\\GAME_NAME\" folder. DO NOT change the name from mmap.txt!\n"));
	printf(xorstr_("3. You may now launch EVO!\n\n"));

	printf(xorstr_("Press any key to exit..."));
	_getch();

	return 0;
}