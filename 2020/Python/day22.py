from typing import List, Tuple

import helper
import inputHelper
from puzzleBase import PuzzleBase



CardList = List[int]



Const_Player1: str = "Player 1"
Const_Player2: str = "Player 2"



class InputData:
    expectedAnswer: int
    player1Deck: CardList
    player2Deck: CardList


    def __init__(self, name: str, part: int) -> None:
        day = 22

        decks = {}
        playerblocks = inputHelper.load_file(day, name).split("\n\n")
        for player in playerblocks:
            bits = player.splitlines()
            key = bits[0].replace(":", "")
            decks[key] = [int(x) for x in bits[1:]]
        self.player1Deck = decks[Const_Player1]
        self.player2Deck = decks[Const_Player2]

        self.expectedAnswer = int(inputHelper.load_file(day, f"{name}-answer{part}"))



class Puzzle(PuzzleBase):
    nextGame: int = 1


    def play_combat(self, p1Deck: CardList, p2Deck: CardList) -> CardList:
        round = 0
        while len(p1Deck) > 0 and len(p2Deck) > 0:
            round += 1
            helper.dprint(f"-- Round {round} --")
            helper.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
            helper.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

            p1Card = p1Deck.pop(0)
            p2Card = p2Deck.pop(0)

            helper.dprint(f"{Const_Player1} plays {p1Card}")
            helper.dprint(f"{Const_Player2} plays {p2Card}")
            if p1Card > p2Card:
                helper.dprint(f"{Const_Player1} wins the round!")
                # add_to_deck(p1Deck, p1Card, p2Card)
                p1Deck.append(p1Card)
                p1Deck.append(p2Card)
            else:
                helper.dprint(f"{Const_Player2} wins the round!")
                # add_to_deck(p2Deck, p2Card, p1Card)
                p2Deck.append(p2Card)
                p2Deck.append(p1Card)
            helper.dprint("")

        helper.dprint("== Post-game results ==")
        helper.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
        helper.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

        return p1Deck if len(p1Deck) > 0 else p2Deck


    def add_to_history(self, game: int, p1Deck: CardList, p2Deck: CardList, history: List[str]) -> bool:
        p1DeckStr = ",".join(str(x) for x in p1Deck)
        p2DeckStr = ",".join(str(x) for x in p2Deck)
        state = f"Game {game}|{Const_Player1}:{p1DeckStr}|{Const_Player2}:{p2DeckStr}"
        if state in history:
            return False
        history.append(state)
        return True


    def play_recursive_combat(self, p1Deck: CardList, p2Deck: CardList, history: List[str]) -> Tuple[str, CardList]:
        game = self.nextGame
        self.nextGame += 1

        if game == 5:
            helper.dprint("")

        helper.dprint(f"=== Game {game} ===")
        helper.dprint("")

        round = 0
        winner = ""
        winningDeck: CardList = []
        while len(p1Deck) > 0 and len(p2Deck) > 0:
            round += 1

            if not self.add_to_history(game, p1Deck, p2Deck, history):
                helper.dprint(f"{Const_Player1} wins round {round} of game {game}!")
                return Const_Player1, p1Deck

            helper.dprint(f"-- Round {round} (Game {game}) --")
            helper.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
            helper.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

            p1Card = p1Deck.pop(0)
            p2Card = p2Deck.pop(0)

            helper.dprint(f"{Const_Player1} plays {p1Card}")
            helper.dprint(f"{Const_Player2} plays {p2Card}")

            if p1Card <= len(p1Deck) and p2Card <= len(p2Deck):
                helper.dprint("Playing a sub-game to determine the winner...")
                helper.dprint("")
                p1SubDeck = p1Deck[:p1Card]
                p2SubDeck = p2Deck[:p2Card]
                winner, winningDeck = self.play_recursive_combat(p1SubDeck, p2SubDeck, history)
            else:
                winner = Const_Player1 if p1Card > p2Card else Const_Player2

            helper.dprint(f"{winner} wins round {round} of game {game}!")

            if winner == Const_Player1:
                p1Deck.append(p1Card)
                p1Deck.append(p2Card)
            else:
                p2Deck.append(p2Card)
                p2Deck.append(p1Card)
            helper.dprint("")

        if game == 1:
            helper.dprint("== Post-game results ==")
            helper.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
            helper.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))
        else:
            helper.dprint(f"The winner of game {game} is {winner}!")

        return (Const_Player1, p1Deck) if winner == Const_Player1 else (Const_Player2, p2Deck)


    def calculate_score(self, deck: CardList, multiplier: int) -> int:
        if len(deck) == 0:
            return 0

        card = deck.pop(len(deck) - 1)
        result = (card * multiplier) + self.calculate_score(deck, multiplier + 1)
        return result


    def run_part1(self, data: InputData) -> str:
        winnerDeck = self.play_combat(data.player1Deck, data.player2Deck)
        result = self.calculate_score(winnerDeck, 1)
        return helper.validate_result("What is the winning player's score?", result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        history: List[str] = []
        winner, winnerDeck = self.play_recursive_combat(data.player1Deck, data.player2Deck, history)
        result = self.calculate_score(winnerDeck, 1)
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