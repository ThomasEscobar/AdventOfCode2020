using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day1
{
    public class Solver
    {
        private readonly Logger logger;
        private List<int> input;
        public Solver(string inputFilePath, Logger logger)
        {
            this.logger = logger;
            try
            {
                this.input = ToolBoxClass.GetIntListFromInput(inputFilePath);
            }
            catch (Exception e)
            {
                logger.Error(e, $"There was an error reading the input file.{Environment.NewLine}");
                System.Environment.Exit(1);
            }
        }

        public void Solve()
        {
            logger.Information($"=== Day 1 ===");

            var sw = new Stopwatch();
            sw.Start();

            this.SolvePart1();
            logger.Information($"(Part 1 took {sw.ElapsedMilliseconds} ms)");

            sw.Restart();

            this.SolvePart2();
            logger.Information($"(Part 2 took {sw.ElapsedMilliseconds} ms)");

            logger.Information("=============");
        }

        public void SolvePart1()
        {
            logger.Information("PART 1 - Now adding the inputs together 2 by 2 to check if the sum is 2020");
            foreach (var num in this.input)
            {
                foreach (var otherNum in this.input.Where(n => !n.Equals(num)))
                {
                    var sum = num + otherNum;
                    if (sum == 2020)
                    {
                        logger.Information("Match found !");
                        logger.Information($"{num} + {otherNum} = {sum}");
                        logger.Information($"{num} * {otherNum} = {num * otherNum}");
                        return;
                    }
                }
            }
            logger.Information("Match not found... Something went wrong !");
        }

        public void SolvePart2()
        {
            logger.Information("PART 2 - Now adding the inputs together 3 by 3 to check if the sum is 2020");
            foreach (var num1 in this.input)
            {
                foreach (var num2 in this.input.Where(n => !n.Equals(num1)))
                {
                    foreach (var num3 in this.input.Where(n => !n.Equals(num1) && !n.Equals(num2)))
                    {
                        var sum = num1 + num2 + num3;
                        if (sum == 2020)
                        {
                            logger.Information("Match found !");
                            logger.Information($"{num1} + {num2} + {num3} = {sum}");
                            logger.Information($"{num1} * {num2} * {num3} = {num1 * num2 * num3}");
                            return;
                        }
                    }
                }
            }
            logger.Information("Match not found... Something went wrong !");
        }
    }
}