#include "computers.hpp"

namespace net {

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

void Computer::setVirus() noexcept {
	containsViruse = true;
}

}
