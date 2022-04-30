from typing import List

from intcode_computer import IntCode, IntCodeState


def run_code(input:List[int], inputValue:int) -> int:

    def output_handler(outputValue):
        nonlocal result
        result = outputValue

    result = 0

    comp = IntCode(input)
    comp.add_output_callback(output_handler)
    comp.run()

    if comp.state == IntCodeState.NeedsInput:
        comp.add_input(inputValue)
        comp.run()

    return result