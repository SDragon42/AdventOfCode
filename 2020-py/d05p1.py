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
    
    # print(f"{boardingPass}: row {row}, column {col}, seat ID {seatId}")
    
    return seatId


def run(title: str, input: list[str], correctResult: int):
    result = 0
    for bPass in input:
        seatId = get_seat_id(bPass)
        if seatId > result:
            result = seatId
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(5, 1, "Binary Boarding")

    run("Test Case #",
        utils.read_input_as_list("day05-example1"),
        820)

    run("Problem",
        utils.read_input_as_list("day05"),
        970)