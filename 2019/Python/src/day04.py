from cgitb import reset
from typing import Callable, List, Tuple

def get_password_range(input:str) -> Tuple[int, int]:
    parts = input.split('-')
    return int(parts[0]), int(parts[1])


def part1(input:str) -> int:
    passwordMin, passwordMax = get_password_range(input)

    rules = [
        rule_is_six_digits,
        rule_two_adjacent_digits_are_same,
        rule_digits_never_decrease,
    ]

    result = count_valid_passwords(passwordMin, passwordMax, rules)
    return result

    

def part2(input:str) -> int:
    passwordMin, passwordMax = get_password_range(input)

    rules = [
        rule_is_six_digits,
        rule_only_two_adjacent_digits_are_same,
        rule_digits_never_decrease,
    ]

    result = count_valid_passwords(passwordMin, passwordMax, rules)
    return result

def count_valid_passwords(passwordMin:int, passwordMax:int, rules:List[Callable]) -> int:
    numValidPasswords = 0
    for password in range(passwordMin, passwordMax + 1):
        if is_password_valid(password, rules):
            numValidPasswords += 1
    return numValidPasswords


def is_password_valid(password:int, rules:List[Callable]) -> bool:
    digits = get_digits(password)
    result = all([r(digits) for r in rules])
    return result

def get_digits(value:int) -> List[int]:
    digits = [int(i) for i in str(value)]
    return digits


def rule_is_six_digits(digits:List[int]) -> bool:
    result = len(digits) == 6
    return result

def rule_two_adjacent_digits_are_same(digits:List[int]) -> bool:
    indexes = range(1, len(digits))
    for i in indexes:
        if digits[i] == digits[i-1]:
            return True
    return False

def rule_only_two_adjacent_digits_are_same(digits:List[int]) -> bool:
    indexes = range(1, len(digits))
    repeatCount = 0
    last = digits[0]
    for i in indexes:
        current = digits[i]

        if current == last:
            repeatCount += 1
            continue

        if repeatCount == 1:
            break

        repeatCount = 0
        last = current

    result = repeatCount == 1
    return result

def rule_digits_never_decrease(digits:List[int]) -> bool:
    indexes = range(1, len(digits))
    for i in indexes:
        if digits[i] < digits[i-1]:
            return False
    return True
