# from: https://github.com/ephemient/aoc2020/blob/main/py/src/aoc2020/day22.py

from typing import List, Tuple
from collections import deque

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    player1Deck: deque
    player2Deck: deque
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 22
        
        lines = inputHelper.load_file(day, name).splitlines()
        it = iter(lines)
        line = next(it)
        
        assert line.rstrip() == 'Player 1:'
        self.player1Deck = deque()
        while (line := next(it)).rstrip():
            self.player1Deck.append(int(line))
        line = next(it)
        
        assert line.rstrip() == 'Player 2:'
        self.player2Deck = deque()
        while True:
            try:
                line = next(it)
            except StopIteration:
                break
            self.player2Deck.append(int(line))
        
        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class Puzzle(PuzzleBase):

    def go(self, deck1: deque, deck2: deque) -> bool:
        seen = set()
        while deck1 and deck2:
            state = tuple(deck1), tuple(deck2)
            if state in seen:
                return False
            seen.add(state)
            card1 = deck1.popleft()
            card2 = deck2.popleft()
            if (self.go(deque(list(deck1)[:card1]), deque(list(deck2)[:card2]))
                    if card1 <= len(deck1) and card2 <= len(deck2) else
                    card1 < card2):
                deck2.append(card2)
                deck2.append(card1)
            else:
                deck1.append(card1)
                deck1.append(card2)
        return not deck1


    def run_part1(self, data: InputData) -> str:
        while data.player1Deck and data.player2Deck:
            card1 = data.player1Deck.popleft()
            card2 = data.player2Deck.popleft()
            if card1 < card2:
                data.player2Deck.append(card2)
                data.player2Deck.append(card1)
            else:
                data.player1Deck.append(card1)
                data.player1Deck.append(card2)
        winner = data.player1Deck or data.player2Deck

        result = sum((len(winner) - i) * x for i, x in enumerate(winner))

        return helper.validate_result("What is the winning player's score?", result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        self.go(data.player1Deck, data.player2Deck)
        winner = data.player1Deck or data.player2Deck
        result = sum((len(winner) - i) * x for i, x in enumerate(winner))
        
        return helper.validate_result("What is the winning player's score?", result, data.expectedAnswer)


    def solve(self):
        print("Day 22: Crab Combat")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_example(lambda: "P2 Ex2) " + self.run_part2(InputData('example2', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))