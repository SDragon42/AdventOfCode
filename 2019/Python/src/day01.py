from typing import List


def calc_fuel_for_mass(mass:int) -> int:
    """Calculates the amount of fuel needed for a given mass."""
    fuel = (mass // 3) - 2
    return fuel

def calc_total_fuel_for_mass(mass:int) -> int:
    """Calculates the total amount of fuel needed for a given mass, including its needed fuel mass."""
    fuel = (mass // 3) - 2
    if fuel <= 0:
        return 0
    return fuel + calc_total_fuel_for_mass(fuel)


def part1(massList:List[int]) -> int:
    totalFuel = sum([calc_fuel_for_mass(mass) for mass in massList])
    return totalFuel

def part2(massList:List[int]) -> int:
    totalFuel = sum([calc_total_fuel_for_mass(mass) for mass in massList])
    return totalFuel