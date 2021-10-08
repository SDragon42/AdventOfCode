# from typing import AnyStr, Callable
from typing import List, Callable
import utils


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

    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    print("---- Day 6: Custom Customs ----")

    # run("Test Case 1",
    #     utils.read_input_as_list(6, "example1"),
    #     count_unique_answers,
    #     11)
    run("Problem",
        utils.read_input_as_list(6, "input"),
        count_unique_answers,
        6726)

    print("---- part 2 ----")

    # run("Test Case 1",
    #     utils.read_input_as_list(6, "example1"),
    #     count_unique_unanimous_answers,
    #     6)
    run("Problem",
        utils.read_input_as_list(6, "input"),
        count_unique_unanimous_answers,
        3316)