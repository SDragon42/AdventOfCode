import utils


class CrabCups:

    cupList: list[int] = []
    current: int = -1
    totalMoves: int = 0
    move: int = 0

    def __init__(self, cupList: list[int], numMoves: int) -> None:
        self.cupList = cupList
        self.current = self.cupList[0]
        self.totalMoves = numMoves
        self.move = 0

    def perform_move(self) -> bool:
        self.move += 1
        if self.move > self.totalMoves:
            return False

        liftedCupList = self.take_cups(3)
        targetCup = self.get_target_cup()
        self.add_cups_at(liftedCupList, targetCup)
        self.set_next_current()

        return True

    def take_cups(self, numCups: int) -> list[int]:
        liftedCupList: list[int] = []
        startIdx = self.cupList.index(self.current) + 1
        while numCups > 0 and startIdx < len(self.cupList):
            liftedCupList.append(self.cupList.pop(startIdx))
            numCups -= 1
        while numCups > 0:
            liftedCupList.append(self.cupList.pop(0))
            numCups -= 1

        return liftedCupList

    def get_target_cup(self) -> int:
        targetCup = self.current
        while True:
            targetCup -= 1
            if targetCup < min(self.cupList):
                targetCup = max(self.cupList)
            if targetCup in self.cupList:
                break
        return targetCup

    def add_cups_at(self, toAdd: list[int], afterCup: int):
        idx = self.cupList.index(afterCup) + 1
        for x in toAdd:
            self.cupList.insert(idx, x)
            idx += 1

    def set_next_current(self):
        idx = self.cupList.index(self.current) + 1
        if idx >= len(self.cupList):
            idx = 0
        self.current = self.cupList[idx]

    def get_cups_after_1(self) -> str:
        idx = self.cupList.index(1)
        tmp = self.cupList[idx + 1:].copy()
        tmp.extend(self.cupList[:idx])
        result = "".join(str(x) for x in tmp)
        return result

    def get_cups_after_1b(self) -> list[int]:
        idx = self.cupList.index(1)
        tmp = self.cupList[idx + 1:2].copy()
        return tmp

#-------------------------------------------------------------------------------

def str_to_int_list(input: str) -> list[int]:
    return [int(x) for x in input]


def run_part1(title: str, input: str, numMoves: int, correctResult: str):
    cupList = str_to_int_list(input)
    game = CrabCups(cupList, numMoves)
    while game.perform_move():
        pass
    result = game.get_cups_after_1()
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: str, numMoves: int, correctResult: str):
    cupList = str_to_int_list(input)

    tmp = range(max(cupList) + 1, 1000001, 1)
    cupList.extend(tmp)

    game = CrabCups(cupList, numMoves)
    while game.perform_move():
        pass
    result = game.get_cups_after_1b()
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    day = 23
    print(f"---- Day {day}: Crab Cups ----")

    run_part1("Test Case 1 (10 moves)",
        "389125467",
        10,
        "92658374")
    run_part1("Test Case 1 (10 moves)",
        "389125467",
        100,
        "67384529")
    run_part1("problem",
        "643719258",
        100,
        "54896723")

    # print("---- part 2 ----")

    # run_part2("Test Case 1 (10,000,000 moves)",
    #     "389125467",
    #     # 10000000,
    #     10,
    #     "92658374")
    # run_part2("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)