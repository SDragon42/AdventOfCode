def generate_filename(day: int, name: str) -> str:
    filename = f".\\data\\day{day:02d}\\{name}.txt"
    return filename


def read_input_as_int_list(day: int, name: str) -> list[int]:
    """ Reads the input file as a list of ints """
    filename = generate_filename(day, name)
    data: list[int] = []
    with open(filename) as dataFile:
        while True:
            line = dataFile.readline()
            if not line:
                break
            line = line.strip()
            value = int(line)
            data.append(value)
    return data


def read_input_as_list(day: int, name: str) -> list[str]:
    """ Reads the input file as a list of ints """
    filename = generate_filename(day, name)
    data: list[str] = []
    with open(filename) as dataFile:
        while True:
            line = dataFile.readline()
            if not line:
                break
            data.append(line.strip())
    return data


def show_title(day: int, puzzle: int, title: str):
    """ Shows the Day/Puzzle/title header """
    print("------------------------------------------------------------")
    print(f"Day {day} Puzzle {puzzle} - {title}")
    print("------------------------------------------------------------")
    print()


def validate_result(title: str, value, correct_value):
    print(f"{title}  --  Result: {value}")
    if correct_value == value:
        print("   CORRECT")
    else:
        print("   WRONG")
