#include "randomFactory.hpp"

RandomFactory::RandomFactory()
	: gen_((std::random_device())())
{}

unsigned int RandomFactory::operator()() {
	return gen_();
}
