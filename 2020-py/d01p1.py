import utils


def get_value(input):
    pos = 0
    idx = pos + 1

    while(pos + 1 < len(input)):

        if (input[pos] + input[idx] == 2020):
            return input[pos] * input[idx]

        idx += 1

        if (idx < len(input) - 1):
            continue

        pos += 1
        idx = pos + 1

        continue


def run(title, input, correctResult):
    result = get_value(input)
    print(f"{title}  --  Result: {result}")
    if correctResult == result:
        print("   CORRECT")
    else:
        print("   WRONG")
    print()


if __name__ == "__main__":
    print("Day 1 Puzzle 1")
    print("------------------------------------------------------------")
    print()

    # run("Test case 1",
    #     [1721,979,366,299,675,1456],
    #     514579)

    run("problem",
        utils.read_input_as_int_list(1),
        969024)