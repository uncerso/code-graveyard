#pragma once
#include <vector>

namespace robots {

/// Solves the problem of destroying robots
class Robots {
	using uint = unsigned int;
	uint amountOfRobots = 0;
	std::vector<bool> withRobot;
	std::vector<std::vector<uint> > graph;
	bool dfs(uint pos, bool color, vector<uint> const & colors) noexcept;

public:

	Robots() = default;
	~Robots() = default;

	/// Add a vertice to the graph
	/// The smallest free number is assigned to it
	void addVertices(uint cnt) noexcept;

	/// Set a robot to the vertice
	void setRobot(uint pos) noexcept;

	/// Add an edge between two vertices
	void addEdge(uint first, uint second) noexcept;

	/// Checks whether robots can destroy each other
	bool solve() noexcept;
};

}
