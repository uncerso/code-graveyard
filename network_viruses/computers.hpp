#pragma once

class Computer {
	unsigned int chance;
	bool containsViruse;
public:
	Computer(unsigned int chance);
	~Computer() = default;
	void probe(unsigned int chance) noexcept;
	void setViruse() noexcept;
	bool isInfected() const noexcept;

};
