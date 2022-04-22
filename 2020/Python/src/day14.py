import math
from typing import List, Dict

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 14
        self.input = inputHelper.load_file(day, name).splitlines()
        
        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class Puzzle(PuzzleBase):
    bitWidth: int = 36
    memory: Dict[int, int] = {}
    mask: str = "".rjust(bitWidth, "X")


    def int_to_binary(self, value: int) -> List[str]:
        ba = list(bin(value)[2:].rjust(self.bitWidth, "0"))
        return ba


    def binary_to_int(self, bValue: List[str]) -> int:
        value = int("".join(bValue), 2)
        return value


    def set_mask(self, line: str):
        self.mask = line[7:]


    def apply_mask_v1(self, bValue: List[str]):
        i = 0
        while i < len(self.mask):
            if self.mask[i] != "X":
                bValue[i] = self.mask[i]
            i += 1


    def set_memory_v1(self, line: str):
        x = line.index("]")
        address = int(line[4:x])
        x += 4
        value = int(line[x:])

        bValue = self.int_to_binary(value)
        self.apply_mask_v1(bValue)
        value = self.binary_to_int(bValue)

        self.memory[address] = value


    def apply_mask_v2(self, bValue: List[str]):
        i = 0
        while i < len(self.mask):
            if self.mask[i] != "0":
                bValue[i] = self.mask[i]
            i += 1


    def flip_address_bits(self, bValue: List[str], replacements: str) -> List[str]:
        i = 0
        while i < len(bValue):
            if bValue[i] == "X":
                bValue[i] = replacements[0]
                replacements = replacements[1:]
            i += 1
        return bValue


    def get_all_possible_addresses(self, bValue: List[str]) -> List[int]:
        addresses: List[int] = []
        count = self.mask.count("X")
        num = int(math.pow(2, count))
        rangeNums = range(num)
        for r in rangeNums:
            br = bin(r)[2:].rjust(count, "0")
            newAddr = self.flip_address_bits(bValue.copy(), br)
            newAddr2 = self.binary_to_int(newAddr)
            addresses.append(newAddr2)
            pass
        return addresses


    def set_memory_v2(self, line: str):
        x = line.index("]")
        address = int(line[4:x])
        x += 4
        value = int(line[x:])

        bAddress = self.int_to_binary(address)
        self.apply_mask_v2(bAddress)
        addresses = self.get_all_possible_addresses(bAddress)
        for addr in addresses:
            self.memory[addr] = value


    def run_part1(self, data: InputData) -> str:
        self.memory.clear()

        actions = {
            "mas" : self.set_mask,
            "mem" : self.set_memory_v1
        }

        for line in data.input:
            key = line[:3]
            actions[key](line)

        result = 0
        for key in self.memory:
            result += self.memory[key]
            pass

        return helper.validate_result('What is the sum of all values left in memory after it completes?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        self.memory.clear()

        actions = {
            "mas" : self.set_mask,
            "mem" : self.set_memory_v2
        }

        for line in data.input:
            key = line[:3]
            actions[key](line)

        result = 0
        for key in self.memory:
            result += self.memory[key]
            pass

        return helper.validate_result('What is the sum of all values left in memory after it completes?', result, data.expectedAnswer)


    def solve(self):
        print("Day 14: Docking Data")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example2', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))