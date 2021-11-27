from typing import List, Dict

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str] = []
    expectedAnswer: int = None

    def __init__(self, name: str, part: int) -> None:
        day = 7
        self.input = inputHelper.load_input_file(day, name)
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



class BagInfo:
    bag: str
    count: int

    def __init__(self, bag, count):
        self.bag = bag
        self.count = count



BagList = List[BagInfo]
BagDict = Dict[str, BagList]



class Puzzle(PuzzleBase):

    def clean_bag_name(self, name: str) -> str:
        name = name.replace(" bags", "")
        name = name.replace(" bag", "")
        name = name.strip()
        return name


    def parse_rules(self, input: List[str]) -> BagDict:
        rules: BagDict = {}

        for line in input:
            lineParts = line.split(" contain ")
            bag = self.clean_bag_name(lineParts[0])
            contents: BagList = []

            subBags = lineParts[1].split(",")
            for subBag in subBags:
                subBag = self.clean_bag_name(subBag.strip(" ,."))
                if subBag == "no other":
                    continue

                subBagParts = subBag.split(" ", 1)
                subBagName = subBagParts[1]
                subBagCount = int(subBagParts[0])
                info = BagInfo(subBagName, subBagCount)
                contents.append(info)

            rules[bag] = contents

        return rules


    def count_bags_containing(self, rules: BagDict, bag: str, foundBags: List[str]):
        for ruleKey in rules:
            for contentBag in rules[ruleKey]:
                if contentBag.bag == bag:
                    found = ruleKey
                    if found not in foundBags:
                        foundBags.append(found)
                        self.count_bags_containing(rules, found, foundBags)


    def count_bags_in(self, rules: BagDict, bag: str) -> int:
        result = 0
        if bag in rules:
            contents = rules[bag]
            for contentBag in contents:
                result += contentBag.count + (contentBag.count * self.count_bags_in(rules, contentBag.bag))
        return result


    def run_part1(self, data: InputData) -> str:
        rules = self.parse_rules(data.input)
        foundBags: List[str] = []
        self.count_bags_containing(rules, "shiny gold", foundBags)
        result = len(foundBags)
        return helper.validate_result('How many bag colors can eventually contain at least one shiny gold bag?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        rules = self.parse_rules(data.input)
        result = self.count_bags_in(rules, "shiny gold")
        return helper.validate_result('How many individual bags are required inside your single shiny gold bag?', result, data.expectedAnswer)


    def solve(self):
        print("Day 7: Handy Haversacks")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_example(lambda: "P2 Ex2) " + self.run_part2(InputData('example2', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))