import sys
from typing import List

sys.path.append('../../Python.Common')
import helper
import inputHelper


def count_trees(input: List[str], slopeX: int, slopeY: int) -> int:
    x = 0
    y = 0
    numTrees = 0
    inputWidth = len(input[0])

    while y < len(input) - 1:
        x += slopeX
        y += slopeY

        if x >= inputWidth:
            x -= inputWidth

        val = input[y][x]
        if val == '#':
            numTrees += 1

    return numTrees


def run(title: str, input: List[str], slopes: List[str], correctResult: int):
    result = 1
    for sl in slopes:
        result *= count_trees(input, int(sl[0]), int(sl[1]))
    helper.validate_result(title, result, correctResult)


def solve():
    print("Day 3: Toboggan Trajectory")
    print("")

    slopes = ["31"]
    # run("Test Case 1", 
    #     utils.read_input_as_list(3, "example1"),
    #     slopes,
    #     7)
    run("Part 1)",
        inputHelper.read_input_as_list(3, "input"),
        slopes,
        259)

    print("")

    slopes = ["11","31","51","71","12"]
    # run("Test Case 1", 
    #     utils.read_input_as_list(3, "example1"),
    #     slopes,
    #     336)
    run("Part 2)",
        inputHelper.read_input_as_list(3, "input"),
        slopes,
        2224913600)


if __name__ == "__main__":
    solve()