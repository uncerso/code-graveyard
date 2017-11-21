#pragma once
#include <vector>

namespace robots {

/// Solves the problem of destroying robots
class Robots {
	using uint = unsigned int;
	uint amountOfRobots = 0;
	std::vector<bool> withRobot;
	std::vector<std::vector<uint> > graph;
	bool dfs(uint, bool, std::vector<uint> ) noexcept;

public:

	Robots() = default;
	~Robots() = default;

	/// Add a vertice to the graph
	/// The smallest free number is assigned to it
	void addVertices(uint) noexcept;

	/// Set a robot to the vertice
	void setRobot(uint) noexcept;

	/// Add an edge between two vertices
	void addEdge(uint, uint) noexcept;

	/// Checks whether robots can destroy each other
	bool solve() noexcept;
};

}
