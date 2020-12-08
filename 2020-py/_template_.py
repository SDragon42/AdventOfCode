import utils


def run(title: str, input: list[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    print("---- Day 0: Title ----")

    run("Test Case 1",
        utils.read_input_as_list(0, "example1"),
        0)
    run("problem",
        utils.read_input_as_list(0, "input"),
        0)

    print("---- part 2 ----")

    run("problem",
        utils.read_input_as_list(0, "input"),
        0)