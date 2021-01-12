import utils
import re
from typing import List, Dict, Any, Optional

# huge refernece from
# https://topaz.github.io/paste/#XQAAAQAnCgAAAAAAAAA0m0pnuFI8c/fBNApcL1Y57OO7A++6puZ0JXJD/kBA29C/HOta3P9p+BHeTV45JG3dakoaAXufTQCuMSCrM908JfdUCKMTdVciHZCGEpf5Yk1dAAXBwmZE2X0cXQQJmUmI4FG+Ph+D+nIfhnkYdBHCPSAEc8elDbDk4dR8lkza37FF7vA+/sm+M77GjNUTTAB7enRY8imtootH1tuhW69nHowJZ2J7sv0euurJJaDZPbUvvPqiDb3RQuvuiJxN88MaPHiO+pvp6lMzsO0QDz5wWNi2cUDTLRiS/k8EZ80afTVkaH2E6vtroX05RQyjc7h1Yf3eFf4f8v2BWKCfH9iLgteU0yWih8xIUXfqOjYwS0AlQId5EH/xWTn01Wn/3HdWvUrVuUtI8e9RJI1NCVVCbS03ZAKiQzsi3YZxxCj7d8oN2hMqmIjYKBiycNyxSemSR8lCldMGidpd8KRPp3pfW07Jq5zKRfcqUV50tCU8NH1G4/fbcOY4UHQli40PwY031Vb8v0sp1CNxC4EAdX8Wyw07qFdIUJNkD3POItH6pPhsG3r3CTAart7LERoepfwsZeLyyzBlv7N+ZESGETJvGEZZAJSR0KgL+u22Ys11SsUQTkbC7hfE4t/E6xsGcHkBASNtV9C2gm+WZwibxX+5DPnVkIESAmhLw4QtAgPsGKaFe5l020QI4NLrx4G/HujXIoF9pOamxrAErfxWNJfOpp1+aZZ1yfjdRDT7NhQEBfE2iX+jyLcFzSGZ9L5lqe7tDnsAW2SHsIs9ZTcPzDxsB+vtOXcDQh+toIod7cURf1oEAAC0HRj/qbRyYd5/lzHGrO35IYbqLRKTtlgPDbQOPllY0Qbxw6bRNBTG9LFwk8nh0bMd4XzpBHcWN6D1k8q/6Vu24zT1A3sY6eJldPzvdfsXdujOLt2EvZ9XH+akwcZN6Yls+D1ZzCHF2Jt0mnoc59UvZ33J056fo+Fio9/uza77aU93MxvmAvDB+xyXuNoJDWLSHyQeBjRthIYZ6d9RL5+vzI1aq2wrdVp5C76vxoB79LSS2EFBV1KNxSfRFddXbtziYOFfujOq6SmrsEZi5pTRyG88Wrc/yfiiA7LLVWr3l3XV7tvejXNxQJ2731LjBRGGseQcY2s5iIFgF1fBJEzOSdvg9TjxJfjHwmskQmILnHK3ZLkqzc2ItdWgTD23FDYuy3eODWXcUK5x9Ul4l15x6v2YeF4lVm0N/8I0ueM=

RuleChains = List[List[Any]]

def build_rule_dict(input: List[str]) -> Dict[int, RuleChains]:
    rules: Dict[int, RuleChains] = {}

    for line in input:
        i = line.index(":")
        key = int(line[:i].strip())
        line = line[i+1:]

        orParts = line.split("|")
        orChain: RuleChains = []
        for orLine in orParts:
            parts = orLine.split()
            andChain = []
            for p in parts:
                val = p.strip().replace('"', '')
                try:
                    andChain.append(int(val))
                except:
                    andChain.append(val)
            orChain.append(andChain)
        rules[key] = orChain

    return rules


def convert_to_regex_rules(rules: Dict[int, RuleChains]) -> Dict[int, RuleChains]:
    while True:
        key = get_str_only_rule(rules)
        if not key:
            break

        rule = rules.pop(key)
        regex = convert_to_regex(rule)
        rules = replace_references(rules, key, regex)

    return rules


def rule_is_all_str(rule: RuleChains) -> bool:
    xx = [val for orGroup in rule for val in orGroup]
    for check in xx:
        if not isinstance(check, str):
            return False
    return True


