import math
import utils

class BusDepatureInfo:
    busId: int = 0
    departureTime: int = 0

    def __init__(self, busId: int, departureTime: int):
        self.busId = busId
        self.departureTime = departureTime

#-------------------------------------------------------------------------------

def get_next_time(currentTime: int, busId: int) -> int:
    while currentTime % busId != 0:
        currentTime += 1
    return currentTime

def get_soonest_bus(earliestTime: int, buses: list[str]) -> BusDepatureInfo:
    nextDepartures: list(BusDepatureInfo) = []

    for x in buses:
        if x == "x":
            continue
        busId = int(x)
        time = get_next_time(earliestTime, busId)
        nextDepartures.append(BusDepatureInfo(busId, time))

    result = BusDepatureInfo(-1, 0)
    for z in nextDepartures:
        if z.departureTime < result.departureTime or result.busId < 0:
            result = z

    return result

def get_lcm(a: int, b: int) -> int:
    """Gets the Least Common Multiple of two numbers"""
    return abs(a * b) // math.gcd(a, b)


def get_next_matching_time(time: int, busId: int, increment: int, offset: int) -> int:
    time += increment
    while (time + offset) % busId != 0:
        time += increment
    return time


def run_part1(title: str, input: list[str], correctResult: int):
    earliestTime = int(input[0])
    buses = input[1].split(",")
    bus = get_soonest_bus(earliestTime, buses)
    result = (bus.departureTime - earliestTime) * bus.busId
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: list[str], correctResult: int):
    buses = input[1].split(",")
    increment = 0
    lcmValue = 1
    time = 0
    i = 0
    while i < len(buses):
        if buses[i] != "x":
            busId = int(buses[i])
            if increment == 0:
                increment = 1
            time = get_next_matching_time(time, busId, increment, i)
            lcmValue = get_lcm(lcmValue, busId)
            increment = lcmValue
        i += 1
    utils.validate_result(title, time, correctResult)


if __name__ == "__main__":
    day = 13
    print(f"---- Day {day}: Title ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     295)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        410)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     1068781)
    # run_part2("Test Case 2",
    #     utils.read_input_as_list(day, "example2"),
    #     3417)
    # run_part2("Test Case 3",
    #     utils.read_input_as_list(day, "example3"),
    #     754018)
    # run_part2("Test Case 4",
    #     utils.read_input_as_list(day, "example4"),
    #     779210)
    # run_part2("Test Case 5",
    #     utils.read_input_as_list(day, "example5"),
    #     1261476)
    # run_part2("Test Case 6",
    #     utils.read_input_as_list(day, "example6"),
    #     1202161486)
    run_part2("problem",
        utils.read_input_as_list(day, "input"),
        600691418730595)