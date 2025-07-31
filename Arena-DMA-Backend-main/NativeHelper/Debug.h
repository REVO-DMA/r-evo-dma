#pragma once

namespace Debug
{
	bool IsDebug = false;

	const std::string newLine = "\n";

	std::string ToUppercase(std::string string)
	{
		std::transform(string.begin(), string.end(), string.begin(), [](unsigned char c) { return std::toupper(c); });

		return string;
	}

	std::string AddrToHex(uintptr_t value) {
		std::stringstream ss;
		ss << "0x" << std::hex << value;
		return ToUppercase(ss.str());
	}

	std::string S(std::string string)
	{
		return string;
	}

	void Log(std::string text)
	{
		if (!IsDebug) return;

		std::cout << text + newLine;
	}
}