#pragma once

#include "computers.hpp"
#include <vector>
#include <list>
#include <cassert>

namespace net {

template <class T>
class Network {
	using uint = unsigned int;
	T gen;
	std::vector<std::list<uint> > graph;
	std::vector<Computer> computers;
	std::list<uint> setOfInfected;
public:
	// Constructor
	// gen - random generator
	Network(T const &gen);

	// Destructor
	~Network() = default;

	// Add an edge between the 'first' and 'second' computer
	void addEdge(uint first, uint second);

	// Next iteration of network emulation
	void step();

	// Add a virus to the computer with number of 'pos'
	void addVirus(uint pos);

	// Add a computer to the network
	// The smallest free number is assigned to it
	void addComputer(Computer const &comp);

	// Returns a vector which indicates whether each computer has a virus
	std::vector<bool> getState() const;
};

template <class T>
Network<T>::Network(T const &gen)
	: gen(gen)
{}

template <class T>
void Network<T>::addEdge(uint first, uint second) {
	assert(first < graph.size());
	assert(second < graph.size());
	graph[first].push_back(second);
	graph[second].push_back(first);
}

template <class T>
void Network<T>::addVirus(uint pos) {
	assert(pos < computers.size());
	computers[pos].setVirus();
	setOfInfected.push_back(pos);
}

template <class T>
void Network<T>::addComputer(Computer const &comp) {
	computers.push_back(comp);
	graph.push_back(std::list<uint>());
}

template <class T>
std::vector<bool> Network<T>::getState() const {
	std::vector<bool> ans(computers.size());
	for (int i = 0, iend = computers.size(); i < iend; ++i)
		ans[i] = computers[i].isInfected();
	return ans; //NRVO optimization
}

template <class T>
void Network<T>::step() {
	std::list<uint> tmp;
	for (auto x : setOfInfected) {
		for (auto &v : graph[x])
			if (!computers[v].isInfected()) {
				computers[v].probe(gen());
				if (computers[v].isInfected())
					tmp.push_back(v);
			}
	}
	for (auto &x : tmp)
		setOfInfected.push_back(x);
}

}
