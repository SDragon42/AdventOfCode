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



def run(title: str, input: [], correctResult: int):
    result = 0
    for entry in input:
        result += check_password(entry)

    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(2, 1, "Password Philosophy")

    run("Test Case 1",
        utils.read_input_as_list("day02-example1"),
        2)

    run("problem",
        utils.read_input_as_list("day02"),
        548)