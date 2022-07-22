from typing import Callable, List, Dict


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



def flip_instruction(instructionIdx: int, input: List[str]) -> List[str]:
    if input[instructionIdx].startswith("nop"):
        input[instructionIdx] = input[instructionIdx].replace("nop", "jmp")
    else:
        input[instructionIdx] = input[instructionIdx].replace("jmp", "nop")
    return input


def run_part1(input: List[str]) -> int:
    comp = AccumulatorProcessor(input)
    comp.run()
    return comp.accValue


def run_part2(input: List[str]) -> int:
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

    return comp.accValue
