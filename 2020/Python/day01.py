# import utils
import sys
from typing import List

sys.path.append('../../Python.Common')
import helper
import inputHelper


def get_value(num_splits: int, testInput: List[int], testValues: List[int]) -> int:
    if num_splits == 0:
        for x in testInput:
            sumTotal = x
            multTotal = x
            for v in testValues:
                sumTotal += v
                multTotal *= v
            if sumTotal == 2020:
                return multTotal
        return -1

    idx = 0
    while idx < len(testInput) - num_splits:
        testValues.append(testInput[idx])
        result = get_value(num_splits - 1, testInput[idx+1:], testValues)
        if result != -1:
            return result
        testValues.pop()
        idx += 1
    
    return -1


def run(title: str, num_splits: int, input: List[int], correctResult: int):
    result = get_value(num_splits, input, [])
    helper.validate_result(title, result, correctResult)

# def run_example(o: utils.PuzzleOptions, title: str, num_splits: int, input: List[int], correctResult: int) -> None:
#     if (o.runExamples):
#         run(title, num_splits, input, correctResult)

# def solve(o: utils.PuzzleOptions):
def solve():
    print("Day 1: Report Repair")
    print("")
    # run("Test case 1", 1,
    #     utils.read_input_as_int_list(1,"example1"),
    #     514579)
    run("Part 1)", 1,
        inputHelper.read_input_as_int_list(1, "input"),
        969024)

    # print("---- part 2 ----")
    print("")

    # run("Test case 1", 2,
    #     utils.read_input_as_int_list(1, "example1"),
    #     241861950)
    run("Part 2)", 2,
        inputHelper.read_input_as_int_list(1, "input"),
        230057040)


if __name__ == "__main__":
    solve()