from typing import Callable, List, Dict

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 8
        self.input = inputHelper.load_file(day, name).splitlines()
        
        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class AccumulatorProcessor:

    idx: int
    accValue: int
    code: List[str]
    instructionDict: Dict[str, Callable]
    callStack: List[int]


    def __init__(self, code: List[str]):
        self.code = code
        self.idx = 0
        self.accValue = 0

        self.instructionDict = {
            "nop": self.no_operation,
            "acc": self.accumulator,
            "jmp": self.jump,
        }

        self.callStack = []


    def no_operation(self, value: int):
        self.idx += 1

    def accumulator(self, value: int):
        self.accValue += value
        self.idx += 1

    def jump(self, value: int):
        self.idx += value


    def run(self) -> bool:
        self.idx = 0
        self.accValue = 0

        self.callStack.clear()

        while self.idx < len(self.code):
            if self.idx in self.callStack:
                break
            self.callStack.append(self.idx)
            parts = self.code[self.idx].split(" ", 1)
            inst = parts[0]
            val = int(parts[1])
            action = self.instructionDict[inst]
            action(val)
        self.callStack.append(self.idx)

        finished = (self.idx >= len(self.code))
        return finished
        


class Puzzle(PuzzleBase):

    def flip_instruction(self, instructionIdx: int, input: List[str]) -> List[str]:
        if input[instructionIdx].startswith("nop"):
            input[instructionIdx] = input[instructionIdx].replace("nop", "jmp")
        else:
            input[instructionIdx] = input[instructionIdx].replace("jmp", "nop")
        return input


    def run_part1(self, data: InputData) -> str:
        comp = AccumulatorProcessor(data.input)
        comp.run()
        return helper.validate_result('The value in the accumulator is:', comp.accValue, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        comp = AccumulatorProcessor(data.input)
        comp.run()

        firstCallStack = comp.callStack.copy()
        while len(firstCallStack) > 0:
            lastIdx = firstCallStack.pop()
            if data.input[lastIdx].startswith("acc"):
                continue

            newInput = self.flip_instruction(lastIdx, data.input.copy())

            comp = AccumulatorProcessor(newInput)
            reachedEnd = comp.run()
            if reachedEnd:
                break

        return helper.validate_result('The value in the accumulator is:', comp.accValue, data.expectedAnswer)


    def solve(self):
        print("Day 8: Handheld Halting")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))