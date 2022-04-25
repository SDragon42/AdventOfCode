from cgitb import reset
from distutils.log import error
from typing import Callable, Dict, List, Tuple
from enum import Enum

# CallTuple = Tuple[Callable, int]
# InstructionDict = Dict[str, Callable]


class IntCodeState(Enum):
    """
    IntCode State enumeration
    """
    Running = 0
    Finished = 1
    NeedsInput = 2


class IntCode:
    """
    IntCode computer.
    """

    _memory:List[int]
    _instuctions:Dict[str, Callable]
    _state:IntCodeState
    _position:int

    def __init__(self, memory:List[int]) -> None:
        # self._memory = memory
        self._memory = list(memory) # maybe use this to make a local copy 
        self._position = 0

        # setup instructions
        self._instuctions = {
            # <OpCode>: (<function>, <num_parameters>),
            1: self._opcode_add,
            2: self._opcode_multiply,
            99: self._opcode_quit
        }

        self._state = IntCodeState.Running



    def run(self) -> None:
        if self._state == IntCodeState.Finished:
            raise error('Already Finished')
        if self._state == IntCodeState.NeedsInput:
            raise error('Needs Input')

        while self._state == IntCodeState.Running:
            opCode = self._read_instruction()
            if opCode not in self._instuctions:
                raise error('Unknown OpCode', opCode)
                
            func = self._instuctions[opCode]
            func()

    def read_value_at(self, position:int) -> int:
        value = self._memory[position]
        return value

    def write_value_at(self, position:int, value:int) -> None:
        self._memory[position] = value

    
    
    def _opcode_add(self):
        value_1 = self._memory[self._memory[self._position + 1]]
        value_2 = self._memory[self._memory[self._position + 2]]
        result = value_1 + value_2
        self._memory[self._memory[self._position + 3]] = result
        self._position += 4

    def _opcode_multiply(self):
        value_1 = self._memory[self._memory[self._position + 1]]
        value_2 = self._memory[self._memory[self._position + 2]]
        result = value_1 * value_2
        self._memory[self._memory[self._position + 3]] = result
        self._position += 4

    def _opcode_quit(self):
        self._state = IntCodeState.Finished
        self._position += 1

    
    def _read_instruction(self) -> int:
        # opCode = self._memory[self._position]
        # return opCode
        return self.read_value_at(self._position)