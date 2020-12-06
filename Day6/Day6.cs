using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day6
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
            logger.Information($"=== Day 6 ===");

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
            logger.Information("PART 1 - Calculating the sum of the questions anyone answered yes for all groups");

            var sum = 0;
            var groups = this.CreateGroups(this.input);
            foreach (var group in groups)
            {
                sum += this.CountUniqueLetters(group);
            }

            logger.Information($"The sum of the counts is {sum} !");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Calculating the sum of the questions EVERYONE answered yes for all groups");

            var sum = 0;
            var groups = this.CreateGroups(this.input, true);
            foreach (var group in groups)
            {
                sum += this.CountCommonLetters(group.Split('-').ToList());
            }

            logger.Information($"The sum of the counts is {sum} !");
        }

        private int CountUniqueLetters(string text)
        {
            var uniqueLetterList = new List<char>();
            foreach (var c in text)
            {
                if (!uniqueLetterList.Contains(c) && !c.Equals('-'))
                {
                    uniqueLetterList.Add(c);
                }
            }
            return uniqueLetterList.Count;
        }

        private List<string> CreateGroups(List<string> lines, bool separator = false)
        {
            var groups = new List<string>();
            var tempGroup = string.Empty;
            foreach (var line in lines)
            {
                if (!line.Equals(string.Empty))
                {
                    tempGroup += (separator && !tempGroup.Equals(string.Empty)) ? $"-{line}" : line;
                    if (line.Equals(lines.LastOrDefault()))
                    {
                        groups.Add(tempGroup);
                    }
                }
                else
                {
                    groups.Add(tempGroup);
                    tempGroup = string.Empty;
                }
            }
            return groups;
        }

        private int CountCommonLetters(List<string> lines)
        {
            // Save valid letters
            var validLetters = lines.FirstOrDefault().ToList();
            var lettersToRemove = new List<char>();
            lines.RemoveAt(0);

            // Find letters to remove, that are not in common with the valid letters
            foreach (var otherLine in lines)
            {
                foreach (var c in validLetters)
                {
                    if (!otherLine.Contains(c) && !lettersToRemove.Contains(c))
                    {
                        lettersToRemove.Add(c);
                    }
                }
            }

            // Remove the letters that are not in common
            foreach (var letter in lettersToRemove)
            {
                validLetters.Remove(letter);
            }

            return validLetters.Count;
        }
    }
}