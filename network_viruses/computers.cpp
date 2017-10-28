#include "computers.hpp"

Computer::Computer(unsigned int chance)
	: chance(chance)
	, containsViruse(false)
{}

void Computer::probe(unsigned int chance) noexcept {
	containsViruse |= chance < this->chance;
}

bool Computer::isInfected() const noexcept {
	return containsViruse;
}

void Computer::setViruse() noexcept {
	containsViruse = true;
}
