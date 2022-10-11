# ref: https://github.com/BastiHz/Advent_of_Code/blob/main/2020/day_21.py

from typing import List, Dict, Set, Tuple



# Types
IngredientsSet = Set[str]
AllergenList = List[str]
AllergenIngredientDict = Dict[str, IngredientsSet]
IngredientCounterDict = Dict[str, int]



def parse_food_line(line: str) -> Tuple[IngredientsSet, AllergenList]:
    parts = line.split(" (contains ")
    ingredients = set(parts[0].split())
    allergens =parts[1].replace(")", "").split(", ")
    return ingredients, allergens


def process_food_lines(foodLineList: List[str]) -> Tuple[AllergenIngredientDict, IngredientCounterDict]:
    allergen_ingredients: AllergenIngredientDict = {}
    ingredient_counter: IngredientCounterDict = {}

    for foodLine in foodLineList:
        ingredients, allergens = parse_food_line(foodLine)

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


def get_ingredients_without_allergens(allergen_ingredients: AllergenIngredientDict, ingredient_counter: IngredientCounterDict) -> Set[str]:
    without_allergens = set(ingredient_counter.keys())
    for v in allergen_ingredients.values():
        for allergen in v:
            without_allergens.discard(allergen)
    return without_allergens


def run_part1(input:List[str]) -> str:
    allergen_ingredients, ingredient_counter = process_food_lines(input)

    without_allergens = get_ingredients_without_allergens(allergen_ingredients, ingredient_counter)
    result = sum(ingredient_counter[ingredient] for ingredient in without_allergens)
    result = str(result)

    return result


def run_part2(input:List[str]) -> str:
    allergen_ingredients, ingredient_counter = process_food_lines(input)

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

    return result
