# from: https://github.com/ephemient/aoc2020/blob/main/py/src/aoc2020/day22.py

from typing import List, Tuple
from collections import deque



def build_decks(input: List[str]) -> Tuple[deque, deque]:
    it = iter(input)
    line = next(it)
    
    assert line.rstrip() == 'Player 1:'
    player1Deck = deque()
    while (line := next(it)).rstrip():
        player1Deck.append(int(line))
    line = next(it)
    
    assert line.rstrip() == 'Player 2:'
    player2Deck = deque()
    while True:
        try:
            line = next(it)
        except StopIteration:
            break
        player2Deck.append(int(line))
    
    return player1Deck, player2Deck


def go(deck1: deque, deck2: deque) -> bool:
    seen = set()
    while deck1 and deck2:
        state = tuple(deck1), tuple(deck2)
        if state in seen:
            return False
        seen.add(state)
        card1 = deck1.popleft()
        card2 = deck2.popleft()
        if (go(deque(list(deck1)[:card1]), deque(list(deck2)[:card2]))
                if card1 <= len(deck1) and card2 <= len(deck2) else
                card1 < card2):
            deck2.append(card2)
            deck2.append(card1)
        else:
            deck1.append(card1)
            deck1.append(card2)
    return not deck1


def run_part1(input: List[str]) -> int:
    player1Deck, player2Deck = build_decks(input)
    while player1Deck and player2Deck:
        card1 = player1Deck.popleft()
        card2 = player2Deck.popleft()
        if card1 < card2:
            player2Deck.append(card2)
            player2Deck.append(card1)
        else:
            player1Deck.append(card1)
            player1Deck.append(card2)
    winner = player1Deck or player2Deck

    result = sum((len(winner) - i) * x for i, x in enumerate(winner))

    return result


def run_part2(input: List[str]) -> int:
    player1Deck, player2Deck = build_decks(input)
    go(player1Deck, player2Deck)
    winner = player1Deck or player2Deck
    result = sum((len(winner) - i) * x for i, x in enumerate(winner))
    
    return result