def get_str_only_rule(rules: Dict[int, RuleChains]) -> Optional[int]:
    for key, rule in rules.items():
        if rule_is_all_str(rule):
            return key
    return None


def convert_to_regex(rule: RuleChains) -> str:
    # ref1 =  r'(' + '|'.join(['('+ ''.join([c for cs in group for c in cs])+ ')' for group in rule]) + ')'

    parts = []
    for group in rule:
        subParts = []
        for cs in group:
            for c in cs:
                subParts.append(c)
        a = "(" + "".join(subParts) + ")"
        parts.append(a)
    ref2 = "(" + "|".join(parts) + ")"

    return ref2


def replace_references(rules: Dict[int, RuleChains], replace_key: int, regex: str) -> Dict[int, RuleChains]:
    for key, rule in rules.items():
        for gIdx, g in enumerate(rule):
            for cIdx, c in enumerate(g):
                if c == replace_key:
                    rules[key][gIdx][cIdx] = regex
    return rules


def process_image(imageData: str, rules: Dict[str, RuleChains], charIdx: int, ruleKey: str) -> int:
    chain: RuleChains = rules[ruleKey]
    # utils.dprint(imageData[charIdx:])
    # utils.dprint(f"rule: {ruleKey}  -  {chain}")
    for orChain in chain:
        # utils.dprint(f"    {orChain}")
        successChars = 0
        for x in orChain:
            if charIdx + successChars >= len(imageData):
                break

            if x == "a" or x == "b":
                if x == imageData[charIdx + successChars]:
                    # utils.dprint("    MATCH")
                    # utils.dprint("")
                    return 1
                else:
                    return 0
                    
            shift = process_image(imageData, rules, charIdx + successChars, x)
            successChars += shift
            if shift <= 0:
                successChars = 0
                break
        
        if successChars > 0:
            return successChars
    return 0

def count_valid_images(messages: List[str], regex: str):
    regex += r'$' # Include line-end in regex to match whole line
    p = re.compile(regex)
    count = 0
    for message in messages:
        match = p.match(message)
        if match:
            count += 1
            utils.dprint(message)
    return count

def run_part1(title: str, input: List[str], correctResult: int):
    end = input.index("")
    rules = build_rule_dict(input[:end])
    rules = convert_to_regex_rules(rules)

    images = input[end+1:]

    regex = convert_to_regex(rules[0])
    result = count_valid_images(images, regex)
    # result = 0
    # for imageData in images:
    #     # charIdx = 0
    #     # utils.dprint(f"IMG: {imageData}")
    #     charsMatched = process_image(imageData, rules, 0, "0") 
    #     if charsMatched == len(imageData):
    #         result += 1
    #         utils.dprint(f"IMG: {imageData}")
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: List[str], correctResult: int):
    end = input.index("")
    rules = build_rule_dict(input[:end])

    rules[8] = [[42], [42, 8]]
    rules[11] = [[42, 31], [42, 11, 31]]

    rules = convert_to_regex_rules(rules)

    rx42 = rules[8][0][0]
    rx31 = rules[11][0][1]
    rx8 = rx42 + "+"
    parts = []
    for n in range(1, 6):
        val = f"{rx42}{{{n}}}{rx31}{{{n}}}"
        parts.append(val)
    rx11 = "(" + "|".join(parts) + ")"
    regex = rx8 + rx11

    images = input[end+1:]

    result = count_valid_images(images, regex)

    # result = 0
    # for imageData in images:
    #     # charIdx = 0
    #     # utils.dprint(f"IMG: {imageData}")
    #     if imageData == "aaaabbaaaabbaaa":
    #         print("---------")
    #         pass
    #     charsMatched = process_image(imageData, rules, 0, "0") 
    #     if charsMatched == len(imageData):
    #         result += 1
    #         utils.dprint(f"IMG: {imageData}")
    utils.validate_result(title, result, correctResult)


# utils.showDebug = True
if __name__ == "__main__":
    day = 19
    print(f"---- Day {day}: Monster Messages ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     2)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        222)

    print("---- part 2 ----")

    # run_part1("Test Case 2 - Pre rule change",
    #     utils.read_input_as_list(day, "example2"),
    #     3)
    # run_part2("Test Case 2 - Post rule change",
    #     utils.read_input_as_list(day, "example2"),
    #     12)
    run_part2("problem",
        utils.read_input_as_list(day, "input"),
        339)