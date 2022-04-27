from typing import Callable, Dict, List
from enum import Enum


class IntCodeState(Enum):
    """
    IntCode State enumeration
    """
    Running = 0
    Finished = 1
    NeedsInput = 2


class IntCodeStateException(Exception):
    MESSAGE_FINISHED = 'Already Finished'
    MESSAGE_NEEDS_INPUT = 'Needs Input'
    MESSAGE_UNKNOWN = 'Unknown state issue'

    def __init__(self, state:IntCodeState) -> None:
        if state == IntCodeState.Finished:
            super().__init__(self.MESSAGE_FINISHED)
        elif state == IntCodeState.NeedsInput:
            super().__init__(self.MESSAGE_NEEDS_INPUT)
        else:
            super().__init__(self.MESSAGE_UNKNOWN)
    

class IntCodeOpcodeException(Exception):
    MESSAGE_UNKNOWN_OPCODE = 'Unknown OpCode'
    def __init__(self, opcode:int) -> None:
        super().__init__(self.MESSAGE_UNKNOWN_OPCODE + f' ({opcode})')


class IntCode:
    """
    IntCode computer.
    """

    _memory:List[int]
    _position:int
    _state:IntCodeState

    _instuctions:Dict[str, Callable]

    _outputListeners:List[Callable]

    def __init__(self, memory:List[int]) -> None:
        self._memory = list(memory)
        self._position = 0
        self._state = IntCodeState.Running
        
        # setup instructions
        self._instuctions = {
            # <OpCode>: (<function>, <num_parameters>),
            1: self._opcode_add,
            2: self._opcode_multiply,
            99: self._opcode_quit
        }

        # listeners
        self._outputListeners = []
        


    def _on_output(self, value:int) -> None:
        for listener in self._outputListeners:
            listener(value)

    def bind_output(self, listener:Callable) -> None:
        if listener not in self._outputListeners:
            self._outputListeners.append(listener)



    def run(self) -> None:
        if self._state == IntCodeState.Finished:
            raise IntCodeStateException(self._state)
        if self._state == IntCodeState.NeedsInput:
            raise IntCodeStateException(self._state)

        while self._state == IntCodeState.Running:
            opCode = self._read_instruction()
            if opCode not in self._instuctions:
                raise IntCodeOpcodeException(opCode)
                
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
        opCode = self.read_value_at(self._position)
        return opCode