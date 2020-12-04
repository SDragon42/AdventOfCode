def get_filename(day: int) -> str:
    """ Gets the input file for a specified day """
    filename = f'.\\data\\day{day:02}.txt'
    return filename


def read_input_as_int_list(day: int) -> list[int]:
    """ Reads the input file as a list of ints """
    filename = get_filename(day)
    data = []
    with open(filename) as dataFile:
        while True:
            line = dataFile.readline()
            if not line:
                break
            line = line.strip()
            value = int(line)
            data.append(value)
    return data


def read_input_as_list(day: int):
    """ Reads the input file as a list of ints """
    filename = get_filename(day)
    data = []
    with open(filename) as dataFile:
        while True:
            line = dataFile.readline()
            if not line:
                break
            data.append(line.strip())
    return data


def read_input_as_list2(filename: str):
    """ Reads the input file as a list of ints """
    filename = f'.\\data\\{filename}.txt'
    data = []
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
