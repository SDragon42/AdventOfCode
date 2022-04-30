from typing import Callable, Dict, List
from enum import Enum
from queue import Queue


class IntCodeState(Enum):
    """
    IntCode State enumeration
    """
    Running = 0
    Finished = 1
    NeedsInput = 2


class IntCodeParameterMode(Enum):
    """
    Parameter mode
    """
    Positional = 0
    Immediate = 1
    

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

    _memory:Dict[int, int]
    _position:int
    _state:IntCodeState
    _inputBuffer:Queue[int]
    _opcode:int
    _paramModes:List[IntCodeParameterMode]

    _instuctions:Dict[str, Callable]

    _outputCallbacks:List[Callable]


    @property
    def state(self) -> IntCodeState:
        return self._state


    def __init__(self, memory:List[int]) -> None:
        self._memory = {addr: memory[addr] for addr in range(len(memory))}

        self._position = 0
        self._state = IntCodeState.Running
        self._inputBuffer = Queue()
        self._opcode = 99
        self._paramModes = []
        
        # setup instructions
        self._instuctions = {
            # <OpCode>: <function>,
            1: self._opcode_add,
            2: self._opcode_multiply,
            3: self._opcode_input,
            4: self._opcode_output,
            5: self._opcode_jump_if_true,
            6: self._opcode_jump_if_false,
            7: self._opcode_lessthan,
            8: self._opcode_equals,
            99: self._opcode_quit,
        }

        # callbacks
        self._outputCallbacks = []
        


    def _on_output(self, value:int) -> None:
        for callback in self._outputCallbacks:
            callback(value)

    def add_output_callback(self, callback:Callable) -> None:
        """
        Adds a listener callback function.
        """
        if callback not in self._outputCallbacks:
            self._outputCallbacks.append(callback)



    def run(self) -> None:
        """
        Starts the execution of the IntCode program.
        """
        if self._state == IntCodeState.Finished:
            raise IntCodeStateException(self._state)
        if self._state == IntCodeState.NeedsInput and self._inputBuffer.empty():
            raise IntCodeStateException(self._state)

        self._state = IntCodeState.Running

        while self._state == IntCodeState.Running:
            self._read_instruction()
            if self._opcode not in self._instuctions:
                raise IntCodeOpcodeException(self._opcode)
                
            func = self._instuctions[self._opcode]
            func()

    def read_value_at(self, position:int) -> int:
        """
        Reads the value at a specified memory position.
        """
        value = self._read_memory(position, IntCodeParameterMode.Immediate)
        return value

    def write_value_at(self, value:int, position:int) -> None:
        """
        Writes a value at the specified memory position.
        """
        self._write_memory(value, position, IntCodeParameterMode.Immediate)


    
    def _read_instruction(self) -> None:
        # instruction format
        # <param3><param2><param1><op code>
        INSTRUCTION_SIZE = 5
        NUM_OPCODE_DIGITS = 2
        value = str(self.read_value_at(self._position)).rjust(INSTRUCTION_SIZE, '0')

        self._opcode = int(value[-NUM_OPCODE_DIGITS:])
        self._paramModes = [IntCodeParameterMode(int(v)) for v in value[0:INSTRUCTION_SIZE - NUM_OPCODE_DIGITS][::-1]]

    def _get_parameter_mode(self, param_number:int) -> IntCodeParameterMode:
        if len(self._paramModes) < param_number:
            return IntCodeParameterMode.Positional
        mode = self._paramModes[param_number - 1]
        return mode
        

    def _read_memory(self, position:int, mode:IntCodeParameterMode) -> int:
        valueAtPosition = self._memory.setdefault(position, 0)
        match mode:
            case IntCodeParameterMode.Positional:
                return self._read_memory(valueAtPosition, IntCodeParameterMode.Immediate)
            case IntCodeParameterMode.Immediate:
                return valueAtPosition
            case _:
                raise Exception("invalid paramater mode")

    def _write_memory(self, value:int, position:int, mode:IntCodeParameterMode) -> None:
        match mode:
            case IntCodeParameterMode.Positional:
                valueAtPosition = self._memory.setdefault(position, 0)
                self._write_memory(value, valueAtPosition, IntCodeParameterMode.Immediate)
            case IntCodeParameterMode.Immediate:
                self._memory[position] = value
            case _:
                raise Exception("invalid paramater mode")

    def _move_position(self, offset:int) -> None:
        self._position += offset
        self._opcode = -1
        self._paramModes = []


    def add_input(self, *args:int) -> None:
        """
        Adds values to the input buffer.
        """
        for value in args:
            if not isinstance(value, int):
                raise Exception('not an int')
            self._inputBuffer.put(value)
    
    def _read_param_value(self, num:int) -> int:
        value = self._read_memory(
            self._position + num, 
            self._get_parameter_mode(num)
        )
        return value

    def _write_param_value(self, num:int, value:int) -> None:
        self._write_memory(
            value, 
            self._position + num, 
            self._get_parameter_mode(num)
        )
        
    
    def _opcode_add(self):
        """
        Adds together numbers read from two positions and stores the result in a third position.
        """
        value1 = self._read_param_value(1)
        value2 = self._read_param_value(2)
        result = value1 + value2
        self._write_param_value(3, result)
        self._move_position(4)

    def _opcode_multiply(self):
        """
        Multiplies together numbers read from two positions and stores the result in a third position.
        """
        value1 = self._read_param_value(1)
        value2 = self._read_param_value(2)
        result = value1 * value2
        self._write_param_value(3, result)
        self._move_position(4)

    def _opcode_input(self):
        """
        Takes an input value from the input buffer, and writes it to memory.
        """
        if self._inputBuffer.empty():
            self._state = IntCodeState.NeedsInput
            return
        value = self._inputBuffer.get()
        self._write_param_value(1, value)
        self._move_position(2)

    def _opcode_output(self):
        """
        Outputs a value from memory.
        """
        value = self._read_param_value(1)
        self._on_output(value)
        self._move_position(2)

    def _opcode_jump_if_true(self):
        """
        """
        value1 = self._read_param_value(1)
        
        if value1 == 0:
            self._move_position(3)
            return

        value2 = self._read_param_value(2)
        self._move_position(0)
        self._position = value2

    def _opcode_jump_if_false(self):
        """
        """
        value1 = self._read_param_value(1)
        
        if value1 != 0:
            self._move_position(3)
            return

        value2 = self._read_param_value(2)
        self._move_position(0)
        self._position = value2

    def _opcode_lessthan(self):
        """
        """
        value1 = self._read_param_value(1)
        value2 = self._read_param_value(2)
        if value1 < value2:
            result = 1
        else:
            result = 0
        self._write_param_value(3, result)
        self._move_position(4)
        
    def _opcode_equals(self):
        """
        """
        value1 = self._read_param_value(1)
        value2 = self._read_param_value(2)
        if value1 == value2:
            result = 1
        else:
            result = 0
        self._write_param_value(3, result)
        self._move_position(4)

    def _opcode_quit(self):
        """
        Halts IntCode execution.
        """
        self._state = IntCodeState.Finished
        self._move_position(1)