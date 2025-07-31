#pragma once

namespace settings
{
	static bool isVerbose = false;
	static std::string verbosityString = xorstr_("");
	static std::string fpgaAlg = xorstr_("-1");
	static bool usingMMAP = false;
	static int SpeedTestReadSize = 1;
	static int SpeedTestDuration = 10;
}