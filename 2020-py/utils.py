def get_filename(day):
    """ Gets the input file for a specified day """
    filename = f'.\\data\\day{day:02}.txt'
    return filename


def read_input_as_int_list(day):
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