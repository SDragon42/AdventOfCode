import sys
from typing import List

sys.path.append('../../Python.Common')
import helper
import inputHelper


def loop_transform(value: int, subjectNumber: int) -> int:
    value *= subjectNumber
    value = value % 20201227
    return value


def find_loop_size(publicKey: int) -> int:
    loopSize = 0
    calcKey = 1
    while True:
        loopSize += 1
        calcKey = loop_transform(calcKey, 7)
        if calcKey == publicKey:
            break
    return loopSize


def calculate_encryption_key(publicKey: int, loopSize: int) -> int:
    l = 0
    encryptKey = 1
    while l < loopSize:
        encryptKey = loop_transform(encryptKey, publicKey)
        l += 1
    return encryptKey


def run_part1(title: str, input: List[int], correctResult: int):
    cardPublicKey = input[0]
    doorPublicKey = input[1]

    cardLoopSize = find_loop_size(cardPublicKey)
    doorLoopSize = find_loop_size(doorPublicKey)
    helper.dprint(f"card key: {cardPublicKey}   loop size: {cardLoopSize}")
    helper.dprint(f"door key: {doorPublicKey}   loop size: {doorLoopSize}")

    result1 = calculate_encryption_key(cardPublicKey, doorLoopSize)
    result2 = calculate_encryption_key(doorPublicKey, cardLoopSize)

    if result1 == result2:
        helper.dprint("    KEY MATCH")
    helper.dprint(f"key: {result1} - {result2}")

    helper.validate_result(title, result1, correctResult)


def solve():
    day = 25
    print(f"Day {day}: Combo Breaker")
    print("")


    # run_part1("Test Case 1",
    #     [5764801,17807724],
    #     14897079)
    run_part1("problem",
        [8458505,16050997],
        448851)


if __name__ == "__main__":
    solve()