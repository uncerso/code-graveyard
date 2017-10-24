#include <iostream>
#include <fstream>
#include <string>
#include <cassert>
#include "robots.hpp"
using namespace std;

Robots loadToSample(string nameOfFile) noexcept {
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

int loadAns(string nameOfFile) noexcept {
	ifstream inp("tests/" + nameOfFile + ".out");
	assert(inp.is_open());
	int ans;
	inp >> ans;
	inp.close();
	return ans;
}

int main() {
	// auto robots = loadToSample("simpleTest1");
	// auto ans = loadAns("simpleTest1");
	// auto robots = loadToSample("oddLoopTest1");
	// auto ans = loadAns("oddLoopTest1");
	auto robots = loadToSample("evenLoopTest1");
	auto ans = loadAns("evenLoopTest1");
	cout << (robots.solve() == ans);
	return 0;
}
