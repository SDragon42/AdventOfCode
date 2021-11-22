import sys
from typing import List

sys.path.append('../../Python.Common')
import helper
import inputHelper


def is_valid(value: int, input: List[int]) -> bool:
    if len(input) == 0:
        return False

    first = input[0]
    rest = input[1:]
    for x in rest:
        if first + x == value:
            return True

    return is_valid(value, rest)


def scan_input(input: List[int], windowSize: int) -> int:
    lowIdx = 0
    highIdx = lowIdx + windowSize
    i = windowSize

    while i < len(input):
        if not is_valid(input[i], input[lowIdx:highIdx]):
            return input[i]

        lowIdx += 1
        highIdx += 1
        i += 1
    return - 1


def find_weakness(input: List[int], invalidNum: int) -> int:
    start = 0
    end = 1

    while end < len(input):
        testInput = input[start:end]
        total = sum(testInput)
        if total == invalidNum:
            a = min(testInput)
            b = max(testInput)
            return a + b

        if total < invalidNum:
            end += 1
            continue

        if total > invalidNum:
            start += 1
            continue
    return -1


def run_part1(title: str, input: List[int], windowSize: int, correctResult: int):
    result = scan_input(input, windowSize)
    helper.validate_result(title, result, correctResult)


def run_part2(title: str, input: List[int], windowSize: int, correctResult: int):
    invalidNum = scan_input(input, windowSize)
    result = find_weakness(input, invalidNum)
    helper.validate_result(title, result, correctResult)


def solve():
    print("Day 9: Encoding Error")
    print("")

    # run_part1("Test Case 1",
    #     inputHelper.read_input_as_int_list(9, "example1"),
    #     5,
    #     127)
    run_part1("Part 1)",
        inputHelper.read_input_as_int_list(9, "input"),
        25,
        36845998)

    print("")

    # run_part2("Test Case 1",
    #     inputHelper.read_input_as_int_list(9, "example1"),
    #     5,
    #     62)
    run_part2("Part 2)",
        inputHelper.read_input_as_int_list(9, "input"),
        25,
        4830226)


if __name__ == "__main__":
    solve()