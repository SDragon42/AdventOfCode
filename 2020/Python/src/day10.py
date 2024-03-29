from typing import List, Dict

# import helper
# import inputHelper
# from puzzleBase import PuzzleBase



# class InputData:
#     input: List[int]
#     expectedAnswer: int

#     def __init__(self, name: str, part: int) -> None:
#         day = 10
#         lines = inputHelper.load_file(day, name).splitlines()
#         self.input = [int(l) for l in lines]

#         answer = inputHelper.load_file(day, f"{name}-answer{part}")
#         self.expectedAnswer = int(answer) if answer is not None else None



# class Puzzle(PuzzleBase):

def get_jolt_differences(adapters: List[int]) -> Dict[int, int]:
    diffDict: Dict[int, int] = {}

    last = 0
    for a in adapters:
        diff = a - last
        if diff in diffDict:
            diffDict[diff] = diffDict[diff] + 1
        else:
            diffDict[diff] = 1
        last = a
    return diffDict


def get_next_possible_adapters(current: int, adapters: List[int]) -> List[int]:
    result: List[int] = []
    for x in adapters:
        if x > current and x <= current + 3:
            result.append(x)
    return result


def count_adapterChains(current:int, adapters: List[int], chainsUnder: Dict[int, int]) -> int:
    if current in chainsUnder:
        return chainsUnder[current]

    nextAdapters = get_next_possible_adapters(current, adapters)
    if len(nextAdapters) == 0:
        return 1

    result = 0
    for x in nextAdapters:
        result += count_adapterChains(x, adapters, chainsUnder)
    chainsUnder[current] = result
    return result


def run_part1(input: List[int]) -> int:
    adapters = sorted(input)
    adapters.append(max(adapters) + 3)

    diffCounts = get_jolt_differences(adapters)
    result = diffCounts[1] * diffCounts[3]
    return result
    # return helper.validate_result('What is the number of 1-jolt differences multiplied by the number of 3-jolt differences?', result, data.expectedAnswer)


def run_part2(input: List[int]) -> int:
    chainsUnder: Dict[int, int] = {}
    adapters = sorted(input)

    result = count_adapterChains(0, adapters, chainsUnder)
    return result
    # return helper.validate_result('What is the total number of distinct ways you can arrange the adapters to connect the charging outlet to your device?', result, data.expectedAnswer)



# def solve(self):
#     print("Day 10: Adapter Array")
#     print("")

#     self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
#     self.run_example(lambda: "P1 Ex2) " + self.run_part1(InputData('example2', 1)))
#     self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

#     print("")

#     self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
#     self.run_example(lambda: "P2 Ex2) " + self.run_part2(InputData('example2', 2)))
#     self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))
