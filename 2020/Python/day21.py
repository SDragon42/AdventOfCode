# ref: https://github.com/BastiHz/Advent_of_Code/blob/main/2020/day_21.py

from typing import List, Dict, Set, Tuple

import helper
import inputHelper

from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: str

    def __init__(self, name: str, part: int) -> None:
        day = 21
        self.input = inputHelper.load_file(day, name).splitlines()
        self.expectedAnswer = inputHelper.load_file(day, f"{name}-answer{part}")



# Types
IngredientsSet = Set[str]
AllergenList = List[str]
AllergenIngredientDict = Dict[str, IngredientsSet]
IngredientCounterDict = Dict[str, int]



class Puzzle(PuzzleBase):

    def parse_food_line(self, line: str) -> Tuple[IngredientsSet, AllergenList]:
        parts = line.split(" (contains ")
        ingredients = set(parts[0].split())
        allergens =parts[1].replace(")", "").split(", ")
        return ingredients, allergens


    def process_food_lines(self, foodLineList: List[str]) -> Tuple[AllergenIngredientDict, IngredientCounterDict]:
        allergen_ingredients: AllergenIngredientDict = {}
        ingredient_counter: IngredientCounterDict = {}

        for foodLine in foodLineList:
            ingredients, allergens = self.parse_food_line(foodLine)

            for a in allergens:
                if a not in allergen_ingredients:
                    allergen_ingredients[a] = ingredients.copy()
                else:
                    allergen_ingredients[a].intersection_update(ingredients)

            for i in ingredients:
                if i not in ingredient_counter:
                    ingredient_counter[i] = 0
                ingredient_counter[i] += 1

        return allergen_ingredients, ingredient_counter


    def get_ingredients_without_allergens(self, allergen_ingredients: AllergenIngredientDict, ingredient_counter: IngredientCounterDict) -> Set[str]:
        without_allergens = set(ingredient_counter.keys())
        for v in allergen_ingredients.values():
            for allergen in v:
                without_allergens.discard(allergen)
        return without_allergens


    def run_part1(self, data: InputData) -> str:
        allergen_ingredients, ingredient_counter = self.process_food_lines(data.input)

        without_allergens = self.get_ingredients_without_allergens(allergen_ingredients, ingredient_counter)
        result = sum(ingredient_counter[ingredient] for ingredient in without_allergens)
        result = str(result)

        return helper.validate_result('How many times do any of those ingredients appear?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        allergen_ingredients, ingredient_counter = self.process_food_lines(data.input)

        finished = False
        finalIngredients = set()
        while not finished:
            finished = True
            for allergen, ingredients in allergen_ingredients.items():
                if len(ingredients) != 1:
                    finished = False
                    continue

                ingredient = next(iter(ingredients))
                if ingredient not in finalIngredients:
                    finalIngredients.add(ingredient)

                    for a, i in allergen_ingredients.items():
                        if a != allergen:
                            i.discard(ingredient)

        result = [(k, *v) for k, v in allergen_ingredients.items()]
        result.sort(key=lambda x: x[0])
        result = ",".join(x[1] for x in result)

        return helper.validate_result('What is your canonical dangerous ingredient list?', result, data.expectedAnswer)


    def solve(self):
        print("Day 21: Allergen Assessment")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))