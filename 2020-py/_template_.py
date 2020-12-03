import utils


def run(title: str, input: list[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title_extended(0, 0, "template")

    run("Test Case #",
        utils.read_input_as_int_list(0),
        0)