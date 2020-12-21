import utils

RuleChains = list[list[str]]

def build_rule_dict(input: list[str]) -> dict[str, RuleChains]:
    rules: dict[str, RuleChains] = {}

    for line in input:
        i = line.index(":")
        key = line[:i].strip()
        line = line[i+1:]

        orParts = line.split("|")
        orChain: RuleChains = []
        for orLine in orParts:
            parts = orLine.split()
            andChain: list[str] = []
            for p in parts:
                val = p.strip().replace('"', '')
                andChain.append(val)
            orChain.append(andChain)
        rules[key] = orChain

    return rules


def process_image(imageData: str, rules: dict[str, RuleChains], charIdx: int, ruleKey: str) -> int:
    chain: RuleChains = rules[ruleKey]
    utils.dprint(imageData[charIdx:])
    utils.dprint(f"rule: {ruleKey}  -  {chain}")
    for orChain in chain:
        utils.dprint(f"    {orChain}")
        successChars = 0
        for x in orChain:
            if x == "a" or x == "b":
                if x == imageData[charIdx + successChars]:
                    utils.dprint("    MATCH")
                    utils.dprint("")
                    return 1
                else:
                    return 0
            else:
                shift = process_image(imageData, rules, charIdx + successChars, x)
                successChars += shift
                if shift <= 0:
                    successChars = 0
                    break
        
        if successChars > 0:
            return successChars
    return 0


def run_part1(title: str, input: list[str], correctResult: int):
    end = input.index("")
    rules = build_rule_dict(input[:end])
    images = input[end+1:]
    result = 0
    for imageData in images:
        # charIdx = 0
        utils.dprint(f"IMG: {imageData}")
        charsMatched = process_image(imageData, rules, 0, "0") 
        if charsMatched == len(imageData):
            result += 1
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: list[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)


# utils.showDebug = True
if __name__ == "__main__":
    day = 19
    print(f"---- Day {day}: Monster Messages ----")

    run_part1("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        2)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        222)

    # print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     0)
    # run_part2("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)