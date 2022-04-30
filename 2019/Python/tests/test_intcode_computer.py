import unittest
from unittest import result

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

from helper import string_to_int_list
from src.intcode_computer import (
    IntCode,
    IntCodeParameterMode,
    IntCodeState,
    IntCodeOpcodeException,
    IntCodeStateException
)


class IntCodeComputer(unittest.TestCase):
    """
    Tests for the IntCode class
    """

    # def setUp(self) -> None:
    #     return super().setUp()

    # def tearDown(self) -> None:
    #     return super().tearDown()

    def test_read_value_at(self):
        data = [
            (0, 1),
            (1, 10),
            (2, 20),
            (3, 30),
        ]
        memory = [v for _,v in data]

        for memoryAddress, expected in data:
            comp = IntCode(memory)
            value = comp.read_value_at(memoryAddress)
            self.assertEqual(value, expected)

    def test_write_value_as(self):
        memory = string_to_int_list('0,10,20,30,40,50')
        data = [
            (1, 15),
            (3, 35),
        ]
        for memoryAddress, expected in data:
            comp = IntCode(memory)
            comp.write_value_at(expected, memoryAddress)
            value = comp.read_value_at(memoryAddress)
            self.assertEqual(value, expected)

    def test_read_instruction(self):
        memory = string_to_int_list('10101,1,2,3,42')
        comp = IntCode(memory)
        comp._read_instruction()
        
        self.assertEquals(comp._position, 0)
        self.assertEquals(comp._opcode, 1)
        self.assertEquals(len(comp._paramModes), 3)
        self.assertEquals(comp._paramModes[0], IntCodeParameterMode.Immediate)
        self.assertEquals(comp._paramModes[1], IntCodeParameterMode.Positional)
        self.assertEquals(comp._paramModes[2], IntCodeParameterMode.Immediate)

    def test_move_position(self):
        memory = string_to_int_list('10101,1,2,3,42')
        comp = IntCode(memory)
        comp._read_instruction()
        comp._move_position(4)

        self.assertEquals(comp._position, 4)
        self.assertEquals(comp._opcode, -1)
        self.assertEquals(len(comp._paramModes), 0)


    def test_opcode_add(self):
        data = [
            ('1,5,6,7,99,20,22,0', 7, 5, 42),
            ('1101,20,22,7,99,0,0,0', 7, 5, 42),
        ]
        for memoryData, readAddr, expectedAddr, expectedValue in data:
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            
            comp.run()
            value = comp.read_value_at(readAddr)

            self.assertEqual(value, expectedValue)
            self.assertEqual(comp._position, expectedAddr)

    def test_opcode_multiply(self):
        data = [
            ('2,5,6,7,99,20,22,0', 7, 5, 440),
            ('1102,20,22,7,99,0,0,0', 7, 5, 440),
        ]
        for memoryData, readAddr, expectedAddr, expectedValue in data:
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            
            comp.run()
            value = comp.read_value_at(readAddr)

            self.assertEqual(value, expectedValue)
            self.assertEqual(comp._position, expectedAddr)

    def test_opcode_input(self):
        data = [
            ('3,4,99,0,0', 4, 3, 42),
            ('103,4,99,0,0', 1, 3, 42),
        ]
        for memoryData, readAddr, expectedAddr, expectedValue in data:
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            comp.add_input(expectedValue)
            
            comp.run()
            value = comp.read_value_at(readAddr)

            self.assertEqual(value, expectedValue)
            self.assertEqual(comp._position, expectedAddr)

    def test_opcode_output(self):
        data = [
            ('4,4,99,0,42', 42),
            ('104,42,99,0,0', 42),
        ]
        for memoryData, expectedValue in data:
            def callback(*args):
                nonlocal result
                result = args[0]
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            comp.add_output_callback(callback)
            
            result = -1
            comp.run()

            self.assertEqual(result, expectedValue)

    def test_opcode_jump_if_true(self):
        data = [
            ('1105,0,5,99,-1,99,-2', 4, -1),
            ('1105,1,5,99,-1,99,-2', 6, -2),
        ]
        for memoryData, expectedAddr, expectedValue in data:
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            
            comp.run()
            
            self.assertEqual(comp._position, expectedAddr)
            value = comp.read_value_at(expectedAddr)
            self.assertEqual(value, expectedValue)

    def test_opcode_jump_if_false(self):
        data = [
            ('1106,0,5,99,-1,99,-2', 6, -2),
            ('1106,1,5,99,-1,99,-2', 4, -1),
        ]
        for memoryData, expectedAddr, expectedValue in data:
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            
            comp.run()
            
            self.assertEqual(comp._position, expectedAddr)
            value = comp.read_value_at(expectedAddr)
            self.assertEqual(value, expectedValue)

    def test_opcode_lessthan(self):
        data = [
            ('7,5,6,7,99,20,22,0', 7, 1),
            ('1107,20,22,7,99,0,0,0', 7, 1),
            ('7,5,6,7,99,22,20,0', 7, 0),
            ('1107,20,20,7,99,0,0,0', 7, 0),
        ]
        for memoryData, readAddr, expectedValue in data:
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            
            comp.run()
            value = comp.read_value_at(readAddr)

            self.assertEqual(value, expectedValue)

    def test_opcode_equals(self):
        data = [
            ('8,5,6,7,99,20,22,0', 7, 0),
            ('1108,20,22,7,99,0,0,0', 7, 0),
            ('8,5,6,7,99,22,20,0', 7, 0),
            ('1108,20,20,7,99,0,0,0', 7, 1),
        ]
        for memoryData, readAddr, expectedValue in data:
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            
            comp.run()
            value = comp.read_value_at(readAddr)

            self.assertEqual(value, expectedValue)

    def test_opcode_quit(self):
        data = [
            ('99,0', 1),
            ('1102,20,22,7,99,0,0,0', 5),
        ]
        for memoryData, expectedAddr in data:
            memory = string_to_int_list(memoryData)
            comp = IntCode(memory)
            
            comp.run()

            self.assertEqual(comp._state, IntCodeState.Finished)
            self.assertEqual(comp._position, expectedAddr)

    def test_exception_finished(self):
        with self.assertRaises(IntCodeStateException) as context:
            memory = string_to_int_list('99,5,6,3,99,20,22')
            comp = IntCode(memory)
            comp._state = IntCodeState.Finished
            comp.run()
        self.assertTrue(IntCodeStateException.MESSAGE_FINISHED in str(context.exception))


    def test_exception_needs_input(self):
        with self.assertRaises(IntCodeStateException) as context:
            memory = string_to_int_list('99,5,6,3,99,20,22')
            comp = IntCode(memory)
            comp._state = IntCodeState.NeedsInput
            comp.run()
        self.assertTrue(IntCodeStateException.MESSAGE_NEEDS_INPUT in str(context.exception))

    def test_exception_unknown_opcode(self):
        with self.assertRaises(IntCodeOpcodeException) as context:
            memory = string_to_int_list('97,5,6,3,99,20,22')
            comp = IntCode(memory)
            comp.run()

    def test_output_listener(self):
        def the_ouput(*args):
            nonlocal result
            result = args[0]

        comp = IntCode([])
        comp.add_output_callback(the_ouput)
        
        result = 0
        comp._on_output(42)

        self.assertEquals(result, 42)

    def test_output_listener_only_adds_once(self):
        def the_ouput(*args):
            pass
        
        comp = IntCode([])
        comp.add_output_callback(the_ouput)
        comp.add_output_callback(the_ouput)

        self.assertEquals(len(comp._outputCallbacks), 1)

    def test_add_input(self):
        comp = IntCode([])
        comp.add_input(1,2)
        self.assertEquals(comp._inputBuffer.qsize(), 2)

    def test_add_input_as_list_exception(self):
        comp = IntCode([])
        input = [1,2,3]

        with self.assertRaises(Exception) as context:
            comp.add_input(input)