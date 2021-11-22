import sys
from typing import List, Dict

sys.path.append('../../Python.Common')
import helper
import inputHelper


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


def run_part1(title: str, adapters: List[int], correctResult: int):
    adapters = sorted(adapters)
    adapters.append(max(adapters) + 3)

    diffCounts = get_jolt_differences(adapters)
    result = diffCounts[1] * diffCounts[3]

    helper.validate_result(title, result, correctResult)


def run_part2(title: str, adapters: List[int], correctResult: int):
    chainsUnder: Dict[int, int] = {}
    adapters = sorted(adapters)
    
    result = count_adapterChains(0, adapters, chainsUnder)
    
    helper.validate_result(title, result, correctResult)



def solve():
    print("Day 10: Adapter Array")
    print("")

    # run_part1("Test Case 1",
    #     inputHelper.read_input_as_int_list(10, "example1"),
    #     35)
    # run_part1("Test Case 2",
    #     inputHelper.read_input_as_int_list(10, "example2"),
    #     220)
    run_part1("Part 1)",
        inputHelper.read_input_as_int_list(10, "input"),
        2414)

    print("")

    # run_part2("Test Case 1",
    #     inputHelper.read_input_as_int_list(10, "example1"),
    #     8)
    # run_part2("Test Case 2",
    #     inputHelper.read_input_as_int_list(10, "example2"),
    #     19208)
    run_part2("Part 2)",
        inputHelper.read_input_as_int_list(10, "input"),
        21156911906816)


if __name__ == "__main__":
    solve()