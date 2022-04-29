from typing import List
from venv import create

from intcode_computer import IntCode, IntCodeState


def part1(input:List[int], fixedPhase:List[int]) -> int:

    def output_handler(value):
        nonlocal outputValue
        outputValue = value

    def create_amp(inputValue:int) -> IntCode:
        amp = IntCode(input)
        amp.add_input(inputValue)
        amp.add_output_callback(output_handler)
        return amp

    phaseList = [fixedPhase] if fixedPhase != None else get_phase_permutations([0,1,2,3,4])
    result = 0

    for phase in phaseList:
        allAmps = [create_amp(phaseItem) for phaseItem in phase]

        outputValue = 0

        for amp in allAmps:
            amp.add_input(outputValue)
            amp.run()

        if outputValue > result:
            result = outputValue

    return result


def part2(input:List[int], fixedPhase:List[int]) -> int:

    def output_handler(value):
        nonlocal outputValue
        outputValue = value

    def create_amp(inputValue:int) -> IntCode:
        amp = IntCode(input)
        amp.add_input(inputValue)
        amp.add_output_callback(output_handler)
        return amp

    phaseList = [fixedPhase] if fixedPhase != None else get_phase_permutations([5,6,7,8,9])
    result = 0

    for phase in phaseList:
        allAmps = [create_amp(phaseItem) for phaseItem in phase]

        outputValue = 0

        while True:
            lastAmp = None
            for amp in allAmps:
                amp.add_input(outputValue)
                amp.run()
                lastAmp = amp

            if lastAmp.state == IntCodeState.Finished:
                break

        if outputValue > result:
            result = outputValue

    return result


def get_phase_permutations(values:List[int]) -> List[List[int]]:
    if len(values) == 1:
        temp = [values[0]]
        yield temp

    for item in values:
        nextItems = list(filter(
            lambda x: x != item,
            values
        ))

        for result in get_phase_permutations(nextItems):
            yield [item] + result
