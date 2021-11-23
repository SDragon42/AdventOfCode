from typing import List


def load_input_file(day: int, name: str) -> list[str]:
    filename = f".\\..\\input\\day{day:02d}\\{name}.txt"
    with open(filename) as dataFile:
        return dataFile.readlines()


def load_answer_file(day: int, part: int, name: str) -> list[str]:
    filename = f".\\..\\input\\day{day:02d}\\{name}-answer{part}.txt"
    try:
        with open(filename) as dataFile:
            return dataFile.readlines()
    except OSError:
        return None


def generate_filename(day: int, name: str) -> str:
    filename = f".\\..\\input\\day{day:02d}\\{name}.txt"
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
    """ Reads the input file as a List of strings """
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