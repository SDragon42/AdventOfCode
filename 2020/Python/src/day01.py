from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[int]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 1
        lines = inputHelper.load_file(day, name).splitlines()
        self.input = [int(l) for l in lines]

        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



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


    def run_part1(self, data: InputData) -> str:
        result = self.get_value(1, data.input, [])
        return helper.validate_result('Product of the two entries?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        result = self.get_value(2, data.input, [])
        return helper.validate_result('Product of the three entries?', result, data.expectedAnswer)


    def solve(self):
        print("Day 1: Report Repair")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))
