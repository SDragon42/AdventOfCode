import utils
from typing import List


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
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    print("---- Day 3: Toboggan Trajectory ----")

    slopes = ["31"]
    # run("Test Case 1", 
    #     utils.read_input_as_list(3, "example1"),
    #     slopes,
    #     7)
    run("problem",
        utils.read_input_as_list(3, "input"),
        slopes,
        259)

    print("---- part 2 ----")

    slopes = ["11","31","51","71","12"]
    # run("Test Case 1", 
    #     utils.read_input_as_list(3, "example1"),
    #     slopes,
    #     336)
    run("problem",
        utils.read_input_as_list(3, "input"),
        slopes,
        2224913600)