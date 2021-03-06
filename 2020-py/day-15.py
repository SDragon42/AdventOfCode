import utils
from typing import List, Dict

class LastSaid:
    value: str = ""
    onTurn: int = 0
    prevTurn: int = 0


def run(title: str, input: List[str], lastTurn: int, correctResult: int):
    history: Dict[str, int] = {}
    turn = 0
    ls = LastSaid()
    initialSeq = input[0].split(",")
    for x in initialSeq:
        turn += 1
        history[x] = turn
        ls.value = x
        ls.onTurn = turn
    
    while turn < lastTurn:
        turn += 1
        
        if ls.prevTurn == 0:
            ls.value = "0"
            ls.onTurn = turn
        else:
            diff = ls.onTurn - ls.prevTurn
            ls.value = str(diff)
            ls.onTurn = turn

        if ls.value in history:
            ls.prevTurn = history[ls.value]
        else:
            ls.prevTurn = 0

        history[ls.value] = turn

    result = int(ls.value)
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    day = 15
    print(f"---- Day {day}: Rambunctious Recitation ----")

    # run("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     10,
    #     0)
    run("problem",
        utils.read_input_as_list(day, "input"),
        2020,
        276)

    print("---- part 2 ----")

    # run("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     30000000,
    #     175594)
    run("problem",
        utils.read_input_as_list(day, "input"),
        30000000,
        31916)