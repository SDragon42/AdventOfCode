import math
from typing import List


def validate_result(aocQuestion: str, value, correct_value) -> str:
    """
    Displays the results of the 'part', and if the answer was correct or wrong.

    Args:
        aocQuestion (str): This is the AOC question text.
        value ([type]): The value that was found.
        correct_value ([type]): This is the correct value.

    Returns:
        str: a composition of '{aocQuestion}    {value}    {check}'
    """
    check = 'WRONG'
    if correct_value is None:
        check = ''
    elif correct_value == value:
        check = 'CORRECT'
    return f"{aocQuestion}    {value}    {check}".rstrip()



showDebug = False

def dprint(x) -> None:
    """
    A print() that only will run if the 'showDebug' value is set to True.

    Args:
        x ([type]): a value to print
    """
    if showDebug:
        print(x)


def get_lcm(a: int, b: int) -> int:
    """
    Gets the Least Common Multiple of two numbers

    Args:
        a (int): Value A
        b (int): Value B

    Returns:
        int: The least common multiple
    """
    return abs(a * b) // math.gcd(a, b)

def string_to_int_list(value:str) -> List[int]:
    """
    Returns a int list of a comma separate string of numbers.

    Args:
        value (str): a string that is a comma separate list of integers.

    Returns:
        List[int]: a list of int values
    """
    result = [int(a) for a in value.split(',')]
    return result