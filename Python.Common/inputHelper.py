
def load_file(year: int, day: int, name: str) -> str:
    """Loads the contents of a data file.

    Args:
        year (int): The puzzle year
        day (int): The puzzle date
        name (str): The name of the file (without the .TXT extension)

    Returns:
        str: The file contents
        None: None if the file was not found.
    """
    filename = f'..\\..\\..\\AdventOfCode.Input\\{year}\\day{day:02d}\\{name}.txt'
    try:
        with open(filename) as dataFile:
            return dataFile.read()
    except OSError:
        return None