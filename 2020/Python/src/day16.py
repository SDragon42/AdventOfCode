from typing import List, Dict

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 16
        self.input = inputHelper.load_file(day, name).splitlines()
        
        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class ValueRange:
    lowValue: int
    highValue: int

    def __init__(self, lowValue: int, highValue: int):
        self.lowValue = lowValue
        self.highValue = highValue

    def in_range(self, value: int) -> bool:
        return self.lowValue <= value <= self.highValue



class FieldInfo:
    field: str
    valueRanges: List[ValueRange]

    def __init__(self, field: str):
        self.field = field
        self.valueRanges = []

    def add_range(self, lowValue: int, highValue: int):
        for r in self.valueRanges:
            if r.lowValue == lowValue and r.highValue == highValue:
                return
        self.valueRanges.append(ValueRange(lowValue, highValue))

    def in_range(self, value: int) -> bool:
        for r in self.valueRanges:
            if r.in_range(value):
                return True
        return False



class Puzzle(PuzzleBase):
    fieldDefInputList: List[FieldInfo] = []
    myTicketValues: str = ""
    otherTicketValuesList: List[str] = []
    fieldOrderDict: Dict[int, List[str]] = {}

    #-------------------------------------------------------------------------------

    def parse_input_data(self, inputList: List[str]):
        endIdx = inputList.index("")
        self.parse_field_definitions(inputList[:endIdx])

        startIdx = endIdx + 2
        endIdx = inputList.index("", startIdx)
        self.myTicketValues = inputList[startIdx]

        self.otherTicketValuesList.clear()
        startIdx = endIdx + 2
        self.otherTicketValuesList = inputList[startIdx:]


    def get_field_info_record(self, field: str) -> FieldInfo:
        for f in self.fieldDefInputList:
            if f.field == field:
                return f
        rec = FieldInfo(field)
        self.fieldDefInputList.append(rec)
        return rec


    def parse_field_definitions(self, inputList: List[str]):
        self.fieldDefInputList.clear()

        for line in inputList:
            fieldDelim = line.index(":")
            field = line[:fieldDelim]

            fieldRecord = self.get_field_info_record(field)

            ranges = line[fieldDelim+1:].split(" or ")
            for r in ranges:
                parts = r.split("-")
                
                lowVal = int(parts[0])
                highVal = int(parts[1])

                fieldRecord.add_range(lowVal, highVal)


    def init_field_order_list(self):
        self.fieldOrderDict.clear()
        i = 0
        numValues = self.myTicketValues.count(",") + 1
        while i < numValues:
            fieldList: List[str] = []
            for f in self.fieldDefInputList:
                fieldList.append(f.field)
            self.fieldOrderDict[i] = fieldList
            i += 1


    #-------------------------------------------------------------------------------

    def get_ticket_values(self, ticket: str) -> List[int]:
        result: List[int] = []
        for value in ticket.split(","):
            result.append(int(value))
        return result


    def is_ticket_valid(self, ticket: str, invalidValues: List[int]) -> bool:
        valueList = self.get_ticket_values(ticket)
        result = True
        for value in valueList:
            isInvalid = True
            for field in self.fieldDefInputList:
                if field.in_range(value):
                    isInvalid = False
                    break
            if isInvalid:
                invalidValues.append(value)
                result = False
        return result


    def get_invalid_ticket_values(self, ticketValues: List[str]) -> List[int]:
        invalidValues: List[int] = []
        for ticket in ticketValues:
            self.is_ticket_valid(ticket, invalidValues)
        return invalidValues


    def discard_invalid_tickets(self):
        i = 0
        while i < len(self.otherTicketValuesList):
            ticket = self.otherTicketValuesList[i]
            if not self.is_ticket_valid(ticket, []):
                del self.otherTicketValuesList[i]
                continue
            i += 1


    def identified_all_fields(self) -> bool:
        for x in self.fieldOrderDict:
            if len(self.fieldOrderDict[x]) > 1:
                return False
        return True


    def identify_fields(self):
        self.identify_fields_with_ticket(self.myTicketValues)
        for ticket in self.otherTicketValuesList:
            self.identify_fields_with_ticket(ticket)
            if self.identified_all_fields():
                return


    def identify_fields_with_ticket(self, ticket: str):
        valueList = self.get_ticket_values(ticket)

        i = 0
        while i < len(valueList):
            for f in self.fieldDefInputList:
                if not f.in_range(valueList[i]):
                    fields = self.fieldOrderDict[i]
                    if f.field in fields:
                        fields.remove(f.field)
                    break
            i += 1
        
        # remove single field entries from other posibilities
        i = 0
        while i < len(self.fieldOrderDict):
            anyRemoved = False
            if len(self.fieldOrderDict[i]) == 1:
                field = self.fieldOrderDict[i][0]
                k = 0
                while k < len(self.fieldOrderDict):
                    if k != i:
                        if field in self.fieldOrderDict[k]:
                            self.fieldOrderDict[k].remove(field)
                            anyRemoved = True
                    k += 1
            if anyRemoved:
                i = 0
            else:
                i += 1


    def run_part1(self, data: InputData) -> str:
        self.parse_input_data(data.input)

        invalidValues = self.get_invalid_ticket_values(self.otherTicketValuesList)
        result = sum(invalidValues)

        return helper.validate_result('What is your ticket scanning error rate?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        self.parse_input_data(data.input)

        self.discard_invalid_tickets()
        self.init_field_order_list()
        self.identify_fields()

        indexes: List[int] = []
        for i in self.fieldOrderDict:
            field = self.fieldOrderDict[i][0]
            if field.startswith("departure"):
                indexes.append(i)

        result = 1
        myTicketIntValues = self.get_ticket_values(self.myTicketValues)
        for i in indexes:
            result *= myTicketIntValues[i]

        return helper.validate_result('What do you get if you multiply those six values together?', result, data.expectedAnswer)


    def solve(self):
        print("Day 16: Ticket Translation")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        # self.run_example(lambda: "P2 Ex2) " + self.run_part2(InputData('example2', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))