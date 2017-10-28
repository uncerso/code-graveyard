#include "randomFactory.hpp"
#include "network.hpp"
#include <iostream>
using namespace std;

class CastomRandomFactory : public RandomFactory {
	unsigned int number;
public:
	CastomRandomFactory(unsigned int number)
		: number(number)
	{}

	~CastomRandomFactory() = default;

	unsigned int operator()() override {
		return number;
	}

};

int main() {

	return 0;
}
