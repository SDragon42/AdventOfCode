from typing import List, Dict

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    lastTurn: int
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 15
        self.input = inputHelper.load_file(day, name).splitlines()

        self.lastTurn = int(inputHelper.load_file(day, name+f'-lastturn{part}'))
        
        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class LastSaid:
    value: str
    onTurn: int
    prevTurn: int

    def __init__(self) -> None:
        self.value = ''
        self.onTurn = 0
        self.prevTurn = 0



class Puzzle(PuzzleBase):

    def run_part(self, data: InputData) -> str:
        history: Dict[str, int] = {}
        turn = 0
        ls = LastSaid()
        initialSeq = data.input[0].split(",")
        for x in initialSeq:
            turn += 1
            history[x] = turn
            ls.value = x
            ls.onTurn = turn
        
        while turn < data.lastTurn:
            turn += 1
            
            if ls.prevTurn == 0:
                ls.value = "0"
                ls.onTurn = turn
            else:
                diff = ls.onTurn - ls.prevTurn
                ls.value = str(diff)
                ls.onTurn = turn

            if ls.value in history:
                ls.prevTurn = history[ls.value]
            else:
                ls.prevTurn = 0

            history[ls.value] = turn

        result = int(ls.value)
        return helper.validate_result(f'what will be the {data.lastTurn}th number spoken?', result, data.expectedAnswer)


    def solve(self):
        print("Day 15: Rambunctious Recitation")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part(InputData('input', 2)))