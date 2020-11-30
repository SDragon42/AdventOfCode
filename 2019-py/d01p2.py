import math
from inputData import get_input_d01, get_answer_d01p2


def calc_fuel(mass):
    fuel = math.floor(mass / 3) - 2
    if (fuel < 0):
        return 0
    return fuel + calc_fuel(fuel)


if __name__ == "__main__":
    print("Day 1 Puzzle 2")
    print("------------------------------------------------------------")
    print()

    # input = [14] # test 1: required fule = 2
    # input = [1969] # test 2: required fule = 966
    # input = [100756] # test 3: required fule = 50346
    input = get_input_d01() # correct answer = 5057481

    totalFuel = 0
    for x in input:
        totalFuel = totalFuel + calc_fuel(x)

    print(f"Total Fuel needed: {totalFuel}")
    if get_answer_d01p2() == totalFuel:
        print("   CORRECT")