#pragma once
#include <random>

class RandomFactory {
	std::mt19937 gen_;
public:
	RandomFactory();
	virtual ~RandomFactory() = default;
	virtual unsigned int operator()();
};
