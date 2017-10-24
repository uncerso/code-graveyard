#include <iostream>
#include <vector>
#include <algorithm>
#include "BST.h"
//#include "gtest/gtest.h"
using namespace std;

void dumpIf(bool b, const vector<int> &x, BST<int> &bst, const char *text1,
            int arg1, const char *text2,
            int arg2 = 0, bool printArg2 = false) {
	if (b) return;

	cout << '\n' << text1 << arg1 << text2;
	if (printArg2) cout << arg2;

	cout << "\ninput:  ";
	for (auto t : x) cout << t << ' ';
	cout << "\noutput: ";
	for (auto t : bst) cout << t << ' ';
	cout << "\ntree:\n";
	bst.print();
	throw 1;
}

void testIterator(const vector<int> &x) {
	BST<int> bst;
	for (auto t : x) bst.insert(t);
	int t = 0;

	for (auto it : bst) {
		dumpIf(t == it, x, bst, "Expected: ", t, ", but received: ", it, true);
		++t;
	}
}

void testFind(const vector<int> &x) {
	BST<int> bst;
	for (auto t : x) bst.insert(t);
	BST<int>::iterator it;
	for (int i = -1; i <= (int)x.size(); ++i) {
		it = bst.find(i);
		if (it != bst.end())
			dumpIf(i == *it, x, bst, "Requested: ", i, ", but received: ", *it, true);
		else dumpIf(i < 0 || i >= x.size(), x, bst, "Requested: ", i, ", but received \"not found\"");
	}
}

void testErase(const vector<int> &x) {
	for (int k = -1; k <= (int)x.size(); ++k) {
		BST<int> bst;
		for (auto t : x) bst.insert(t);
		bst.erase(k);
		int t = 0;
		for (auto p : bst) {
			if (t == k) ++t;
			dumpIf(t == p, x, bst, "Requested for erase: ", k, "");
			++t;
		}
	}
}

void check(int n, void (*foo)(const vector<int>&), const char *text) {
	cout << text;
	cout.flush();
	vector<int> x(n);
	for (int i = 0; i < n; ++i) x[i] = i;
	foo(x);
	while (next_permutation(x.begin(), x.end())) foo(x);
	cout << "\t[ ok ]\n";
}

int main(int argc, char *argv[]) {
	const int n = 10;

	check(n,     testIterator, "testIterator");
	check(n,     testFind,     "testFind");
	check(n - 1, testErase,    "testErase");
	return 0;
}
