import utils


def get_jolt_differences(adapters: list[int]) -> dict[int, int]:
    diffDict: dict[int, int] = {}
    adapters.append(max(adapters) + 3)
    last = 0
    for a in adapters:
        diff = a - last
        if diff in diffDict:
            diffDict[diff] = diffDict[diff] + 1
        else:
            diffDict[diff] = 1
        last = a
    return diffDict


def get_next_possible_adapters(current: int, adapters: list[int]) -> list[int]:
    result: list[int] = []
    for x in adapters:
        if x > current and x <= current + 3:
            result.append(x)
    return result


def count_adapterChains(current:int, adapters: list[int]) -> int:
    if current in chainsUnder:
        return chainsUnder[current]

    nextAdapters = get_next_possible_adapters(current, adapters)
    if len(nextAdapters) == 0:
        return 1

    result = 0
    for x in nextAdapters:
        result += count_adapterChains(x, adapters)
    chainsUnder[current] = result
    return result


def run_part1(title: str, adapters: list[str], correctResult: int):
    adapters = sorted(adapters)
    diffCounts = get_jolt_differences(adapters)
    result = diffCounts[1] * diffCounts[3]
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, adapters: list[str], correctResult: int):
    chainsUnder.clear()
    adapters = sorted(adapters)
    result = count_adapterChains(0, adapters)
    utils.validate_result(title, result, correctResult)


chainsUnder: dict[int, int] = {}
if __name__ == "__main__":
    print("---- Day 10: Adapter Array ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_int_list(10, "example1"),
    #     35)
    # run_part1("Test Case 2",
    #     utils.read_input_as_int_list(10, "example2"),
    #     220)
    run_part1("problem",
        utils.read_input_as_int_list(10, "input"),
        2414)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_int_list(10, "example1"),
    #     8)
    # run_part2("Test Case 2",
    #     utils.read_input_as_int_list(10, "example2"),
    #     19208)
    run_part2("problem",
        utils.read_input_as_int_list(10, "input"),
        21156911906816)