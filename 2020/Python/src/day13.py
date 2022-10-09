from typing import List

import helper



class BusDepatureInfo:
    busId: int
    departureTime: int

    def __init__(self, busId: int, departureTime: int):
        self.busId = busId
        self.departureTime = departureTime



def get_next_time(currentTime: int, busId: int) -> int:
    while currentTime % busId != 0:
        currentTime += 1
    return currentTime


def get_soonest_bus(earliestTime: int, buses: List[str]) -> BusDepatureInfo:
    nextDepartures: List[BusDepatureInfo] = []

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


def get_next_matching_time(time: int, busId: int, increment: int, offset: int) -> int:
    time += increment
    while (time + offset) % busId != 0:
        time += increment
    return time


def run_part1(input: List[str]) -> int:
    earliestTime = int(input[0])
    buses = input[1].split(",")
    bus = get_soonest_bus(earliestTime, buses)
    result = (bus.departureTime - earliestTime) * bus.busId
    return result


def run_part2(input: List[str]) -> int:
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
            lcmValue = helper.get_lcm(lcmValue, busId)
            increment = lcmValue
        i += 1
    return time
