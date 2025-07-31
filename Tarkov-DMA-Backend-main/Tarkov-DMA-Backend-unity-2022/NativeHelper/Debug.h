#pragma once

namespace Debug
{
	#define LOG(message) Debug::Log(message)

	const std::string newLine = "\n";

	std::string ToUppercase(std::string string)
	{
		std::transform(string.begin(), string.end(), string.begin(), [](unsigned char c) { return std::toupper(c); });

		return string;
	}

	std::string AddrToHex(uintptr_t value) {
		std::stringstream ss;
		ss << xorstr_("0x") << std::hex << value;
		return ToUppercase(ss.str());
	}

	std::string S(std::string string)
	{
		return string;
	}

	void Log(std::string text)
	{
#ifndef COMMERCIAL
		std::cout << text + newLine;
#else
		std::cout << newLine;
#endif
	}
}