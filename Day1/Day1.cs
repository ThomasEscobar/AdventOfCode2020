using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Serilog.Core;

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
                var inputLines = File.ReadAllLines(inputFilePath);
                this.input = new List<int>();
                foreach (var line in inputLines)
                {
                    this.input.Add(Int32.Parse(line));
                }
            }
            catch (Exception e)
            {
                logger.Error(e, $"There was an error reading the input file.{Environment.NewLine}");
                System.Environment.Exit(1);
            }
        }

        public string Solve()
        {
            var answer = string.Empty;
            this.LogStart(1);

            logger.Information("PART 1 - Now adding the inputs together 2 by 2 to check if the sum is 2020");
            var matchFound = false;
            foreach (var num in this.input)
            {
                if (!matchFound)
                {
                    foreach (var otherNum in this.input.Where(n => !n.Equals(num)))
                    {
                        var sum = num + otherNum;
                        if (sum == 2020)
                        {
                            logger.Information($"{num} + {otherNum} = {sum}");
                            logger.Information("Match found !");
                            matchFound = true;
                            var multiplication = num * otherNum;
                            logger.Information($"{num} * {otherNum} = {multiplication}");
                            answer = multiplication.ToString();
                            break;
                        }
                    }
                }
            }

            logger.Information("PART 2 - Now adding the inputs together 3 by 3 to check if the sum is 2020");
            matchFound = false;
            foreach (var num1 in this.input)
            {
                if (!matchFound)
                {
                    foreach (var num2 in this.input.Where(n => !n.Equals(num1)))
                    {
                        if (!matchFound)
                        {
                            foreach (var num3 in this.input.Where(n => !n.Equals(num1) && !n.Equals(num2)))
                            {
                                var sum = num1 + num2 + num3;
                                if (sum == 2020)
                                {
                                    logger.Information($"{num1} + {num2} + {num3} = {sum}");
                                    logger.Information("Match found !");
                                    matchFound = true;
                                    var multiplication = num1 * num2 * num3;
                                    logger.Information($"{num1} * {num2} * {num3} = {multiplication}");
                                    answer = multiplication.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            this.LogEnd(answer);
            return answer;
        }

        public void LogStart(int dayIndex)
        {
            logger.Information($"=== Day {dayIndex} ===");
            logger.Information("Start...");
        }

        public void LogEnd(string answer)
        {
            logger.Information($"End ! Answer is {answer}");
            logger.Information("=============");
        }
    }
}