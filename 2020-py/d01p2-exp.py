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


def run(title: str, input: list[int], correctResult: int):
    result = get_value(2, input, [])
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(1, 2, "Report Repair (refactored)")

    # run("Test case 1",
    #     [1721,979,366,299,675,1456],
    #     241861950)

    run("problem",
        utils.read_input_as_int_list(1),
        230057040)