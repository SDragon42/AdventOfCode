from typing import List


def validate_result(title: str, value, correct_value) -> None:
    success = 'CORRECT' if correct_value == value else 'WRONG'
    print(f"{title}  Result: {value}    {success}")


def validate_result2(title: str, value, correct_value) -> str:
    success = 'CORRECT' if correct_value == value else 'WRONG'
    return f"{title}  Result: {value}    {success}"


showDebug = False

def dprint(x) -> None:
    if showDebug:
        print(x)
