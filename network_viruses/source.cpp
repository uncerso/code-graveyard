#include "randomFactory.hpp"
#include "network.hpp"
#include <gtest/gtest.h>
#include <iostream>
using namespace std;
using namespace net;

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
	CustomRandomFactory customRandomFactory(1);
	Network<CustomRandomFactory> net(CustomRandomFactory(5));
	for (int i = 0; i < n; ++i)
		net.addComputer(Computer(customRandomFactory()));
	for (int i = 1; i < n; ++i)
		net.addEdge(0, i);
	net.addVirus(0);
	nothingBesideZero(net);
	for (int i = 0; i < 10000; ++i) {
		net.step();
		nothingBesideZero(net);
	}
}

TEST(CustomRandomFactory, strong_viruse_one_step) {
	const int n = 100000;
	CustomRandomFactory customRandomFactory(10);
	Network<CustomRandomFactory> net(CustomRandomFactory(5));
	for (int i = 0; i < n; ++i)
		net.addComputer(Computer(customRandomFactory()));
	for (int i = 1; i < n; ++i)
		net.addEdge(0, i);
	net.addVirus(0);
	nothingBesideZero(net);
	net.step();
	for (auto const & x : net.getState())
		ASSERT_TRUE(x);
}

TEST(CustomRandomFactory, strong_viruse_tree) {
	const int n = (1 << 16) - 1;
	CustomRandomFactory customRandomFactory(10);
	Network<CustomRandomFactory> net(CustomRandomFactory(5));
	for (int i = 0; i < n; ++i)
		net.addComputer(Computer(customRandomFactory()));
	for (int i = 0; ((i + 1) << 1) < n; ++i) { // (i + 1) << 1 equal i*2 + 2
		net.addEdge(i, (i << 1) + 1);
		net.addEdge(i, (i << 1) + 2);
	}
	net.addVirus(0);
	size_t infestedTo = 1;
	nothingBesideZero(net);
	for (size_t i = 0; i < 63; ++i) {
		net.step();
		infestedTo <<= 1;
		++infestedTo;
		auto const & x(net.getState());
		for (size_t j = 0; j < n; ++j)
			ASSERT_EQ(x[j], j < infestedTo);
	}
}

TEST(RandomFactory, random_viruse_tree) {
	const int n = (1 << 16) - 1;
	RandomFactory randomFactory;
	Network<RandomFactory> net((RandomFactory()));
	for (int i = 0; i < n; ++i)
		net.addComputer(Computer(randomFactory()));
	for (int i = 0; ((i + 1) << 1) < n; ++i) { // (i + 1) << 1 equal i*2 + 2
		net.addEdge(i, (i << 1) + 1);
		net.addEdge(i, (i << 1) + 2);
	}
	net.addVirus(0);
	nothingBesideZero(net);
	size_t infestedTo = 1;
	auto last(net.getState());
	for (size_t i = 0; i < 100; ++i) {
		net.step();
		if (i < 63) {
			infestedTo <<= 1;
			++infestedTo;
		}

		auto now(net.getState());
		ASSERT_TRUE(now[0]);
		for (size_t j = 1; j < n; ++j) {
			if (now[j]) ASSERT_TRUE(now[(j - 1) >> 1]); // parent infested
			if ((i < 63) && (j >= infestedTo)) ASSERT_FALSE(now[j]);
		}
		swap(last, now);
	}
}

int main(int argc, char *argv[]) {
	::testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
	//return 0;
}
