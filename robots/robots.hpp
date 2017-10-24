#pragma once
#include <vector>

class Robots {
	using uint = unsigned int;
	uint amountOfRobots = 0;
	std::vector<bool> withRobot;
	std::vector<std::vector<uint> > graph;
	bool dfs(uint, bool, std::vector<uint> ) noexcept;

public:

	Robots() = default;
	~Robots() = default;
	void addVertices(uint) noexcept;
	void setRobot(uint) noexcept;
	void addEdge(uint, uint) noexcept;
	bool solve() noexcept;
};
