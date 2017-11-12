FLAGS := --std=c++14 -lgtest -pthread
NAME := run
.PHONY := clean


release : *.cpp
	g++ -O3 $(FLAGS) $^ -o $(NAME)
debug : *.cpp
	g++ -g $(FLAGS) $^ -o $(NAME)
	gdb $(NAME)
coverage : *.cpp
	g++ -g -fprofile-arcs -ftest-coverage -lgcov $(FLAGS) $^ -o $(NAME)
	make clean
	./run
	mkdir coverage_results
	lcov --directory . --capture -o coverage_results/lcov.info
	mv coverage_results/lcov.info coverage_results/lcov_orig.info
	lcov --remove coverage_results/lcov_orig.info "/usr/*" -o coverage_results/lcov.info
	lcov --directory . --zerocounters
	rm -rf *.gcno
	genhtml -o coverage_results/ coverage_results/lcov.info --highlight --legend --branch-coverage --title "robots"
	echo "You can find coverage results in coverage_results/index.html"
#	google-chrome coverage_results/index.html
clang : *.cpp
	clang++-4.0 -O3 --std=c++1z $^ -o $(NAME)
clean :
	rm -rf coverage_results/
	lcov --directory . --zerocounters
