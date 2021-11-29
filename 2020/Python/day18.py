from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 18
        self.input = inputHelper.load_file(day, name).splitlines()
        
        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class Puzzle(PuzzleBase):
    OPS = ["+","*"]


    def find_last(self, text: str, toFind: str) -> int:
        result = -1
        i = 0
        while i >= 0:
            i = text.find(toFind, result + 1)
            if i > result:
                result = i
        return result


    def evaluate(self, equation: str) -> int:
        while True:
            endP = equation.find(")")
            if endP < 0:
                break
            startP = self.find_last(equation[:endP], "(")

            innerEQ = equation[startP+1:endP]
            subEQ = equation[startP:endP+1]
            result = self.evaluate(innerEQ)
            equation = equation.replace(subEQ, str(result))

        equationParts = equation.split()
        result = 0
        op = ""
        for p in equationParts:
            if p in self.OPS:
                op = p
                continue

            currValue = int(p)
            if op == "":
                result = currValue
                continue

            if op == "+":
                result += currValue
                op = ""
                continue

            if op == "*":
                result *= currValue
                op = ""
                continue

        return result


    def evaluate2(self, equation: str) -> int:
        while True:
            endP = equation.find(")")
            if endP < 0:
                break
            startP = self.find_last(equation[:endP], "(")

            innerEQ = equation[startP+1:endP]
            subEQ = equation[startP:endP+1]
            result = self.evaluate2(innerEQ)
            equation = equation.replace(subEQ, str(result))

        equationParts = equation.split()
        
        i = 0
        while i < len(equationParts):
            if equationParts[i] == "+":
                a = int(equationParts[i - 1])
                b = int(equationParts[i + 1])
                equationParts[i - 1] = str(a + b)
                equationParts.pop(i)
                equationParts.pop(i)
                i = 0
                continue
            i += 1

        result = 0
        op = ""
        for p in equationParts:
            if p in self.OPS:
                op = p
                continue

            currValue = int(p)
            if op == "":
                result = currValue
                continue

            if op == "*":
                result *= currValue
                op = ""
                continue

        return result


    def run_part1(self, data: InputData) -> str:
        result = 0
        for line in data.input:
            total = self.evaluate(line)
            result += total
            helper.dprint(f"{total:-6} = {line}")

        return helper.validate_result('What is the sum of the resulting values?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        result = 0
        for line in data.input:
            total = self.evaluate2(line)
            result += total
            helper.dprint(f"{total:-6} = {line}")

        return helper.validate_result('What do you get if you add up the results of evaluating the homework problems using these new rules?', result, data.expectedAnswer)


    def solve(self):
        print("Day 18: Operation Order")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))