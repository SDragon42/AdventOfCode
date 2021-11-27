import re
from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int
    checkValues: bool

    def __init__(self, name: str, part: int) -> None:
        day = 4
        self.input = inputHelper.load_input_file(day, name)

        self.checkValues = True if part == 2 else False
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



class KeyValuePair:
    key: str
    value: str

    def __init__(self, key: str, value: str) -> None:
        self.key = key
        self.value = value



class Puzzle(PuzzleBase):
    _eyeColors = ["amb","blu","brn","gry","grn","hzl","oth"]

    def validate_birth_year(self, value):
        return 1920 <= int(value) <= 2002


    def validate_issue_year(self, value):
        return 2010 <= int(value) <= 2020


    def validate_expiration_year(self, value):
        return 2020 <= int(value) <= 2030


    def validate_height(self, value):
        isMatch = re.match("^[0-9]+cm$", value)
        if isMatch:
            height = int(isMatch.string[:isMatch.endpos-2])
            return 150 <= height <= 193
        
        isMatch = re.match("^[0-9]+in$", value)
        if isMatch:
            height = int(isMatch.string[:isMatch.endpos-2])
            return 59 <= height <= 76
        return False


    def validate_hair_color(self, value):
        isMatch = re.match("^[#]{1}[0-9a-f]{6}$", value)
        return isMatch is not None


    def validate_eye_color(self, value):
        return value in self._eyeColors


    def validate_passport_id(self, value):
        isMatch = re.match("^[0-9]{9}$", value)
        return isMatch is not None

    
    passportFieldDict = {
        "byr": validate_birth_year,
        "iyr": validate_issue_year,
        "eyr": validate_expiration_year,
        "hgt": validate_height,
        "hcl": validate_hair_color,
        "ecl": validate_eye_color,
        "pid": validate_passport_id,
    }


    def get_passort_key_values(self, lines: List[str]) -> List[KeyValuePair]:
        keyValues: List[KeyValuePair] = []

        for l in lines:
            parts = l.split()
            for p in parts:
                aaa = p.split(":")
                item = KeyValuePair(aaa[0], aaa[1])
                keyValues.append(item)

        return keyValues


    def has_required_fields(self, keyValues: List[KeyValuePair]) -> bool:
        for key in self.passportFieldDict:
            notFound = True

            for kv in keyValues:
                if kv.key == key:
                    notFound = False
                    break
            
            if notFound == True:
                return False

        return True


    def validate_passort_fields(self, keyValues: List[KeyValuePair]) -> bool:
        try:
            result = True
            for kv in keyValues:
                if kv.key in self.passportFieldDict:
                    result &= self.passportFieldDict[kv.key](self, kv.value)
            return result
        except:
            return False


    def count_valid_passorts(self, input: List[str], checkValues: bool) -> int:
        passportCount = 0

        ppStart = 0
        ppEnd = 0

        while ppStart < len(input):
            try:
                ppEnd = input.index("", ppStart)
            except ValueError:
                ppEnd = len(input)

            lines = input[ppStart:ppEnd]    

            keyValues = self.get_passort_key_values(lines)

            isValid = self.has_required_fields(keyValues)
            if checkValues:
                isValid &= self.validate_passort_fields(keyValues)

            if isValid:        
                passportCount += 1

            ppStart = ppEnd + 1

        return passportCount


    def run_part(self, data: InputData) -> str:
        result = self.count_valid_passorts(data.input, data.checkValues)
        return helper.validate_result('How many passports are valid?', result, data.expectedAnswer)


    def solve(self):
        print("Day 4: Passport Processing")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex2) " + self.run_part(InputData('example-invalid-passports', 2)))
        self.run_example(lambda: "P2 Ex3) " + self.run_part(InputData('example-valid-passports', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part(InputData('input', 2)))