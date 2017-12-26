#include <iostream>
#include <vector>
#include <algorithm>
#include "BST.h"
#include "gtest/gtest.h"
using namespace std;

void testIterator(const vector<int> &x) {
	BST<int> bst;
	for (auto t : x) bst.insert(t);
	int t = 0;

	for (auto it : bst) {
		ASSERT_EQ(t, it);
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
			ASSERT_EQ(i, *it);
		else
			ASSERT_TRUE(i < 0 || i >= x.size());
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
			ASSERT_EQ(t, p);
			++t;
		}
	}
}

void check(int n, void (*foo)(const vector<int>&), const char *text) {
	vector<int> x(n);
	for (int i = 0; i < n; ++i) x[i] = i;
	foo(x);
	while (next_permutation(x.begin(), x.end())) foo(x);
}

const int n = 10;

TEST(testIterator, allTree) {
	check(n,     testIterator, "testIterator");
}

TEST(testFint, allTree) {
	check(n,     testFind,     "testFind");
}

TEST(testErase, allTree) {
	check(n - 1, testErase,    "testErase");
}

int main(int argc, char *argv[]) {
	::testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
//	return 0;
}
