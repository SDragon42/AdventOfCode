import sys
from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase


class Puzzle(PuzzleBase):

    def get_value(self, num_splits: int, testInput: List[int], testValues: List[int]) -> int:
        if num_splits == 0:
            for x in testInput:
                sumTotal = x
                multTotal = x
                for v in testValues:
                    sumTotal += v
                    multTotal *= v
                if sumTotal == 2020:
                    return multTotal
            return -1

        idx = 0
        while idx < len(testInput) - num_splits:
            testValues.append(testInput[idx])
            result = self.get_value(num_splits - 1, testInput[idx+1:], testValues)
            if result != -1:
                return result
            testValues.pop()
            idx += 1
        
        return -1


    def do_it(self, title: str, num_splits: int, input: List[int], correctResult: int) -> str:
        result = self.get_value(num_splits, input, [])
        return helper.validate_result2(title, result, correctResult)

    # def run_example(o: utils.PuzzleOptions, title: str, num_splits: int, input: List[int], correctResult: int) -> None:
    #     if (o.runExamples):
    #         run(title, num_splits, input, correctResult)

    def example1(self) -> str:
        return self.do_it(" Ex. 1)", 1,
            inputHelper.read_input_as_int_list(1,"example1"),
            514579)

    def part1(self) -> str:
        return self.do_it("Part 1)", 1,
            inputHelper.read_input_as_int_list(1, "input"),
            969024)

    def example2(self) -> str:
        return self.do_it(" Ex. 2)", 2,
            inputHelper.read_input_as_int_list(1, "example1"),
            241861950)
    def part2(self) -> str:
        return self.do_it("Part 2)", 2,
            inputHelper.read_input_as_int_list(1, "input"),
            230057040)

    # def solve(o: utils.PuzzleOptions):
    def solve(self):
        print("Day 1: Report Repair")
        print("")

        self.run_example(self.example1)
        self.run(self.part1)

        print("")

        # run("Test case 1", 2,
        #     inputHelper.read_input_as_int_list(1, "example1"),
        #     241861950)
        self.run_example(self.example2)
        self.run(self.part1)
