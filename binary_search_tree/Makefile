FLAGS := --std=c++14
NAME := run

release : *.cpp
	g++ -O3 $(FLAGS) $^ -o $(NAME)
debug : *.cpp
	g++ -g $(FLAGS) $^ -o $(NAME)
	gdb $(NAME)
clang : *.cpp
	clang++-4.0 -O3 --std=c++1z $^ -o $(NAME)
