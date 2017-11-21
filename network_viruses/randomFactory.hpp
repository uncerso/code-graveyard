#pragma once
#include <random>

///Random geenerator
class RandomFactory {
	std::mt19937 gen_;
public:
	/// Constructor
	RandomFactory();

	/// Destructor
	virtual ~RandomFactory() = default;

	/// Get random number
	virtual unsigned int operator()();
};
