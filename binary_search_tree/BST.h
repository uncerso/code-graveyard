#pragma once
#include <iterator>
#include <cassert>
#include <iostream>

template <class T>
class BST {
	struct vert {
		T    *_value;
		vert *_previous;
		vert *_next;
		vert *_left;
		vert *_right;
		vert();
		vert(const T &value,
		     vert    *prev,
		     vert    *next);
		~vert();
		vert* find(const T &value);
		void insert(const T &value);
		vert* erase(const T &value,
		            bool needRemoveValue = true);

		void print();
	};

	vert *_root;
	vert *_begin;
	vert *_end;

public:
	class iterator : public std::iterator<std::forward_iterator_tag, T> {
		friend BST;
		vert *_now;
		iterator(vert *now);

public:
		iterator();
		iterator(const iterator &it);
		bool operator!=(const iterator &it) const;
		bool operator==(const iterator &it) const;
		iterator& operator++();
		iterator operator++(int);
		T&        operator*() const;
	};

	BST();
	~BST();
	iterator find(const T &value);
	void insert(const T &value);
	void erase(const T &value);

	void print();

	iterator begin();
	iterator end();
};

// ------------------------------<iterator>------------------------------

template <class T>
BST<T>::iterator::iterator(vert *now)
	: _now(now)
{}

template <class T>
BST<T>::iterator::iterator()
	: _now(nullptr)
{}

template <class T>
BST<T>::iterator::iterator(const iterator &it)
	: _now(it._now)
{}

template <class T>
bool BST<T>::iterator::operator!=(const iterator &it) const {
	return _now != it._now;
}

template <class T>
bool BST<T>::iterator::operator==(const iterator &it) const {
	return _now == it._now;
}

template <class T>
typename BST<T>::iterator&BST<T>::iterator::operator++() {
	assert(_now && _now->_next);
	_now = _now->_next;
	return *this;
}

template <class T>
typename BST<T>::iterator BST<T>::iterator::operator++(int) {
	assert(_now && _now->_next);
	iterator tmp(this->_now);
	_now = _now->_next;
	return tmp;
}

template <class T>
T&BST<T>::iterator::operator*() const {
	assert(_now && _now->_value);
	return *(_now->_value);
}

// ------------------------------</iterator>------------------------------
// ------------------------------<BST>------------------------------

template <class T>
typename BST<T>::iterator BST<T>::begin() {
	return iterator(_begin->_next);
}

template <class T>
typename BST<T>::iterator BST<T>::end() {
	return iterator(_end);
}

template <class T>
BST<T>::BST()
	: _root(nullptr)
	, _begin(new vert())
	, _end(new vert())
{
	_begin->_next   = _end;
	_end->_previous = _begin;
}

template <class T>
BST<T>::~BST() {
	if (_root) delete _root;
	delete _begin;
	delete _end;
}

template <class T>
void BST<T>::insert(const T &value) {
	if (!_root) {
		_root           = new vert(value, _begin, _end);
		_begin->_next   = _root;
		_end->_previous = _root;
		return;
	}
	_root->insert(value);
}

template <class T>
void BST<T>::erase(const T &value) {
	if (!_root) return;

	_root = _root->erase(value);
}

template <class T>
typename BST<T>::iterator BST<T>::find(const T &value) {
	if (!_root) return iterator(_root);

	vert *tmp = _root->find(value);
	if (tmp) return iterator(tmp);

	return end();
}

template <class T>
void BST<T>::print() {
	if (_root) _root->print();
	std::cout << '\n';
}

// ------------------------------</BST>------------------------------
// ------------------------------<vert>------------------------------
template <class T>
BST<T>::vert::vert()
	: _value(nullptr)
	, _previous(nullptr)
	, _next(nullptr)
	, _left(nullptr)
	, _right(nullptr)
{}

template <class T>
BST<T>::vert::vert(const T &value,
                   vert    *prev,
                   vert    *next)
	: _value(new T(value))
	, _previous(prev)
	, _next(next)
	, _left(nullptr)
	, _right(nullptr)
{}

template <class T>
BST<T>::vert::~vert() {
	if (_value) delete _value;
	if (_left) delete _left;
	if (_right) delete _right;
}

template <class T>
void BST<T>::vert::insert(const T &value) {
	if (value < *_value) {
		if (!_left) {
			_left            = new vert(value, _previous, this);
			_previous->_next = _left;
			_previous        = _left;
			return;
		}
		_left->insert(value);
		return;
	}
	if (!_right) {
		_right           = new vert(value, this, _next);
		_next->_previous = _right;
		_next            = _right;
		return;
	}
	_right->insert(value);
}

template <class T>
typename BST<T>::vert * BST<T>::vert::erase(const T &value,
                                            bool needRemoveValue)
{
	if (value < *_value) {
		if (_left) _left = _left->erase(value, needRemoveValue);
		return this;
	}
	if (*_value < value) {
		if (_right) _right = _right->erase(value, needRemoveValue);
		return this;
	}
	if (!needRemoveValue) _value = nullptr;
	if (!_left || !_right) {
		_previous->_next = _next;
		_next->_previous = _previous;
		vert *tmp = nullptr;
		if (!_left) {
			tmp    = _right;
			_right = nullptr;
		} else {
			tmp   = _left;
			_left = nullptr;
		}
		delete this;
		return tmp;
	}
	vert *temp = new vert();
	temp->_value     = _next->_value;
	temp->_previous  = _previous;
	temp->_next      = _next->_next;
	temp->_left      = _left;
	temp->_right     = _right->erase(*(_next->_value), false);
	_previous->_next = temp;
	_next->_previous = temp;
	_left            = _right = nullptr;
	delete this;
	return temp;
}

template <class T>
typename BST<T>::vert * BST<T>::vert::find(const T &value) {
	if (value < *_value) return (_left ? _left->find(value) : nullptr);

	if (*_value < value) return (_right ? _right->find(value) : nullptr);

	return this;
}

template <class T>
void BST<T>::vert::print() {
	if (_left) _left->print();
	std::cout << *_value << "\tthis: " << _value << "\tprev: " <<
	_previous << "\tnext: " << _next << '\n';
	if (_right) _right->print();
}

// ------------------------------</vert>------------------------------
