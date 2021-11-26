from typing import Callable
import time

class PuzzleBase:
    _benchmarks = False
    _examples = False

    def __init__(self) -> None:
        pass

    def set_options(self, runBenchmarks: bool, runExamples: bool):
        self._benchmarks = runBenchmarks
        self._examples = runExamples

    
    def run_problem(self, puzzleFunction: Callable[[],str]) -> None:
        if self._benchmarks:
            print(self.__run_with_benchmarks(puzzleFunction))
        else:
            print(self.__run_without_benchmarks(puzzleFunction))


    def run_example(self, puzzleFunction: Callable[[],str]) -> None:
        if self._examples:
            self.run_problem(puzzleFunction)
        

    
    def __run_with_benchmarks(self, puzzleFunction: Callable[[],str]) -> str:
        start = time.perf_counter_ns()
        text = puzzleFunction()
        end = time.perf_counter_ns()
        elapsed = (end - start) / 1000000
        return f'{text}   {elapsed} ms'


    def __run_without_benchmarks(self, puzzleFunction: Callable[[],str]) -> str:
        text = puzzleFunction()
        return text


    def solve(self) -> None:
        pass