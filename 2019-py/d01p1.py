import math
from inputData import get_input_d1


def calc_fuel(mass):
    fuel = math.floor(mass / 3) - 2
    return fuel


def Run():
    print("Day 1 Puzzle 1")
    print("------------------------------------------------------------")
    print()

    totalFuel = 0
    input = get_input_d1()
    for x in input:
        totalFuel = totalFuel + calc_fuel(x)

    print(f"Total Fuel needed: {totalFuel}")


if __name__ == "__main__":
    # execute only if run as a script
    Run()