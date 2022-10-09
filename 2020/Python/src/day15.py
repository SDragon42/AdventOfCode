from typing import List, Dict



class LastSaid:
    value: str
    onTurn: int
    prevTurn: int

    def __init__(self) -> None:
        self.value = ''
        self.onTurn = 0
        self.prevTurn = 0



def run_part(input: List[str], lastTurn: int) -> str:
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
    return result
