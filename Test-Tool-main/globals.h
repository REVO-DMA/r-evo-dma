#pragma once

#include <windows.h>
#include <fstream>
#include <filesystem>
#include <conio.h>
#include <numeric>
#include <random>
#include <vector>
#include <chrono>

#include <wininet.h>
#pragma comment(lib,"wininet.lib")

#include "vendor/libversion/version.h"
#pragma comment(lib,"vendor/libversion/libversion.lib")

#include "leechcore/leechcore.h"
#pragma comment(lib,"vendor/dma/leechcore.lib")

#include "vmm/vmmdll.h"
#include "vmm/vmm.h"
#pragma comment(lib,"vendor/dma/vmm.lib")

#include "vendor/color.h"
#include "vendor/xorstr.h"

#include "src/settings.h"
#include "src/utils.h"
#include "src/updater.h"
#include "src/printer.h"
#include "src/dma.h"
#include "src/rps.h"
#include "src/throughput.h"