import math
from . import testData
# from data import day01


print("Day 1 Puzzle 1")
print("------------------------------------------------------------")
print()


def Run():
    # doit(1,12)
    # doit(2,14)
    # doit(3,1969)
    # doit(4,100756)

    totalFuel = 0
    input = testData.get_input()
    for x in input:
        totalFuel = totalFuel + calc_fuel(x)

    print(f"Total Fuel needed: {totalFuel}")


def calc_fuel(mass):
    fuel = math.floor(mass / 3) - 2
    return fuel


def doit(exampleNum, mass):
    print(f"Example {exampleNum}")
    print(F"    mass = {mass}")
    fuel = calc_fuel(mass)
    print(f"    fuel = {fuel}")
    print()

