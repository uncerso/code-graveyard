#pragma once

namespace net {

class Computer {
	unsigned int chance;
	bool containsViruse;
public:
	explicit Computer(unsigned int chance);
	~Computer() = default;
	void probe(unsigned int chance) noexcept;
	void setVirus() noexcept;
	bool isInfected() const noexcept;
};

}
