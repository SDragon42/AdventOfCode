from typing import final
import utils
import collections

# ref: https://github.com/BastiHz/Advent_of_Code/blob/main/2020/day_21.py

# Types
IngredientsSet = set[str]
AllergenList = list[str]
AllergenIngredientDict = dict[str, IngredientsSet]
IngredientCounterDict = dict[str, int]


def parse_food_line(line: str) -> tuple[IngredientsSet, AllergenList]:
    parts = line.split(" (contains ")
    ingredients = set(parts[0].split())
    allergens =parts[1].replace(")", "").split(", ")
    return ingredients, allergens


def process_food_lines(foodLineList: list[str]) -> tuple[AllergenIngredientDict, IngredientCounterDict]:
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


def get_ingredients_without_allergens(allergen_ingredients: AllergenIngredientDict, ingredient_counter: IngredientCounterDict) -> set[str]:
    without_allergens = set(ingredient_counter.keys())
    for v in allergen_ingredients.values():
        for allergen in v:
            without_allergens.discard(allergen)
    return without_allergens


def run_part1(title: str, inputList: list[str], correctResult: int):
    allergen_ingredients, ingredient_counter =  process_food_lines(inputList)

    without_allergens = get_ingredients_without_allergens(allergen_ingredients, ingredient_counter)
    result = sum(ingredient_counter[ingredient] for ingredient in without_allergens)

    utils.validate_result(title, result, correctResult)


def run_part2(title: str, inputList: list[str], correctResult: str):
    allergen_ingredients, ingredient_counter =  process_food_lines(inputList)

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

    utils.validate_result(title, result, correctResult)


# utils.showDebug = True
if __name__ == "__main__":
    day = 21
    print(f"---- Day {day}: Allergen Assessment ----")

    run_part1("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        5)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        2262)

    print("---- part 2 ----")

    run_part2("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        "mxmxvkd,sqjhc,fvjkl")
    run_part2("problem",
        utils.read_input_as_list(day, "input"),
        "cxsvdm,glf,rsbxb,xbnmzr,txdmlzd,vlblq,mtnh,mptbpz")