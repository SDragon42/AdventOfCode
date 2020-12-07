import utils


def check_password(entry: str) -> int:
    entryParts = entry.split()

    range = entryParts[0].split('-')
    min = int(range[0])
    max = int(range[1])

    letter = entryParts[1].split(":")[0]

    password = entryParts[2]

    count = 0
    for x in password:
        if (x == letter):
            count += 1

    if (min <= count <= max):
        return 1
    return 0

def check_password2(entry: str) -> int:
    entryParts = entry.split()
    
    positions = entryParts[0].split('-')
    letter = entryParts[1].split(":")[0]
    password = entryParts[2]

    matches = 0
    for x in positions:
        check = password[int(x) - 1]
        if check == letter:
            matches += 1
    
    if matches == 1:
        return 1
    return 0



def run_part1(title: str, input: [], correctResult: int):
    result = 0
    for entry in input:
        result += check_password(entry)
    utils.validate_result(title, result, correctResult)

def run_part2(title: str, input: [], correctResult: int):
    result = 0
    for entry in input:
        result += check_password2(entry)
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    print("---- Day 2: Password Philosophy ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_list("day02-example1"),
    #     2)
    run_part1("problem",
        utils.read_input_as_list("day02"),
        548)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list("day02-example1"),
    #     1)
    run_part2("problem",
        utils.read_input_as_list("day02"),
        502)