from typing import List


def validate_result(title: str, value, correct_value) -> None:
    success = 'CORRECT' if correct_value == value else 'WRONG'
    print(f"{title}  Result: {value}    {success}")


showDebug = False

def dprint(x) -> None:
    if showDebug:
        print(x)
