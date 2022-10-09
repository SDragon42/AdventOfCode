from typing import List, Dict



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



fieldDefInputList: List[FieldInfo] = []
myTicketValues: str = ""
otherTicketValuesList: List[str] = []
fieldOrderDict: Dict[int, List[str]] = {}

#-------------------------------------------------------------------------------

def parse_input_data(inputList: List[str]):
    global myTicketValues
    global otherTicketValuesList

    endIdx = inputList.index("")
    parse_field_definitions(inputList[:endIdx])

    startIdx = endIdx + 2
    endIdx = inputList.index("", startIdx)
    myTicketValues = inputList[startIdx]

    otherTicketValuesList.clear()
    startIdx = endIdx + 2
    otherTicketValuesList = inputList[startIdx:]


def get_field_info_record(field: str) -> FieldInfo:
    for f in fieldDefInputList:
        if f.field == field:
            return f
    rec = FieldInfo(field)
    fieldDefInputList.append(rec)
    return rec


def parse_field_definitions(inputList: List[str]):
    fieldDefInputList.clear()

    for line in inputList:
        fieldDelim = line.index(":")
        field = line[:fieldDelim]

        fieldRecord = get_field_info_record(field)

        ranges = line[fieldDelim+1:].split(" or ")
        for r in ranges:
            parts = r.split("-")
            
            lowVal = int(parts[0])
            highVal = int(parts[1])

            fieldRecord.add_range(lowVal, highVal)


def init_field_order_list():
    fieldOrderDict.clear()
    i = 0
    numValues = myTicketValues.count(",") + 1
    while i < numValues:
        fieldList: List[str] = []
        for f in fieldDefInputList:
            fieldList.append(f.field)
        fieldOrderDict[i] = fieldList
        i += 1


#-------------------------------------------------------------------------------

def get_ticket_values(ticket: str) -> List[int]:
    result: List[int] = []
    for value in ticket.split(","):
        result.append(int(value))
    return result


def is_ticket_valid(ticket: str, invalidValues: List[int]) -> bool:
    valueList = get_ticket_values(ticket)
    result = True
    for value in valueList:
        isInvalid = True
        for field in fieldDefInputList:
            if field.in_range(value):
                isInvalid = False
                break
        if isInvalid:
            invalidValues.append(value)
            result = False
    return result


def get_invalid_ticket_values(ticketValues: List[str]) -> List[int]:
    invalidValues: List[int] = []
    for ticket in ticketValues:
        is_ticket_valid(ticket, invalidValues)
    return invalidValues


def discard_invalid_tickets():
    i = 0
    while i < len(otherTicketValuesList):
        ticket = otherTicketValuesList[i]
        if not is_ticket_valid(ticket, []):
            del otherTicketValuesList[i]
            continue
        i += 1


def identified_all_fields() -> bool:
    for x in fieldOrderDict:
        if len(fieldOrderDict[x]) > 1:
            return False
    return True


def identify_fields():
    identify_fields_with_ticket(myTicketValues)
    for ticket in otherTicketValuesList:
        identify_fields_with_ticket(ticket)
        if identified_all_fields():
            return


def identify_fields_with_ticket(ticket: str):
    valueList = get_ticket_values(ticket)

    i = 0
    while i < len(valueList):
        for f in fieldDefInputList:
            if not f.in_range(valueList[i]):
                fields = fieldOrderDict[i]
                if f.field in fields:
                    fields.remove(f.field)
                break
        i += 1
    
    # remove single field entries from other posibilities
    i = 0
    while i < len(fieldOrderDict):
        anyRemoved = False
        if len(fieldOrderDict[i]) == 1:
            field = fieldOrderDict[i][0]
            k = 0
            while k < len(fieldOrderDict):
                if k != i:
                    if field in fieldOrderDict[k]:
                        fieldOrderDict[k].remove(field)
                        anyRemoved = True
                k += 1
        if anyRemoved:
            i = 0
        else:
            i += 1


def run_part1(input: List[str]) -> int:
    parse_input_data(input)

    invalidValues = get_invalid_ticket_values(otherTicketValuesList)
    result = sum(invalidValues)

    return result


def run_part2(input: List[str]) -> int:
    parse_input_data(input)

    discard_invalid_tickets()
    init_field_order_list()
    identify_fields()

    indexes: List[int] = []
    for i in fieldOrderDict:
        field = fieldOrderDict[i][0]
        if field.startswith("departure"):
            indexes.append(i)

    result = 1
    myTicketIntValues = get_ticket_values(myTicketValues)
    for i in indexes:
        result *= myTicketIntValues[i]

    return result
