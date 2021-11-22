import utils
import sys
from typing import Callable, List, Dict

sys.path.append('../../Python.Common')
import helper
import inputHelper

#---------------------------------------------------------------------
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
        
#---------------------------------------------------------------------


def flip_instruction(instructionIdx: int, input: List[str]) -> List[str]:
    if input[instructionIdx].startswith("nop"):
        input[instructionIdx] = input[instructionIdx].replace("nop", "jmp")
    else:
        input[instructionIdx] = input[instructionIdx].replace("jmp", "nop")
    return input


def run_part1(title: str, input: List[str], correctResult: int):
    comp = AccumulatorProcessor(input)
    comp.run()
    utils.validate_result(title, comp.accValue, correctResult)


def run_part2(title: str, input: List[str], correctResult: int):
    comp = AccumulatorProcessor(input)
    comp.run()

    firstCallStack = comp.callStack.copy()
    while len(firstCallStack) > 0:
        lastIdx = firstCallStack.pop()
        if input[lastIdx].startswith("acc"):
            continue

        newInput = flip_instruction(lastIdx, input.copy())

        comp = AccumulatorProcessor(newInput)
        reachedEnd = comp.run()
        if reachedEnd:
            break

    utils.validate_result(title, comp.accValue, correctResult)


def solve():
    print("---- Day 8: Handheld Halting ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_list(8, "example1"),
    #     5)
    run_part1("problem",
        utils.read_input_as_list(8, "input"),
        1489)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(8, "example1"),
    #     8)
    run_part2("problem",
        utils.read_input_as_list(8, "input"),
        1539)


if __name__ == "__main__":
    solve()