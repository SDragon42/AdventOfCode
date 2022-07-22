from typing import List, Dict


class BagInfo:
    bag: str
    count: int

    def __init__(self, bag, count):
        self.bag = bag
        self.count = count



BagList = List[BagInfo]
BagDict = Dict[str, BagList]



def clean_bag_name(name: str) -> str:
    name = name.replace(" bags", "")
    name = name.replace(" bag", "")
    name = name.strip()
    return name


def parse_rules(input: List[str]) -> BagDict:
    rules: BagDict = {}

    for line in input:
        lineParts = line.split(" contain ")
        bag = clean_bag_name(lineParts[0])
        contents: BagList = []

        subBags = lineParts[1].split(",")
        for subBag in subBags:
            subBag = clean_bag_name(subBag.strip(" ,."))
            if subBag == "no other":
                continue

            subBagParts = subBag.split(" ", 1)
            subBagName = subBagParts[1]
            subBagCount = int(subBagParts[0])
            info = BagInfo(subBagName, subBagCount)
            contents.append(info)

        rules[bag] = contents

    return rules


def count_bags_containing(rules: BagDict, bag: str, foundBags: List[str]):
    for ruleKey in rules:
        for contentBag in rules[ruleKey]:
            if contentBag.bag == bag:
                found = ruleKey
                if found not in foundBags:
                    foundBags.append(found)
                    count_bags_containing(rules, found, foundBags)


def count_bags_in(rules: BagDict, bag: str) -> int:
    result = 0
    if bag in rules:
        contents = rules[bag]
        for contentBag in contents:
            result += contentBag.count + (contentBag.count * count_bags_in(rules, contentBag.bag))
    return result


def run_part1(input: List[str]) -> str:
    rules = parse_rules(input)
    foundBags: List[str] = []
    count_bags_containing(rules, "shiny gold", foundBags)
    result = len(foundBags)
    return result


def run_part2(input: List[str]) -> str:
    rules = parse_rules(input)
    result = count_bags_in(rules, "shiny gold")
    return result
