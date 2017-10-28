#pragma once

#include "computers.hpp"
#include <vector>
#include <list>
#include <cassert>

template <class T>
class Network {
	using uint = unsigned int;
	T gen;
	std::vector<std::list<int> > graph;
	std::vector<Computer> computers;
	std::list<uint> setOfIefested;
public:
	Network(T const &gen);
	~Network() = default;
	void addEdge(uint first, uint second);
	void step();
	void addViruse(uint pos);
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
void Network<T>::addViruse(uint pos) {
	assert(pos < computers.size());
	computers[pos].setViruse();
	setOfIefested.push_back(pos);
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
	std::list<int> tmp;
	for (auto x : setOfIefested) {
		for (auto &v : graph[x])
			if (!v.isInfected()) {
				v.probe(gen());
				if (v.isInfected())
					tmp.push_back(v);
			}
	}
	setOfIefested.push_back(tmp);
}
