from typing import List


def validate_result(message: str, value, correct_value) -> None:
    print(validate_result2(message, value, correct_value))


def validate_result2(message: str, value, correct_value) -> str:
    success = 'WRONG'
    if correct_value is None:
        success = '???'
    elif correct_value == value:
        success = 'CORRECT'
    return f"{message}    {value}    {success}"


showDebug = False

def dprint(x) -> None:
    if showDebug:
        print(x)
