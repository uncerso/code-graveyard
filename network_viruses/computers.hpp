#pragma once

namespace net {

class Computer {
	unsigned int chance;
	bool containsViruse;
public:
	// Constructor
	// Parameter - chance of getting infected
	explicit Computer(unsigned int chance);

	// Destructor
	~Computer() = default;

	// Infects the computer if the parameter chance
	// is less than computer's chance of getting infected
	void probe(unsigned int chance) noexcept;

	// Infects the computer
	void setVirus() noexcept;

	// Returns true if the computer is isInfected
	// False otherwise
	bool isInfected() const noexcept;
};

}
