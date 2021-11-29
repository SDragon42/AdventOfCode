from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[int]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 25

        lines = inputHelper.load_file(day, name).splitlines()
        self.input = [int(l) for l in lines]
        self.expectedAnswer = int(inputHelper.load_file(day, f"{name}-answer{part}"))



class Puzzle(PuzzleBase):

    def loop_transform(self, value: int, subjectNumber: int) -> int:
        value *= subjectNumber
        value = value % 20201227
        return value


    def find_loop_size(self, publicKey: int) -> int:
        loopSize = 0
        calcKey = 1
        while True:
            loopSize += 1
            calcKey = self.loop_transform(calcKey, 7)
            if calcKey == publicKey:
                break
        return loopSize


    def calculate_encryption_key(self, publicKey: int, loopSize: int) -> int:
        l = 0
        encryptKey = 1
        while l < loopSize:
            encryptKey = self.loop_transform(encryptKey, publicKey)
            l += 1
        return encryptKey


    def run_part1(self, data: InputData) -> str:
        cardPublicKey = data.input[0]
        doorPublicKey = data.input[1]

        cardLoopSize = self.find_loop_size(cardPublicKey)
        doorLoopSize = self.find_loop_size(doorPublicKey)
        helper.dprint(f"card key: {cardPublicKey}   loop size: {cardLoopSize}")
        helper.dprint(f"door key: {doorPublicKey}   loop size: {doorLoopSize}")

        result1 = self.calculate_encryption_key(cardPublicKey, doorLoopSize)
        result2 = self.calculate_encryption_key(doorPublicKey, cardLoopSize)

        if result1 == result2:
            helper.dprint("    KEY MATCH")
        helper.dprint(f"key: {result1} - {result2}")

        return helper.validate_result('What encryption key is the handshake trying to establish?', result1, data.expectedAnswer)


    def solve(self):
        print("Day 25: Combo Breaker")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))