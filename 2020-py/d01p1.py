# import math
from inputData import get_input_d01, get_answer_d01p1


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


if __name__ == "__main__":
    print("Day 1 Puzzle 1")
    print("------------------------------------------------------------")
    print()

    # input = [1721,979,366,299,675,1456] # result = 514579
    input = get_input_d01()

    result = get_value(input)
    print(f"Result: {result}")
    if get_answer_d01p1() == result:
        print("   CORRECT")

