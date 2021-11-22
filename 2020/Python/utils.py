from typing import List

def generate_filename(day: int, name: str) -> str:
    filename = f".\\data\\day{day:02d}\\{name}.txt"
    return filename


def read_input_as_int_list(day: int, name: str) -> List[int]:
    """ Reads the input file as a List of ints """
    filename = generate_filename(day, name)
    data: List[int] = []
    with open(filename) as dataFile:
        while True:
            line = dataFile.readline()
            if not line:
                break
            line = line.strip()
            value = int(line)
            data.append(value)
    return data


def read_input_as_list(day: int, name: str) -> List[str]:
    """ Reads the input file as a List of ints """
    filename = generate_filename(day, name)
    data: List[str] = []
    with open(filename) as dataFile:
        while True:
            line = dataFile.readline()
            if not line:
                break
            data.append(line.strip())
    return data


def read_input(day: int, name: str) -> str:
    """ Reads the input file as a single string """
    filename = generate_filename(day, name)
    with open(filename) as dataFile:
        result = dataFile.read()
        return result


def show_title(day: int, puzzle: int, title: str) -> None:
    """ Shows the Day/Puzzle/title header """
    print("------------------------------------------------------------")
    print(f"Day {day} Puzzle {puzzle} - {title}")
    print("------------------------------------------------------------")
    print()


def validate_result(title: str, value, correct_value) -> None:
    print(f"{title}  --  Result: {value}")
    if correct_value == value:
        print("   CORRECT")
    else:
        print("   WRONG")


showDebug = False

def dprint(x):
    if showDebug:
        print(x)


# class PuzzleOptions:
#     runExamples: bool = False

#     def __init__(self) -> None:
#         self.runExamples = False