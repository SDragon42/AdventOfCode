from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 5
        self.input = inputHelper.load_input_file(day, name)
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



class Puzzle(PuzzleBase):

    def get_row(self, lowCode: str, highCode: str, lowStart: int, highStart: int, input: str) -> int:
        low = lowStart
        high = highStart
        for x in input:
            byHalf = (high - low + 1) / 2
            if x == highCode:
                high -= byHalf
            elif x == lowCode:
                low += byHalf
        return int(low - 1)


    def get_seat_id(self, boardingPass: str) -> int:
        rowCode = boardingPass[:7]
        colCode = boardingPass[7:]

        row = self.get_row("B", "F", 1, 128, rowCode)
        col = self.get_row("R", "L", 1, 8, colCode)
        seatId = (row * 8) + col
        
        return seatId


    def run_part1(self, data: InputData) -> str:
        result = 0
        for bPass in data.input:
            seatId = self.get_seat_id(bPass)
            if seatId > result:
                result = seatId
        return helper.validate_result2('What is the highest seat ID on a boarding pass?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        seats = []
        for x in range(1024):
            seats.append(x)
        
        for bPass in data.input:
            seatId = self.get_seat_id(bPass)
            seats[seatId] = 0

        result = -1

        for x in seats[1:-1]:
            if x != 0 and seats[x - 1] == 0 and seats[x + 1] == 0:
                result = x
                break

        return helper.validate_result2('What is the ID of your seat?', result, data.expectedAnswer)


    def solve(self):
        print("Day 5: Binary Boarding")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))