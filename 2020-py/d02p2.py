import utils


def check_password(entry: str) -> int:
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



def run(title: str, input: [], correctResult: int):
    result = 0
    for entry in input:
        result += check_password(entry)

    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(2, 2, "Password Philosophy")

    run("Test Case 1",
        utils.read_input_as_list("day02-example1"),
        1)

    run("problem",
        utils.read_input_as_list("day02"),
        502)