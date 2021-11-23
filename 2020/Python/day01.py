from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase


class Puzzle(PuzzleBase):
    _DAY = 1

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


    def run_part1(self, input: List[int], correctResult: int) -> str:
        result = self.get_value(1, input, [])
        return helper.validate_result2('Product of the two entries?', result, correctResult)


    def run_part2(self, input: List[int], correctResult: int) -> str:
        result = self.get_value(2, input, [])
        return helper.validate_result2('Product of the three entries?', result, correctResult)


    def load_input(self, name: str) -> List[int]:
        lines = inputHelper.load_input_file(self._DAY, name)
        return [int(l) for l in lines]

    def load_answer(self, part: int, name: str) -> int:
        lines = inputHelper.load_answer_file(self._DAY, part, name)
        return int(lines[0]) if lines is not None else None
        

    def part1_ex1(self) -> str:
        return "P1 Ex1) " + self.run_part1(
            self.load_input('example1'),
            self.load_answer(1, 'example1'))
    def part1(self) -> str:
        return "Part 1) " + self.run_part1(
            self.load_input('input'),
            self.load_answer(1, 'input'))

    def part2_ex1(self) -> str:
        return "P2 Ex1) " + self.run_part2(
            self.load_input('example1'),
            self.load_answer(2, 'example1'))
    def part2(self) -> str:
        return "Part 2) " + self.run_part2(
            self.load_input('input'),
            self.load_answer(2, 'input'))


    def solve(self):
        print("Day 1: Report Repair")
        print("")
        
        self.run_example(self.part1_ex1)
        self.run(self.part1)

        print("")

        self.run_example(self.part2_ex1)
        self.run(self.part2)