from typing import List, Tuple



CardList = List[int]



Const_Player1: str = "Player 1"
Const_Player2: str = "Player 2"




nextGame: int = 1


def build_decks(input: List[str]) -> Tuple[CardList, CardList]:
    it = iter(input)
    line = next(it)
    
    assert line.rstrip() == 'Player 1:'
    player1Deck:CardList = []
    while (line := next(it)).rstrip():
        player1Deck.append(int(line))
    line = next(it)
    
    assert line.rstrip() == 'Player 2:'
    player2Deck:CardList = []
    while True:
        try:
            line = next(it)
        except StopIteration:
            break
        player2Deck.append(int(line))
    
    return player1Deck, player2Deck


def play_combat(p1Deck: CardList, p2Deck: CardList) -> CardList:
    round = 0
    while len(p1Deck) > 0 and len(p2Deck) > 0:
        round += 1
        # helper.dprint(f"-- Round {round} --")
        # helper.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
        # helper.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

        p1Card = p1Deck.pop(0)
        p2Card = p2Deck.pop(0)

        # helper.dprint(f"{Const_Player1} plays {p1Card}")
        # helper.dprint(f"{Const_Player2} plays {p2Card}")
        if p1Card > p2Card:
            # helper.dprint(f"{Const_Player1} wins the round!")
            # add_to_deck(p1Deck, p1Card, p2Card)
            p1Deck.append(p1Card)
            p1Deck.append(p2Card)
        else:
            # helper.dprint(f"{Const_Player2} wins the round!")
            # add_to_deck(p2Deck, p2Card, p1Card)
            p2Deck.append(p2Card)
            p2Deck.append(p1Card)
        # helper.dprint("")

    # helper.dprint("== Post-game results ==")
    # helper.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
    # helper.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

    return p1Deck if len(p1Deck) > 0 else p2Deck


def add_to_history(game: int, p1Deck: CardList, p2Deck: CardList, history: List[str]) -> bool:
    p1DeckStr = ",".join(str(x) for x in p1Deck)
    p2DeckStr = ",".join(str(x) for x in p2Deck)
    state = f"Game {game}|{Const_Player1}:{p1DeckStr}|{Const_Player2}:{p2DeckStr}"
    if state in history:
        return False
    history.append(state)
    return True


def play_recursive_combat(p1Deck: CardList, p2Deck: CardList, history: List[str]) -> Tuple[str, CardList]:
    global nextGame

    game = nextGame
    nextGame += 1

    # if game == 5:
    #     helper.dprint("")

    # helper.dprint(f"=== Game {game} ===")
    # helper.dprint("")

    round = 0
    winner = ""
    winningDeck: CardList = []
    while len(p1Deck) > 0 and len(p2Deck) > 0:
        round += 1

        if not add_to_history(game, p1Deck, p2Deck, history):
            # helper.dprint(f"{Const_Player1} wins round {round} of game {game}!")
            return Const_Player1, p1Deck

        # helper.dprint(f"-- Round {round} (Game {game}) --")
        # helper.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
        # helper.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))

        p1Card = p1Deck.pop(0)
        p2Card = p2Deck.pop(0)

        # helper.dprint(f"{Const_Player1} plays {p1Card}")
        # helper.dprint(f"{Const_Player2} plays {p2Card}")

        if p1Card <= len(p1Deck) and p2Card <= len(p2Deck):
            # helper.dprint("Playing a sub-game to determine the winner...")
            # helper.dprint("")
            p1SubDeck = p1Deck[:p1Card]
            p2SubDeck = p2Deck[:p2Card]
            winner, winningDeck = play_recursive_combat(p1SubDeck, p2SubDeck, history)
        else:
            winner = Const_Player1 if p1Card > p2Card else Const_Player2

        # helper.dprint(f"{winner} wins round {round} of game {game}!")

        if winner == Const_Player1:
            p1Deck.append(p1Card)
            p1Deck.append(p2Card)
        else:
            p2Deck.append(p2Card)
            p2Deck.append(p1Card)
        # helper.dprint("")

    # if game == 1:
    #     helper.dprint("== Post-game results ==")
    #     helper.dprint(f"{Const_Player1}'s deck: " + ", ".join(str(x) for x in p1Deck))
    #     helper.dprint(f"{Const_Player2}'s deck: " + ", ".join(str(x) for x in p2Deck))
    # else:
    #     helper.dprint(f"The winner of game {game} is {winner}!")

    return (Const_Player1, p1Deck) if winner == Const_Player1 else (Const_Player2, p2Deck)


def calculate_score(deck: CardList, multiplier: int) -> int:
    if len(deck) == 0:
        return 0

    card = deck.pop(len(deck) - 1)
    result = (card * multiplier) + calculate_score(deck, multiplier + 1)
    return result


def run_part1(input: List[str]) -> int:
    player1Deck, player2Deck = build_decks(input)
    winnerDeck = play_combat(player1Deck, player2Deck)
    result = calculate_score(winnerDeck, 1)
    return result


def run_part2(input: List[str]) -> int:
    player1Deck, player2Deck = build_decks(input)
    history: List[str] = []
    winner, winnerDeck = play_recursive_combat(player1Deck, player2Deck, history)
    result = calculate_score(winnerDeck, 1)
    return result
