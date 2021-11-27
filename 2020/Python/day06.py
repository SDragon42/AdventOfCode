from typing import List, Callable

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 6
        self.input = inputHelper.load_input_file(day, name)
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



class Puzzle(PuzzleBase):

    def count_unique_answers(self, lines: List[str]) -> int:
        answers = []

        for line in lines:
            for c in line:
                if answers.count(c) == 0:
                    answers.append(c)
        
        return len(answers)


    def count_unique_unanimous_answers(self, lines: List[str]) -> int:
        answers = {}
        pCount = 0

        for line in lines:
            pCount += 1
            for c in line:
                if c in answers:
                    answers[c] = answers[c] + 1
                else:
                    answers[c] = 1

        result = 0
        for a in answers:
            if answers[a] == pCount:
                result += 1
        
        return result


    def run_part(self, data: InputData, process: Callable[[List[str]], int]) -> str:
        result = 0
        groupStart = 0
        groupEnd = 0
        while groupStart < len(data.input):
            try:
                groupEnd = data.input.index("", groupStart)
            except:
                groupEnd = len(data.input)

            lines = data.input[groupStart:groupEnd]
            result += process(lines)

            groupStart = groupEnd + 1

        return helper.validate_result('What is the sum of those counts?', result, data.expectedAnswer)


    def solve(self):
        print("Day 6: Custom Customs")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part(InputData('example1', 1), self.count_unique_answers))
        self.run_problem(lambda: "Part 1) " + self.run_part(InputData('input', 1), self.count_unique_answers))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part(InputData('example1', 2), self.count_unique_unanimous_answers))
        self.run_problem(lambda: "Part 2) " + self.run_part(InputData('input', 2), self.count_unique_unanimous_answers))