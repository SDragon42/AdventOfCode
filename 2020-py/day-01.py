import utils


def get_value(num_splits: int, testInput: list[int], testValues: list[int]) -> int:
    if num_splits == 0:
        for x in testInput:
            sumTotal = x
            multTotal = x
            for v in testValues:
                sumTotal += v
                multTotal *= v
            if sumTotal == 2020:
                return multTotal
        return -1

    idx = 0
    while idx < len(testInput) - num_splits:
        testValues.append(testInput[idx])
        result = get_value(num_splits - 1, testInput[idx+1:], testValues)
        if result != -1:
            return result
        testValues.pop()
        idx += 1
    
    return -1


def run(title: str, num_splits: int, input: list[int], correctResult: int):
    result = get_value(num_splits, input, [])
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    print("---- Day 1: Report Repair ----")

    # run("Test case 1", 1,
    #     utils.read_input_as_int_list("day01-example1"),
    #     514579)
    run("problem", 1,
        utils.read_input_as_int_list("day01"),
        969024)

    print("---- part 2 ----")

    # run("Test case 1", 2,
    #     utils.read_input_as_int_list("day01-example1"),
    #     241861950)
    run("problem", 2,
        utils.read_input_as_int_list("day01"),
        230057040)