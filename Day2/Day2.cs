using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day2
{
    public class Solver
    {
        private readonly Logger logger;
        private List<string> input;
        public Solver(string inputFilePath, Logger logger)
        {
            this.logger = logger;
            try
            {
                this.input = ToolBoxClass.GetStringListFromInput(inputFilePath);
            }
            catch (Exception e)
            {
                logger.Error(e, $"There was an error reading the input file.{Environment.NewLine}");
                System.Environment.Exit(1);
            }
        }

        public void Solve()
        {
            logger.Information($"=== Day 2 ===");

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
            logger.Information("PART 1 - Counting the number of passwords that are valid, based on their associated password policy");

            var validPswdCount = 0;
            foreach (var line in this.input)
            {
                // Split the line into the respective interesting parameters
                var bits = line.Split(" ");
                var min = Int32.Parse(bits[0].Split("-").FirstOrDefault());
                var max = Int32.Parse(bits[0].Split("-").LastOrDefault());
                var letter = bits[1].ToCharArray()[0];
                var password = bits[2];

                // Count the occurence of the letter in the password
                var letterCountInPswd = 0;
                foreach (var c in password)
                {
                    if (c == letter)
                    {
                        letterCountInPswd++;
                    }
                }

                // Compare the number of occurences to the min/max values from the password policy
                if (letterCountInPswd >= min && letterCountInPswd <= max)
                {
                    validPswdCount++;
                }
            }

            logger.Information($"There are {validPswdCount} valid passwords !");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Counting the number of passwords that are valid, based on the \"revised\" password policy");

            var validPswdCount = 0;
            foreach (var line in this.input)
            {
                // Split the line into the respective interesting parameters
                var bits = line.Split(" ");
                var pos1 = Int32.Parse(bits[0].Split("-").FirstOrDefault());
                var pos2 = Int32.Parse(bits[0].Split("-").LastOrDefault());
                var letter = bits[1].ToCharArray()[0];
                var password = bits[2];
                var letter1 = password.ToCharArray()[pos1 - 1];
                var letter2 = password.ToCharArray()[pos2 - 1];


                // Verify EXACTLY one of the letters at the described positions match the letter
                if (letter1 == letter ^ letter2 == letter)
                {
                    validPswdCount++;
                }
            }

            logger.Information($"There are {validPswdCount} valid passwords !");
        }
    }
}