using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day9
{
    public class Solver
    {
        private readonly Logger logger;
        private List<double> input;
        public Solver(string inputFilePath, Logger logger)
        {
            this.logger = logger;
            try
            {
                this.input = ToolBoxClass.GetDoubleListFromInput(inputFilePath);
            }
            catch (Exception e)
            {
                logger.Error(e, $"There was an error reading the input file.{Environment.NewLine}");
                System.Environment.Exit(1);
            }
        }

        public void Solve()
        {
            logger.Information($"=== Day 9 ===");

            var sw = new Stopwatch();
            sw.Start();

            this.SolvePart1();
            logger.Information($"(Part 1 took {sw.ElapsedMilliseconds} ms)");

            sw.Restart();

            this.SolvePart2();
            logger.Information($"(Part 2 took {sw.ElapsedMilliseconds} ms)");

            logger.Information("=============");
        }

        private void SolvePart1()
        {
            logger.Information("PART 1 - Finding the first number that isn't the sum of two of the previous 25 numbers");

            var preamble = this.input.GetRange(0, 25);
            for (int i = 25; i < this.input.Count; i++)
            {
                var currentNumber = this.input[i];
                if (this.CheckIfValid(currentNumber, preamble))
                {
                    preamble.RemoveAt(0);
                    preamble.Add(currentNumber);
                }
                else
                {
                    logger.Information($"The invalid number is {currentNumber}");
                    return;
                }
            }
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Finding the contiguous set that adds up to the answer from part 1");

            double answerPart1 = 375054920;
            for (int i = 0; i < this.input.Count; i++)
            {
                var result = this.CheckIfContiguousSumIsValid(i, this.input, answerPart1);
                if (result.valid == true)
                {
                    logger.Information($"We found the contiguous set that adds up to {answerPart1} ! The sum of the smallest and largest is {result.contiguousList.Min() + result.contiguousList.Max()}");
                    foreach (var x in result.contiguousList)
                    {
                        logger.Information(x.ToString());
                    }
                    return;
                }
            }
        }

        private bool CheckIfValid(double number, List<double> validNumbers)
        {
            foreach (var validNumber in validNumbers)
            {
                var target = number - validNumber;
                if (validNumbers.Contains(target))
                {
                    return true;
                }
            }
            return false;
        }

        private Result CheckIfContiguousSumIsValid(int index, List<double> numberList, double target)
        {
            double sum = 0;
            var contiguousList = new List<double>();
            var valid = false;

            for (int i = index; i < numberList.Count; i++)
            {
                contiguousList.Add(numberList[i]);
                sum += numberList[i];
                if (sum == target)
                {
                    valid = true;
                    break;
                }
                else if (sum > target)
                {
                    valid = false;
                    break;
                }
            }

            return new Result { contiguousList = contiguousList, valid = valid };
        }

        public class Result
        {
            public List<double> contiguousList = new List<double>();
            public bool valid = false;
        }
    }
}