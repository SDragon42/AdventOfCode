from typing import List

from intcode_computer import IntCode

def run_code(input:List[int], valueAt1:int = -1, valueAt2:int = -1) -> int:
    comp = IntCode(input)
    if valueAt1 != -1:
        comp.write_value_at(valueAt1, 1)
    if valueAt2 != -1:
        comp.write_value_at(valueAt2, 2)
    comp.run()

    value = comp.read_value_at(0)
    return value

def find_noun_verb(input:List[int], desiredValueAt0:int) -> int:
    valueRange = range(0, 99, 1)
    for noun in valueRange:
        for verb in valueRange:
            value = run_code(input, noun, verb)
            if value == desiredValueAt0:
                return (100 * noun) + verb
    return -1