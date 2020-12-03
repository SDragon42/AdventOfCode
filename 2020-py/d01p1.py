import utils


def get_value(input: list[int]) -> int:
    pos = 0
    idx = pos + 1

    while pos + 1 < len(input):

        if (input[pos] + input[idx] == 2020):
            return input[pos] * input[idx]

        idx += 1

        if (idx < len(input) - 1):
            continue

        pos += 1
        idx = pos + 1

        continue


def run(title: str, input: list[int], correctResult: int):
    result = get_value(input)
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(1, 1, "Report Repair")

    # run("Test case 1",
    #     [1721,979,366,299,675,1456],
    #     514579)
    
    run("problem",
        utils.read_input_as_int_list(1),
        969024)