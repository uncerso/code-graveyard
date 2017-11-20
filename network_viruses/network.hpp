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
	Network(T const &gen);
	~Network() = default;
	void addEdge(uint first, uint second);
	void step();
	void addVirus(uint pos);
	void addComputer(Computer const &comp);
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
