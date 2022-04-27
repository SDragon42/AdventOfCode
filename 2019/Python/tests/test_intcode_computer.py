import unittest

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

from helper import string_to_int_list
from src.intcode_computer import (
    IntCode,
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
            comp.write_value_at(memoryAddress, expected)
            value = comp.read_value_at(memoryAddress)
            self.assertEqual(value, expected)

    def test_opcode_add(self):
        memory = string_to_int_list('99,5,6,3,99,20,22')

        comp = IntCode(memory)
        comp._opcode_add()
        value = comp.read_value_at(3)

        self.assertEqual(value, 42)
        self.assertEqual(comp._position, 4)

    def test_opcode_multiply(self):
        memory = string_to_int_list('99,5,6,3,99,20,22')

        comp = IntCode(memory)
        comp._opcode_multiply()
        value = comp.read_value_at(3)

        self.assertEqual(value, 440)
        self.assertEqual(comp._position, 4)

    def test_opcode_quit(self):
        memory = string_to_int_list('99,0')

        comp = IntCode(memory)
        comp._opcode_quit()

        self.assertEqual(comp._state, IntCodeState.Finished)
        self.assertEqual(comp._position, 1)

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
        result = -1
        def the_ouput(*args):
            nonlocal result
            result = args[0]
        comp = IntCode([])
        comp.bind_output(the_ouput)
        comp._on_output(42)

        self.assertEquals(result, 42)