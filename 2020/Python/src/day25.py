from typing import List

# import helper



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


def run_part1(input: List[int]) -> int:
    cardPublicKey = input[0]
    doorPublicKey = input[1]

    cardLoopSize = find_loop_size(cardPublicKey)
    doorLoopSize = find_loop_size(doorPublicKey)
    # helper.dprint(f"card key: {cardPublicKey}   loop size: {cardLoopSize}")
    # helper.dprint(f"door key: {doorPublicKey}   loop size: {doorLoopSize}")

    result1 = calculate_encryption_key(cardPublicKey, doorLoopSize)
    result2 = calculate_encryption_key(doorPublicKey, cardLoopSize)

    # if result1 == result2:
    #     helper.dprint("    KEY MATCH")
    # helper.dprint(f"key: {result1} - {result2}")

    return result1
