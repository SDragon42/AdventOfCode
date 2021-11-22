import utils
import sys
from typing import List

sys.path.append('../../Python.Common')
import helper
import inputHelper


CardList = List[int]

Const_Player1 = "Player 1"
Const_Player2 = "Player 2"


def parse_input(input: str) -> tuple[CardList, CardList]:
    decks = {}
    partsList = input.split("\n\n")
    for part in partsList:
        bits = part.split("\n")
        key = bits[0].replace(":", "")
        cards = [int(x) for x in bits[1:]]
        decks[key] = cards

    return decks[Const_Player1], decks[Const_Player2]


def play_combat(p1Deck: CardList, p2Deck: CardList) -> CardList:
    round = 0
    while len(p1Deck) > 0 and len(p2Deck) > 0:
        round += 1
        utils.dprint(f"-- Round {round} --")
        utils.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
        utils.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

        p1Card = p1Deck.pop(0)
        p2Card = p2Deck.pop(0)

        utils.dprint(f"{Const_Player1} plays {p1Card}")
        utils.dprint(f"{Const_Player2} plays {p2Card}")
        if p1Card > p2Card:
            utils.dprint(f"{Const_Player1} wins the round!")
            # add_to_deck(p1Deck, p1Card, p2Card)
            p1Deck.append(p1Card)
            p1Deck.append(p2Card)
        else:
            utils.dprint(f"{Const_Player2} wins the round!")
            # add_to_deck(p2Deck, p2Card, p1Card)
            p2Deck.append(p2Card)
            p2Deck.append(p1Card)
        utils.dprint("")

    utils.dprint("== Post-game results ==")
    utils.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
    utils.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

    if len(p1Deck) > 0:
        return p1Deck
    return p2Deck


def add_to_history(game: int, p1Deck: CardList, p2Deck: CardList, history: List[str]) -> bool:
    p1DeckStr = ",".join(str(x) for x in p1Deck)
    p2DeckStr = ",".join(str(x) for x in p2Deck)
    state = f"Game {game}|{Const_Player1}:{p1DeckStr}|{Const_Player2}:{p2DeckStr}"
    if state in history:
        return False
    history.append(state)
    return True


nextGame:int = 1
def play_recursive_combat(p1Deck: CardList, p2Deck: CardList, history: List[str]) -> tuple[str, CardList]:
    global nextGame

    game = nextGame
    nextGame += 1

    if game == 5:
        utils.dprint("")

    utils.dprint(f"=== Game {game} ===")
    utils.dprint("")

    round = 0
    winner = ""
    winningDeck: CardList = []
    while len(p1Deck) > 0 and len(p2Deck) > 0:
        round += 1

        if not add_to_history(game, p1Deck, p2Deck, history):
            utils.dprint(f"{Const_Player1} wins round {round} of game {game}!")
            return Const_Player1, p1Deck

        utils.dprint(f"-- Round {round} (Game {game}) --")
        utils.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
        utils.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

        p1Card = p1Deck.pop(0)
        p2Card = p2Deck.pop(0)

        utils.dprint(f"{Const_Player1} plays {p1Card}")
        utils.dprint(f"{Const_Player2} plays {p2Card}")

        if p1Card <= len(p1Deck) and p2Card <= len(p2Deck):
            utils.dprint("Playing a sub-game to determine the winner...")
            utils.dprint("")
            p1SubDeck = p1Deck[:p1Card]
            p2SubDeck = p2Deck[:p2Card]
            winner, winningDeck = play_recursive_combat(p1SubDeck, p2SubDeck, history)
        else:
            if p1Card > p2Card:
                winner = Const_Player1
            else:
                winner = Const_Player2

        utils.dprint(f"{winner} wins round {round} of game {game}!")

        if winner == Const_Player1:
            p1Deck.append(p1Card)
            p1Deck.append(p2Card)
        else:
            p2Deck.append(p2Card)
            p2Deck.append(p1Card)
        utils.dprint("")

    if game == 1:
        utils.dprint("== Post-game results ==")
        utils.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
        utils.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))
    else:
        utils.dprint(f"The winner of game {game} is {winner}!")

    if winner == Const_Player1:
        return Const_Player1, p1Deck
    return Const_Player2, p2Deck


def calculate_score(deck: CardList, multiplier: int) -> int:
    if len(deck) == 0:
        return 0

    card = deck.pop(len(deck) - 1)
    result = (card * multiplier) + calculate_score(deck, multiplier + 1)
    return result


def run_part1(title: str, input: str, correctResult: int):
    p1Deck, p2Deck = parse_input(input)
    winnerDeck = play_combat(p1Deck, p2Deck)
    result = calculate_score(winnerDeck, 1)
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: str, correctResult: int):
    p1Deck, p2Deck = parse_input(input)
    history: List[str] = []
    winner, winnerDeck = play_recursive_combat(p1Deck, p2Deck, history)
    result = calculate_score(winnerDeck, 1)
    utils.validate_result(title, result, correctResult)


# utils.showDebug = True
def solve():
    day = 22
    print(f"---- Day {day}: Crab Combat ----")

    # run_part1("Test Case 1",
    #     utils.read_input(day, "example1"),
    #     306)
    run_part1("problem",
        utils.read_input(day, "input"),
        31314)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input(day, "example1"),
    #     291)
    # run_part2("Test Case 2",
    #     utils.read_input(day, "example2"),
    #     105)
    run_part2("problem",
        utils.read_input(day, "input"),
        32760)


if __name__ == "__main__":
    solve()