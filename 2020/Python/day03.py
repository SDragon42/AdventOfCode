from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str] = []
    slopes: List[str] = []
    expectedAnswer: int = None

    def __init__(self, name: str, part: int) -> None:
        day = 3
        self.input = inputHelper.load_input_file(day, name)

        lines = inputHelper.load_input_file(day, f'slopes{part}')
        self.slopes = lines[0].split(',')
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



class Puzzle(PuzzleBase):

    def count_trees(self, input: List[str], slopeX: int, slopeY: int) -> int:
        x = 0
        y = 0
        numTrees = 0
        inputWidth = len(input[0])

        while y < len(input) - 1:
            x += slopeX
            y += slopeY

            if x >= inputWidth:
                x -= inputWidth

            val = input[y][x]
            if val == '#':
                numTrees += 1

        return numTrees


    def run_part(self, data: InputData) -> str:
        result = 1
        for sl in data.slopes:
            result *= self.count_trees(data.input, int(sl[0]), int(sl[1]))
        return helper.validate_result2("how many trees would you encounter?", result, data.expectedAnswer)


    def solve(self):
        print("Day 3: Toboggan Trajectory")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part(InputData('input', 2)))