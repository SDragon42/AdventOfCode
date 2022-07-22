from typing import List, Callable

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


def run_part(input: List[str], process: Callable[[List[str]], int]) -> str:
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

    return result
