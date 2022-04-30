import sys
from typing import List


DIMENSION_X:int = 25
DIMENSION_Y:int = 6
IMAGE_SIZE:int = DIMENSION_X * DIMENSION_Y


def part1(input:List[int]) -> int:
    numLayers = get_number_of_layers(input)
    foundLayer = get_layer_number_with_least(input, numLayers, 0)
    layerData = get_image_layer(input, foundLayer)
    num1s = count_in_layer(layerData, 1)
    num2s = count_in_layer(layerData, 2)
    result = num1s * num2s
    return result


def part2(input:List[int]) -> str:
    numLayers = get_number_of_layers(input)
    result = build_composite_image(input, numLayers)
    return result


def get_number_of_layers(imageData:List[int]):
    numLayers = len(imageData) // IMAGE_SIZE
    return numLayers

def get_layer_number_with_least(imageData:List[int], numLayers:int, value:int) -> int:
    layerNumber = -1
    lowestCount = sys.maxsize
    
    for i in range(numLayers):
        layerData = get_image_layer(imageData, i)
        count = count_in_layer(layerData, value)
        if count < lowestCount:
            layerNumber = i
            lowestCount = count

    return layerNumber

def get_image_layer(imageData:List[int], layerNumber:int) -> List[int]:
    start = layerNumber * IMAGE_SIZE
    end = start + IMAGE_SIZE
    layerData = imageData[start:end]
    return layerData

def count_in_layer(imageData:List[int], value:int) -> int:
    matching = list(filter(
        lambda x: (x == value),
        imageData
    ))
    result = len(matching)
    return result

def build_composite_image(imageData:List[int], numLayers:int) -> str:
    result = ''
    for pixelIndex in range(IMAGE_SIZE):
        pixelchar = get_composite_pixel(imageData, numLayers, pixelIndex)
        result += pixelchar
        if (pixelIndex + 1) % DIMENSION_X == 0 and pixelIndex + 1 < IMAGE_SIZE:
            result += '\n'
    return result

def get_composite_pixel(imageData:List[int], numLayers:int, pixelIndex:int) -> str:
    for layer in range(numLayers):
        index = (layer * IMAGE_SIZE) + pixelIndex
        value = imageData[index]
        match value:
            case 0: return ' '
            case 1: return '#'
    return ''