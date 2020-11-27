import math
from inputData import get_input_d1


def calc_fuel(mass):
    fuel = math.floor(mass / 3) - 2
    return fuel


if __name__ == "__main__":
    print("Day 1 Puzzle 1")
    print("------------------------------------------------------------")
    print()

    totalFuel = 0

    # input = [12] # test 1: required fule = 2
    # input = [14] # test 2: required fule = 2
    # input = [1969] # test 2: required fule = 654
    # input = [100756] # test 2: required fule = 33583
    input = get_input_d1() # correct answer = 3373568
    
    for x in input:
        totalFuel = totalFuel + calc_fuel(x)

    print(f"Total Fuel needed: {totalFuel}")