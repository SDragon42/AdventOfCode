import utils
import math
from typing import List, Dict

bitWidth: int = 36

memory: Dict[int, int] = {}
mask: str = "".rjust(bitWidth, "X")


def int_to_binary(value: int) -> List[str]:
    ba = List(bin(value)[2:].rjust(bitWidth, "0"))
    return ba


def binary_to_int(bValue: List[str]) -> int:
    value = int("".join(bValue), 2)
    return value


def set_mask(line: str):
    global mask
    mask = line[7:]


def apply_mask_v1(bValue: List[str]):
    global mask
    i = 0
    while i < len(mask):
        if mask[i] != "X":
            bValue[i] = mask[i]
        i += 1


def set_memory_v1(line: str):
    global mask
    global memory
    x = line.index("]")
    address = int(line[4:x])
    x += 4
    value = int(line[x:])

    bValue = int_to_binary(value)
    apply_mask_v1(bValue)
    value = binary_to_int(bValue)

    memory[address] = value


def apply_mask_v2(bValue: List[str]):
    global mask
    i = 0
    while i < len(mask):
        if mask[i] != "0":
            bValue[i] = mask[i]
        i += 1


def flip_address_bits(bValue: List[str], replacements: str) -> List[str]:
    i = 0
    while i < len(bValue):
        if bValue[i] == "X":
            bValue[i] = replacements[0]
            replacements = replacements[1:]
        i += 1
    return bValue


def get_all_possible_addresses(bValue: List[str]) -> List[int]:
    global mask
    addresses: List[int] = []
    count = mask.count("X")
    num = int(math.pow(2, count))
    rangeNums = range(num)
    for r in rangeNums:
        br = bin(r)[2:].rjust(count, "0")
        newAddr = flip_address_bits(bValue.copy(), br)
        newAddr2 = binary_to_int(newAddr)
        addresses.append(newAddr2)
        pass
    return addresses


def set_memory_v2(line: str):
    global mask
    global memory
    x = line.index("]")
    address = int(line[4:x])
    x += 4
    value = int(line[x:])

    bAddress = int_to_binary(address)
    apply_mask_v2(bAddress)
    addresses = get_all_possible_addresses(bAddress)
    for addr in addresses:
        memory[addr] = value


def run_part1(title: str, input: List[str], correctResult: int):
    global memory
    memory.clear()

    actions = {
        "mas" : set_mask,
        "mem" : set_memory_v1
    }

    for line in input:
        key = line[:3]
        actions[key](line)

    result = 0
    for key in memory:
        result += memory[key]
        pass

    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: List[str], correctResult: int):
    global memory
    memory.clear()

    actions = {
        "mas" : set_mask,
        "mem" : set_memory_v2
    }

    for line in input:
        key = line[:3]
        actions[key](line)

    result = 0
    for key in memory:
        result += memory[key]
        pass

    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    day = 14
    print(f"---- Day {day}: Docking Data ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     165)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        11327140210986)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example2"),
    #     208)
    run_part2("problem",
        utils.read_input_as_list(day, "input"),
        2308180581795)