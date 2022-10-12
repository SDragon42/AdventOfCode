# Assistence from:
# https://topaz.github.io/paste/#XQAAAQBmBwAAAAAAAAA0m0pnuFI8c/fBNApcL1pmU7gQJ13v1+YhguIn3pR0zOaQ4v8vj0T0GZD91/Qbo2hM5nCqaFlDw+OBPRSF3ewxIy5NZrOqQbD9Kh7oFNwchmD9695r4vyj+mz1c4oXVWCGYHHwJ2VwiL3QE7OrbNzmf2w9+mDd+zLHFeykh3A6dHonmdmCrpyWv9HWuYGtqYmaTmo0XPFtAbhipfsM5QpMgmJca3QZDAyJ2VutU4lqC1zCBnqJPh81yJaW7+5A12miU2B58CiK3YmwFi5dt6gYyutT/6sW6nPhSpIu+04o47ZoEuRvPF+Sbc7OptvVIJAgPtQRSU7Urjnb3GxLnj61pTk3oc7o5G97Qx85tpA0/TA9LYq6QCARuEKSUn/kJJMujuDHGGRdjEU85SY5afeqaKlIGStnDCp26i4n0iX2z65LF8QUVap9GUamNLeelfLlqcXCKselRZJeC5eiDfWHe30MG/DuZ35NSh3Ui7qK2R5SXVIcz5ofmhI11FLhgCGdVvJ2CyHl/TEmEzTdZWK90OVTZkzygCuQf608vj3GpQeocemQvGNT98YWNdVwG+49RlCSluol1A8lkBd/mYu416rsFVQK+9TtOiummhnxTLo2SGEbgSdL0x0cEGq9cuka6BF2Sp1kyHTWPoQ3H3StqUB5y6jAC8h7oXJZqcseoUvhZUsVUgj9FQRcSASOvoxqgP+4urD4d1QeiStRA3qHB/nJBUcDkP7c7LkrJvYVz0jOUJiTPHDLbwob4ZxBtEj+/yok0WRApn6Txw9m7NBcVS2q/AqiItbrLa/qi01kFFsPqi2CesKmBkK95zUXYulQ3t95nY/y+xWiu+i6VBVZYNsUf/aAlTA8U2t8u95Mr2ssn6H0aRP2x2FAjG8wXz/4a2VfgI7sYWbLBRo5Mt/Z32LGpDWm/FR58ow/E6Ms/H7NklhKu4wm49uBjXGzXStI5404a0Tj1To1dLa9SQiFSdzxYGysBOJl7NuwCUDhayr86aIFcM46MntjuYy/6teQ0ea/ZmTmgapT+Mt3ZNgTJuCFUuwPGfE2hMQ3Sl1M11rDCocsaVFjpYUcGLP9TF//+h1+DFbV2vRMx4pSjl1JiaotagEb+iNVDgfDB+Or61U8Bp9hKk0Yhx3+1CuRTPuxynwKydwZ2u7cw9tJYluBIVJGQ1qg9qwiMfVfJVyQLv/8oErq

from typing import List, Dict



class CrabCups:
    cupMinMax: tuple[int, int] = (0,0)
    cupLinks: Dict[int, int] = {}
    current: int = 0
    totalMoves: int = 0
    move: int = 0


    def __init__(self, cupLinks: Dict[int, int], head: int, numMoves: int) -> None:
        self.cupLinks = cupLinks
        self.current = head
        self.totalMoves = numMoves
        self.move = 0

        keys = [x for x in cupLinks.keys()]
        self.cupMinMax = (min(keys), max(keys))


    def perform_move(self) -> bool:
        self.move += 1
        if self.move > self.totalMoves:
            return False

        liftedCuplist = self.take_cups(3)
        targetCup = self.get_target_cup(liftedCuplist)
        self.add_cups_at(liftedCuplist, targetCup)
        self.set_next_current()
        return True


    def take_cups(self, numCups: int) -> List[int]:
        liftedCuplist = []
        tmp = self.current
        i = 0
        while i < numCups:
            tmp = self.cupLinks[tmp]
            liftedCuplist.append(tmp)
            i += 1

        self.cupLinks[self.current] = self.cupLinks[tmp]
        self.cupLinks[tmp] = -1
        return liftedCuplist


    def get_target_cup(self, excludedCupsList: List[int]) -> int:
        targetCup = self.current
        while True:
            targetCup -= 1
            if targetCup < self.cupMinMax[0]:
                targetCup = self.cupMinMax[1]
            if targetCup in excludedCupsList:
                continue
            break
        return targetCup


    def add_cups_at(self, toAdd: List[int], afterCup: int):
        tmp = self.cupLinks[afterCup]
        self.cupLinks[afterCup] = toAdd[0]
        self.cupLinks[toAdd[len(toAdd) - 1]] = tmp


    def set_next_current(self):
        self.current = self.cupLinks[self.current]


    def get_cups_after_1(self) -> int:
        result = ""
        tmp = self.cupLinks[1]
        while tmp != 1:
            result += str(tmp)
            tmp = self.cupLinks[tmp]
        return int(result)



    def get_cups_after_1b(self) -> tuple[int, int]:
        tmp = self.cupLinks[1]
        return (tmp, self.cupLinks[tmp])



def int_list_to_dict_links(values: List[int]) -> Dict[int, int]:
    links = {number: values[(idx + 1) % len(values)] for idx, number in enumerate(values)}
    return links


def run_part1(input: List[int], numMoves: int) -> int:
    cupLinks = int_list_to_dict_links(input)

    game = CrabCups(cupLinks, input[0], numMoves)
    while game.perform_move():
        pass
    result = game.get_cups_after_1()
    return result


def run_part2(input: List[int], numMoves: int) -> int:
    tmp = range(max(input) + 1, 1000001, 1)
    input.extend(tmp)
    cupLinks = int_list_to_dict_links(input)

    game = CrabCups(cupLinks, input[0], numMoves)
    while game.perform_move():
        pass
    values = game.get_cups_after_1b()
    result = values[0] * values[1]
    return result
