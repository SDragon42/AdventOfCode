
def load_file(day: int, name: str) -> str:
    """Loads the contents of a data file.

    Args:
        day (int): The puzzle date
        name (str): The name of the file (without the .TXT extention)

    Returns:
        str: The file contents
        None: None if the file was not found.
    """
    filename = f'.\\..\\input\\day{day:02d}\\{name}.txt'
    try:
        with open(filename) as dataFile:
            return dataFile.read()
    except OSError:
        return None