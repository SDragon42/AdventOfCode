from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 2
        self.input = inputHelper.load_file(day, name).splitlines()
        
        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class Puzzle(PuzzleBase):

    def check_password(self, entry: str) -> int:
        entryParts = entry.split()

        range = entryParts[0].split('-')
        min = int(range[0])
        max = int(range[1])

        letter = entryParts[1].split(":")[0]

        password = entryParts[2]

        count = 0
        for x in password:
            if (x == letter):
                count += 1

        if (min <= count <= max):
            return 1
        return 0


    def check_password2(self, entry: str) -> int:
        entryParts = entry.split()
        
        positions = entryParts[0].split('-')
        letter = entryParts[1].split(":")[0]
        password = entryParts[2]

        matches = 0
        for x in positions:
            check = password[int(x) - 1]
            if check == letter:
                matches += 1
        
        if matches == 1:
            return 1
        return 0


    def run_part1(self, data: InputData) -> str:
        result = 0
        for entry in data.input:
            result += self.check_password(entry)
        return helper.validate_result("How many passwords are valid?", result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        result = 0
        for entry in data.input:
            result += self.check_password2(entry)
        return helper.validate_result("How many passwords are valid?", result, data.expectedAnswer)


    def solve(self):
        print("Day 2: Password Philosophy")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))