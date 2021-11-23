from typing import AnyStr, Callable, Type, List
import time

class PuzzleBase:
    runBenchmarks = False
    runExamples = False

    def __init__(self) -> None:
        pass

    def set_options(self, runBenchmarks: bool, runExamples: bool):
        self.runBenchmarks = runBenchmarks
        self.runExamples = runExamples

    
    def run(self, puzzleFunction: Callable[[],str]) -> None:
        if self.runBenchmarks:
            print(self.__run_with_benchmarks(puzzleFunction))
        else:
            print(self.__run_without_benchmarks(puzzleFunction))


    def run_example(self, puzzleFunction: Callable[[],str]) -> None:
        if self.runExamples:
            self.run(puzzleFunction)
        

    
    def __run_with_benchmarks(self, puzzleFunction: Callable[[],str]) -> str:
        start = time.perf_counter_ns()
        text = puzzleFunction()
        end = time.perf_counter_ns()
        elapsed = (end - start) / 1000000
        return f'{text}   {elapsed} ms'


    def __run_without_benchmarks(self, puzzleFunction: Callable[[],str]) -> str:
        return puzzleFunction()


    def solve(self) -> None:
        pass