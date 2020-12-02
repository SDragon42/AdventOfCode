import utils


def run(title, input, correctResult):
    result = 0
    print(f"{title}  --  Result: {result}")
    if correctResult == result:
        print("   CORRECT")
    else:
        print("   WRONG")
    print()


if __name__ == "__main__":
    utils.show_title(0, 0)
    # utils.show_title_extended(0, 0, "template")

    run("Test Case #",
        utils.read_input_as_int_list(0),
        0)