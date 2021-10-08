import utils
from typing import List


def run_part1(title: str, input: List[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: List[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    day = 0
    print(f"---- Day {day}: Title ----")

    run_part1("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        0)
    # run_part1("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)

    # print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     0)
    # run_part2("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)