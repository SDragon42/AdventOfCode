from typing import List


def show_title(day: int, puzzle: int, title: str) -> None:
    """ Shows the Day/Puzzle/title header """
    print("------------------------------------------------------------")
    print(f"Day {day} Puzzle {puzzle} - {title}")
    print("------------------------------------------------------------")
    print()


def validate_result(title: str, value, correct_value) -> None:
    success = 'CORRECT' if correct_value == value else 'WRONG'
    print(f"{title}  --  Result: {value}    {success}")


showDebug = False

def dprint(x) -> None:
    if showDebug:
        print(x)
