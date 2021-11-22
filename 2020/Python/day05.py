import utils
import sys
from typing import List

sys.path.append('../../Python.Common')
import helper
import inputHelper


def get_row(lowCode: str, highCode: str, lowStart: int, highStart: int, input: str) -> int:
    low = lowStart
    high = highStart
    for x in input:
        byHalf = (high - low + 1) / 2
        if x == highCode:
            high -= byHalf
        elif x == lowCode:
            low += byHalf
    return int(low - 1)


def get_seat_id(boardingPass: str) -> int:
    rowCode = boardingPass[:7]
    colCode = boardingPass[7:]

    row = get_row("B", "F", 1, 128, rowCode)
    col = get_row("R", "L", 1, 8, colCode)
    seatId = (row * 8) + col
    
    return seatId


def run_part1(title: str, input: List[str], correctResult: int):
    result = 0
    for bPass in input:
        seatId = get_seat_id(bPass)
        if seatId > result:
            result = seatId
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: List[str], correctResult: int):
    seats = []
    for x in range(1024):
        seats.append(x)
    
    for bPass in input:
        seatId = get_seat_id(bPass)
        seats[seatId] = 0

    result = -1

    for x in seats[1:-1]:
        if x != 0 and seats[x - 1] == 0 and seats[x + 1] == 0:
            result = x
            break

    utils.validate_result(title, result, correctResult)


def solve():
    print("---- Day 5: Binary Boarding ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_list(5, "example1"),
    #     820)
    run_part1("Problem",
        utils.read_input_as_list(5, "input"),
        970)

    print("---- part 2 ----")

    run_part2("Problem",
        utils.read_input_as_list(5, "input"),
        587)


if __name__ == "__main__":
    solve()