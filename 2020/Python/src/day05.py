from typing import List


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


def get_highest_seat_id(data: List[str]) -> int:
    result = 0
    for bPass in data:
        seatId = get_seat_id(bPass)
        if seatId > result:
            result = seatId
    return result


def get_your_seat_id(data: List[str]) -> int:
    seats = []
    for x in range(1024):
        seats.append(x)

    for bPass in data:
        seatId = get_seat_id(bPass)
        seats[seatId] = 0

    result = -1

    for x in seats[1:-1]:
        if x != 0 and seats[x - 1] == 0 and seats[x + 1] == 0:
            result = x
            break

    return result