# from typing import AnyStr, Callable
import sys
from typing import List, Callable

sys.path.append('../../Python.Common')
import helper
import inputHelper


def count_unique_answers(lines: List[str]) -> int:
    answers = []

    for line in lines:
        for c in line:
            if answers.count(c) == 0:
                answers.append(c)
    
    return len(answers)


def count_unique_unanimous_answers(lines: List[str]) -> int:
    answers = {}
    pCount = 0

    for line in lines:
        pCount += 1
        for c in line:
            if c in answers:
                answers[c] = answers[c] + 1
            else:
                answers[c] = 1

    result = 0
    for a in answers:
        if answers[a] == pCount:
            result += 1
    
    return result


def run(title: str, input: List[str], process: Callable[[List[str]], int], correctResult: int):
    result = 0
    groupStart = 0
    groupEnd = 0
    while groupStart < len(input):
        try:
            groupEnd = input.index("", groupStart)
        except:
            groupEnd = len(input)

        lines = input[groupStart:groupEnd]
        result += process(lines)

        groupStart = groupEnd + 1

    helper.validate_result(title, result, correctResult)


def solve():
    print("Day 6: Custom Customs")
    print("")

    # run("Test Case 1",
    #     inputHelper.read_input_as_list(6, "example1"),
    #     count_unique_answers,
    #     11)
    run("Part 1)",
        inputHelper.read_input_as_list(6, "input"),
        count_unique_answers,
        6726)

    print("")

    # run("Test Case 1",
    #     inputHelper.read_input_as_list(6, "example1"),
    #     count_unique_unanimous_answers,
    #     6)
    run("Part 2)",
        inputHelper.read_input_as_list(6, "input"),
        count_unique_unanimous_answers,
        3316)


if __name__ == "__main__":
    solve()