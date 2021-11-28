import re
from typing import List, Dict, Any, Optional

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 19
        self.input = inputHelper.load_input_file(day, name)
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



RuleChains = List[List[Any]]



# huge refernece from
# https://topaz.github.io/paste/#XQAAAQAnCgAAAAAAAAA0m0pnuFI8c/fBNApcL1Y57OO7A++6puZ0JXJD/kBA29C/HOta3P9p+BHeTV45JG3dakoaAXufTQCuMSCrM908JfdUCKMTdVciHZCGEpf5Yk1dAAXBwmZE2X0cXQQJmUmI4FG+Ph+D+nIfhnkYdBHCPSAEc8elDbDk4dR8lkza37FF7vA+/sm+M77GjNUTTAB7enRY8imtootH1tuhW69nHowJZ2J7sv0euurJJaDZPbUvvPqiDb3RQuvuiJxN88MaPHiO+pvp6lMzsO0QDz5wWNi2cUDTLRiS/k8EZ80afTVkaH2E6vtroX05RQyjc7h1Yf3eFf4f8v2BWKCfH9iLgteU0yWih8xIUXfqOjYwS0AlQId5EH/xWTn01Wn/3HdWvUrVuUtI8e9RJI1NCVVCbS03ZAKiQzsi3YZxxCj7d8oN2hMqmIjYKBiycNyxSemSR8lCldMGidpd8KRPp3pfW07Jq5zKRfcqUV50tCU8NH1G4/fbcOY4UHQli40PwY031Vb8v0sp1CNxC4EAdX8Wyw07qFdIUJNkD3POItH6pPhsG3r3CTAart7LERoepfwsZeLyyzBlv7N+ZESGETJvGEZZAJSR0KgL+u22Ys11SsUQTkbC7hfE4t/E6xsGcHkBASNtV9C2gm+WZwibxX+5DPnVkIESAmhLw4QtAgPsGKaFe5l020QI4NLrx4G/HujXIoF9pOamxrAErfxWNJfOpp1+aZZ1yfjdRDT7NhQEBfE2iX+jyLcFzSGZ9L5lqe7tDnsAW2SHsIs9ZTcPzDxsB+vtOXcDQh+toIod7cURf1oEAAC0HRj/qbRyYd5/lzHGrO35IYbqLRKTtlgPDbQOPllY0Qbxw6bRNBTG9LFwk8nh0bMd4XzpBHcWN6D1k8q/6Vu24zT1A3sY6eJldPzvdfsXdujOLt2EvZ9XH+akwcZN6Yls+D1ZzCHF2Jt0mnoc59UvZ33J056fo+Fio9/uza77aU93MxvmAvDB+xyXuNoJDWLSHyQeBjRthIYZ6d9RL5+vzI1aq2wrdVp5C76vxoB79LSS2EFBV1KNxSfRFddXbtziYOFfujOq6SmrsEZi5pTRyG88Wrc/yfiiA7LLVWr3l3XV7tvejXNxQJ2731LjBRGGseQcY2s5iIFgF1fBJEzOSdvg9TjxJfjHwmskQmILnHK3ZLkqzc2ItdWgTD23FDYuy3eODWXcUK5x9Ul4l15x6v2YeF4lVm0N/8I0ueM=
class Puzzle(PuzzleBase):
    
    def build_rule_dict(self, input: List[str]) -> Dict[int, RuleChains]:
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


    def convert_to_regex_rules(self, rules: Dict[int, RuleChains]) -> Dict[int, RuleChains]:
        while True:
            key = self.get_str_only_rule(rules)
            if not key:
                break
            rule = rules.pop(key)
            regex = self.convert_to_regex(rule)
            rules = self.replace_references(rules, key, regex)
        return rules


    def rule_is_all_str(self, rule: RuleChains) -> bool:
        xx = [val for orGroup in rule for val in orGroup]
        for check in xx:
            if not isinstance(check, str):
                return False
        return True


    def get_str_only_rule(self, rules: Dict[int, RuleChains]) -> Optional[int]:
        for key, rule in rules.items():
            if self.rule_is_all_str(rule):
                return key
        return None


    def convert_to_regex(self, rule: RuleChains) -> str:
        parts = []
        for group in rule:
            subParts = []
            for cs in group:
                for c in cs:
                    subParts.append(c)
            a = f"({''.join(subParts)})"
            parts.append(a)
        ref2 = f"({'|'.join(parts)})"

        return ref2


    def replace_references(self, rules: Dict[int, RuleChains], replace_key: int, regex: str) -> Dict[int, RuleChains]:
        for key, rule in rules.items():
            for gIdx, g in enumerate(rule):
                for cIdx, c in enumerate(g):
                    if c == replace_key:
                        rules[key][gIdx][cIdx] = regex
        return rules


    def process_image(self, imageData: str, rules: Dict[str, RuleChains], charIdx: int, ruleKey: str) -> int:
        chain: RuleChains = rules[ruleKey]
        for orChain in chain:
            successChars = 0
            for x in orChain:
                if charIdx + successChars >= len(imageData):
                    break

                if x == "a" or x == "b":
                    if x == imageData[charIdx + successChars]:
                        return 1
                    else:
                        return 0
                        
                shift = self.process_image(imageData, rules, charIdx + successChars, x)
                successChars += shift
                if shift <= 0:
                    successChars = 0
                    break
            
            if successChars > 0:
                return successChars
        return 0


    def count_valid_images(self, messages: List[str], regex: str):
        regex += r'$' # Include line-end in regex to match whole line
        p = re.compile(regex)
        count = 0
        for message in messages:
            match = p.match(message)
            if match:
                count += 1
                helper.dprint(message)
        return count


    def run_part1(self, data: InputData) -> str:
        end = data.input.index("")
        rules = self.build_rule_dict(data.input[:end])
        rules = self.convert_to_regex_rules(rules)

        images = data.input[end+1:]

        regex = self.convert_to_regex(rules[0])
        result = self.count_valid_images(images, regex)

        return helper.validate_result('How many messages completely match rule 0?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        end = data.input.index("")
        rules = self.build_rule_dict(data.input[:end])

        rules[8] = [[42], [42, 8]]
        rules[11] = [[42, 31], [42, 11, 31]]

        rules = self.convert_to_regex_rules(rules)

        rx42 = rules[8][0][0]
        rx31 = rules[11][0][1]
        rx8 = f"{rx42}+"
        parts = []
        for n in range(1, 6):
            val = f"{rx42}{{{n}}}{rx31}{{{n}}}"
            parts.append(val)
        rx11 = "(" + "|".join(parts) + ")"
        regex = rx8 + rx11

        images = data.input[end+1:]

        result = self.count_valid_images(images, regex)
        
        return helper.validate_result('After updating rules 8 and 11, how many messages completely match rule 0?', result, data.expectedAnswer)


    def solve(self):
        print("Day 19: Monster Messages")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example2', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))