from typing import List


def get_value(num_splits: int, testInput: List[int], testValues: List[int]) -> int:
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
