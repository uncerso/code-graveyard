#include "randomFactory.hpp"
#include "network.hpp"
#include <gtest/gtest.h>
using namespace std;

class CustomRandomFactory : public RandomFactory {
	unsigned int number;
public:
	CustomRandomFactory(unsigned int number)
		: number(number)
	{}

	~CustomRandomFactory() = default;

	unsigned int operator()() override {
		return number;
	}

};

template <class T>
void nothingBesideZero(Network<T> const &net) {
	bool b = true;
	for (auto const &x : net.getState()) {
		ASSERT_EQ(b, x);
		b = false;
	}
}

TEST(CustomRandomFactory, weak_viruse) {
	const int n = 10;
	CustomRandomFactory rd(1);
	Network<CustomRandomFactory> net(CustomRandomFactory(5));
	for (int i = 0; i < n; ++i)
		net.addComputer(Computer(rd()));
	for (int i = 1; i < n; ++i)
		net.addEdge(0, i);
	net.addViruse(0);
	nothingBesideZero(net);
	for (int i = 0; i < 10000; ++i) {
		net.step();
		nothingBesideZero(net);
	}
}

// TEST(RandomFactory, b) {
//      Network<RandomFactory> net(RandomFactory());
// }

int main(int argc, char *argv[]) {
	::testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
	//return 0;
}
