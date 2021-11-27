from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[int]
    windowSize: int
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 9
        self.input = [int(l) for l in inputHelper.load_input_file(day, name)]

        self.windowSize = int(inputHelper.load_input_file(day, name+'-windowsize')[0])
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



class Puzzle(PuzzleBase):

    def is_valid(self, value: int, input: List[int]) -> bool:
        if len(input) == 0:
            return False

        first = input[0]
        rest = input[1:]
        for x in rest:
            if first + x == value:
                return True

        return self.is_valid(value, rest)


    def scan_input(self, input: List[int], windowSize: int) -> int:
        lowIdx = 0
        highIdx = lowIdx + windowSize
        i = windowSize

        while i < len(input):
            if not self.is_valid(input[i], input[lowIdx:highIdx]):
                return input[i]

            lowIdx += 1
            highIdx += 1
            i += 1
        return - 1


    def find_weakness(self, input: List[int], invalidNum: int) -> int:
        start = 0
        end = 1

        while end < len(input):
            testInput = input[start:end]
            total = sum(testInput)
            if total == invalidNum:
                a = min(testInput)
                b = max(testInput)
                return a + b

            if total < invalidNum:
                end += 1
                continue

            if total > invalidNum:
                start += 1
                continue
        return -1


    def run_part1(self, data: InputData) -> str:
        result = self.scan_input(data.input, data.windowSize)
        return helper.validate_result2('The first number without this property:', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        invalidNum = self.scan_input(data.input, data.windowSize)
        result = self.find_weakness(data.input, invalidNum)
        return helper.validate_result2('The encryption weakness in your XMAS-encrypted list of numbers:', result, data.expectedAnswer)


    def solve(self):
        print("Day 9: Encoding Error")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))