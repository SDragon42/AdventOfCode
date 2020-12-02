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
    print(f"{title}  --  Result: {result}")
    if correctResult == result:
        print("   CORRECT")
    else:
        print("   WRONG")
    print()


if __name__ == "__main__":
    print("Day 1 Puzzle 2")
    print("------------------------------------------------------------")
    print()

    # run("Test case 1",
    #     [1721,979,366,299,675,1456],
    #     241861950)

    run("problem",
        utils.read_input_as_int_list(1),
        230057040)