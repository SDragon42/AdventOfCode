import utils


def get_row(lowCode: str, highCode: str, lowStart: int, highStart: int, input: str) -> int:
    low = lowStart
    high = highStart
    for x in input:
        if x == highCode:
            high -= (high - low + 1) / 2
        elif x == lowCode:
            low += (high - low + 1) / 2
    return int(low - 1)


def get_seat_id(boardingPass: str) -> int:
    rowCode = boardingPass[:7]
    colCode = boardingPass[7:]

    row = get_row("B", "F", 1, 128, rowCode)
    col = get_row("R", "L", 1, 8, colCode)
    seatId = (row * 8) + col
    
    return seatId


def run(title: str, input: list[str], correctResult: int):
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
    print()


if __name__ == "__main__":
    utils.show_title(5, 2, "Binary Boarding")

    run("Problem",
        utils.read_input_as_list("day05"),
        587)