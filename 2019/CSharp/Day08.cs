﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/8
    /// </summary>
    class Day08 : PuzzleBase
    {
        public Day08(bool benchmark) : base(benchmark) { }

        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 8: Space Image Format";

            yield return string.Empty;
            yield return "Part 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "input")));

            yield return string.Empty;
            yield return "Part 2) " + base.Run(() => RunPart2(GetPuzzleData(2, "input")));
        }


        class InputAnswer : InputAnswer<string, int?>
        {
            public InputAnswer(string input, int? expectedAnswer1 = null, string expectedAnswer2 = null)
            {
                Input = input;
                ExpectedAnswer = expectedAnswer1;
                ExpectedAnswer2 = expectedAnswer2;

                ImageData = Input.Select(c => (int)char.GetNumericValue(c)).ToList();
            }

            public List<int> ImageData { get; private set; }
            public string ExpectedAnswer2 { get; private set; }
        }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 8;

            var result = part switch
            {
                1 => new InputAnswer(
                    InputHelper.LoadInputFile(DAY, name),
                    expectedAnswer1: InputHelper.LoadAnswerFile(DAY, part, name).ToInt32()
                    ),
                2 => new InputAnswer(
                    InputHelper.LoadInputFile(DAY, name),
                    expectedAnswer2: InputHelper.LoadAnswerFile(DAY, part, name)
                    ),
                _ => throw new ApplicationException($"Invalid part ({part}) value")
            };
            return result;
        }

        const int DimensionX = 25;
        const int DimensionY = 6;
        const int ImageSize = DimensionX * DimensionY;


        string RunPart1(InputAnswer puzzleData)
        {
            var foundLayer = GetFindLayerNumberWithLeast(puzzleData.ImageData, 0);

            var layerData = GetImageLayer(puzzleData.ImageData, foundLayer);
            var numOf1s = CountInLayer(layerData, 1);
            var numOf2s = CountInLayer(layerData, 2);

            var result = numOf1s * numOf2s;

            return Helper.GetPuzzleResultText($"The number of 1 digits multiplied by the number of 2 digits: {result}", result, puzzleData.ExpectedAnswer);
        }

        string RunPart2(InputAnswer puzzleData)
        {
            var compositeImage = buildCompositImage(puzzleData.ImageData);

            return Helper.GetPuzzleResultText("\r\n"+compositeImage, compositeImage, puzzleData.ExpectedAnswer2);
        }

        int GetFindLayerNumberWithLeast(IList<int> imageData, int value)
        {
            var layerNumber = -1;
            var lowestCount = int.MaxValue;
            var numLayers = imageData.Count / ImageSize;
            for (var i = 0; i < numLayers; i++)
            {
                var layerData = GetImageLayer(imageData, i);
                var count = CountInLayer(layerData, value);
                if (count < lowestCount)
                {
                    layerNumber = i;
                    lowestCount = count;
                }
            }

            return layerNumber;
        }

        IList<int> GetImageLayer(IList<int> imageData, int layerNumber)
        {
            var layerData = imageData
                .Skip(layerNumber * ImageSize)
                .Take(ImageSize)
                .ToList();
            return layerData;
        }
        int CountInLayer(IList<int> layerData, int value)
        {
            var result = layerData.Where(v => v == value).Count();
            return result;
        }

        string buildCompositImage(IList<int> imageData)
        {
            var sb = new StringBuilder();
            for (var pixel = 0; pixel < ImageSize; pixel++)
            {
                var pixelChar = GetCompositedPixel(imageData, pixel);
                sb.Append(pixelChar);
                if (((pixel + 1) % DimensionX == 0) && (pixel + 1 < ImageSize))
                    sb.AppendLine();
            }
            return sb.ToString();
        }


        char GetCompositedPixel(IList<int> imageData, int pixel)
        {
            var numLayers = imageData.Count / ImageSize;
            for (var layer = 0; layer < numLayers; layer++)
            {
                var idx = (layer * ImageSize) + pixel;
                var value = imageData[idx];
                switch (value)
                {
                    case 0: return ' ';
                    case 1: return '#';
                    default: break;
                }
            }

            return ' ';
        }

    }
}