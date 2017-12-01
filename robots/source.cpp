//#include <iostream>
#include <fstream>
#include <string>
#include <cassert>
#include "robots.hpp"
#include <gtest/gtest.h>
using namespace std;
using namespace robots;

Robots loadToSample(string const & nameOfFile) noexcept {
	Robots robots;
	ifstream inp("tests/" + nameOfFile + ".in");
	assert(inp.is_open());
	unsigned int amountOfVertex, amountOfEdges, amountOfRobots;
	inp >> amountOfVertex >> amountOfEdges >> amountOfRobots;
	int a, b;
	robots.addVertices(amountOfVertex);
	for (int i = 0; i < amountOfEdges; ++i) {
		inp >> a >> b;
		robots.addEdge(a - 1, b - 1);
	}
	for (int i = 0; i < amountOfRobots; ++i) {
		inp >> a;
		robots.setRobot(a - 1);
	}
	inp.close();
	return robots;
}

int loadAns(string const & nameOfFile) noexcept {
	ifstream inp("tests/" + nameOfFile + ".out");
	assert(inp.is_open());
	int ans;
	inp >> ans;
	inp.close();
	return ans;
}

TEST(simpleTest, simpleTest1) {
	auto robots = loadToSample("simpleTest1");
	auto ans = loadAns("simpleTest1");
	ASSERT_EQ(robots.solve(), ans);
}

TEST(simpleTest, simpleTest2) {
	auto robots = loadToSample("simpleTest2");
	auto ans = loadAns("simpleTest2");
	ASSERT_EQ(robots.solve(), ans);
}

TEST(oddLoopTest, oddLoopTest1) {
	auto robots = loadToSample("oddLoopTest1");
	auto ans = loadAns("oddLoopTest1");
	ASSERT_EQ(robots.solve(), ans);
}

TEST(oddLoopTest, oddLoopTest2) {
	auto robots = loadToSample("oddLoopTest2");
	auto ans = loadAns("oddLoopTest2");
	ASSERT_EQ(robots.solve(), ans);
}

TEST(evenLoopTest, evenLoopTest1) {
	auto robots = loadToSample("evenLoopTest1");
	auto ans = loadAns("evenLoopTest1");
	ASSERT_EQ(robots.solve(), ans);
}

TEST(evenLoopTest, evenLoopTest2) {
	auto robots = loadToSample("evenLoopTest2");
	auto ans = loadAns("evenLoopTest2");
	ASSERT_EQ(robots.solve(), ans);
}

int main(int argc, char *argv[]) {
	::testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
}
