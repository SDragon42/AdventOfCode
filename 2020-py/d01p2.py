# import math
from inputData import get_input_d01, get_answer_d01p2


def get_value(input):
    pos = 0
    idx = pos + 1
    idx2 = pos + 2

    while(pos + 2 < len(input)):

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


if __name__ == "__main__":
    print("Day 1 Puzzle 2")
    print("------------------------------------------------------------")
    print()

    # input = [1721,979,366,299,675,1456] # result = 241861950
    input = get_input_d01()

    result = get_value(input)
    print(f"Result: {result}")
    if get_answer_d01p2() == result:
        print("   CORRECT")

