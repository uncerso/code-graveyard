#include "robots.hpp"
#include <cassert>
const int notAColor(2);
using std::vector;

namespace robots {

void Robots::addEdge(uint first, uint second) noexcept {
	assert(first < graph.size());
	assert(second < graph.size());
	graph[first].push_back(second);
	graph[second].push_back(first);
}

void Robots::setRobot(uint pos) noexcept {
	assert(pos < withRobot.size());
	withRobot[pos] = true;
	++amountOfRobots;
}

void Robots::addVertices(uint cnt) noexcept {
	graph.resize(graph.size() + cnt);
	withRobot.resize(withRobot.size() + cnt);
}

bool Robots::dfs(uint pos, bool color, vector<uint> & colors) noexcept {
	colors[pos] = color;
	for (auto const &v : graph[pos]) {
		if (colors[v] == color)
			return true;          //It is an odd loop
		if (colors[v] == notAColor)
			if (dfs(v, !color, colors))
				return true;  //Odd loop has been found
	}
	return false; //Odd loop hasn't been found
}

bool Robots::solve() noexcept {
	vector<uint> colors(graph.size(), notAColor);
	if (dfs(0, false, colors) && !(amountOfRobots & 1)) return true;  //Odd loop with even amount of robots
	int cntColor1 = 0;
	int cntColor2 = 0;
	for (int i = 0, iend = withRobot.size(); i < iend; ++i)
		if (withRobot[i]) {
			if (colors[i])
				++cntColor1;
			else
				++cntColor2;
		}
	return !(1 & (cntColor1 | cntColor2)); //ItObvious
}

}
