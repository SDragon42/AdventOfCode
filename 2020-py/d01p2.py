import utils


def get_value(input: list[int]) -> int:
    pos = 0
    idx = pos + 1
    idx2 = pos + 2

    while pos + 2 < len(input):

        if (input[pos] + input[idx] + input[idx2] == 2020):
            return input[pos] * input[idx] * input[idx2]

        idx2 += 1

        if (idx2 < len(input)):
            continue

        idx += 1
        idx2 = idx + 1

        if (idx < len(input) - 1):
            continue

        pos += 1
        idx = pos + 1
        idx2 = idx + 1

        continue


def run(title: str, input: list[int], correctResult: int):
    result = get_value(input)
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(1, 2, "Report Repair")

    run("Test case 1",
        utils.read_input_as_int_list("day01-example1"),
        241861950)

    run("problem",
        utils.read_input_as_int_list("day01"),
        230057040)