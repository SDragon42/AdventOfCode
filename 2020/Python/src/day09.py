from typing import List

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
