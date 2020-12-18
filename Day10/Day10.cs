using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day10
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
            logger.Information($"=== Day 10 ===");

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
            logger.Information("PART 1 - ");

            var list = new List<int>();
            list = this.input.ConvertAll<int>(n => n);
            var oneDifCount = 0;
            var threeDifCount = 0;

            list.Add(0);
            list.Add(list.Max() + 3);
            list.Sort();

            for (int i = 0; i < list.Count - 1; i++)
            {
                var dif = list[i + 1] - list[i];
                if (dif == 1)
                {
                    oneDifCount++;
                }
                else if (dif == 3)
                {
                    threeDifCount++;
                }
            }

            logger.Information($"{oneDifCount} * {threeDifCount} = {oneDifCount * threeDifCount}");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - ");

            var list = new List<int>();
            list = this.input.ConvertAll<int>(n => n);

            var listBranchCount = new List<int>();

            list.Add(0);
            list.Add(list.Max() + 3);
            list.Sort();

            // Go through the list to count the number of branches for each number
            for (int i = 0; i < list.Count; i++)
            {
                var branchCount = 0;
                try
                {
                    if (list[i + 1] <= list[i] + 3)
                    {
                        branchCount++;
                    }
                    if (list[i + 2] <= list[i] + 3)
                    {
                        branchCount++;
                    }
                    if (list[i + 3] <= list[i] + 3)
                    {
                        branchCount++;
                    }
                }
                catch
                {
                    // Ignore out of bound exception
                }

                listBranchCount.Add(branchCount);
            }

            // Calculate the total number of branches, one "section" of contiguous numbers at a time
            long totalBranchCount = 1;
            var contiguousDictionary = new Dictionary<int, int>();
            var endOfSectionIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                // Check the following numbers to see if there are contiguous numbers
                if (listBranchCount[i] == 2)
                {
                    if (listBranchCount[i + 1] != 1 || listBranchCount[i + 2] != 1)
                    {
                        AddToDictionaryUnique(contiguousDictionary, list[i], listBranchCount[i]);
                        AddToDictionaryUnique(contiguousDictionary, list[i + 1], listBranchCount[i + 1]);
                        AddToDictionaryUnique(contiguousDictionary, list[i + 2], listBranchCount[i + 2]);
                        endOfSectionIndex = i + 2;
                    }
                    // If it's on it's own, multiply the number of total branches by that number of branches (2)
                    else if (!contiguousDictionary.Keys.Contains(list[i]))
                    {
                        totalBranchCount *= 2;
                    }
                }
                else if (listBranchCount[i] == 3)
                {
                    if (listBranchCount[i + 1] != 1 || listBranchCount[i + 2] != 1 || listBranchCount[i + 3] != 1)
                    {
                        AddToDictionaryUnique(contiguousDictionary, list[i], listBranchCount[i]);
                        AddToDictionaryUnique(contiguousDictionary, list[i + 1], listBranchCount[i + 1]);
                        AddToDictionaryUnique(contiguousDictionary, list[i + 2], listBranchCount[i + 2]);
                        AddToDictionaryUnique(contiguousDictionary, list[i + 3], listBranchCount[i + 3]);
                        endOfSectionIndex = i + 3;
                    }
                    // If it's on it's own, multiply the number of total branches by that number of branches (3)
                    else if (!contiguousDictionary.Keys.Contains(list[i]))
                    {
                        totalBranchCount *= 3;
                    }
                }
                // If this "1" marks the end of a section, calculate the branch count on the section, reset the contiguous dictionary and calculate the new total
                else if (contiguousDictionary.Any() && i == endOfSectionIndex)
                {
                    var sectionBranchCount = this.CalculateSectionBranchCount(contiguousDictionary);
                    contiguousDictionary.Clear();
                    totalBranchCount *= sectionBranchCount;
                    endOfSectionIndex = 0;
                }
            }

            logger.Information($"Total branch count is {totalBranchCount}");
        }

        public void AddToDictionaryUnique(Dictionary<int, int> dictionary, int key, int value)
        {
            // Only add the number to the list if it's not already in the list
            if (!dictionary.Keys.Contains(key))
            {
                dictionary.Add(key, value);
            }
        }

        private int CalculateSectionBranchCount(Dictionary<int, int> contiguousDictionary)
        {
            return this.CalculateNumberBranchCount(contiguousDictionary, contiguousDictionary.FirstOrDefault());
        }

        private int CalculateNumberBranchCount(Dictionary<int, int> contiguousDictionary, KeyValuePair<int, int> keyValuePair)
        {
            int numberTotal = 0;
            try
            {
                if (keyValuePair.Value == 1)
                {
                    numberTotal = 1;
                }
                else if (keyValuePair.Value == 2)
                {
                    var currentNumberIndex = contiguousDictionary.Keys.ToList().IndexOf(keyValuePair.Key);

                    numberTotal += this.CalculateNumberBranchCount(contiguousDictionary, contiguousDictionary.ElementAt(currentNumberIndex + 1));
                    numberTotal += this.CalculateNumberBranchCount(contiguousDictionary, contiguousDictionary.ElementAt(currentNumberIndex + 2));
                }
                else if (keyValuePair.Value == 3)
                {
                    var currentNumberIndex = contiguousDictionary.Keys.ToList().IndexOf(keyValuePair.Key);

                    numberTotal += this.CalculateNumberBranchCount(contiguousDictionary, contiguousDictionary.ElementAt(currentNumberIndex + 1));
                    numberTotal += this.CalculateNumberBranchCount(contiguousDictionary, contiguousDictionary.ElementAt(currentNumberIndex + 2));
                    numberTotal += this.CalculateNumberBranchCount(contiguousDictionary, contiguousDictionary.ElementAt(currentNumberIndex + 3));
                }
            }
            catch
            {
                // Ignore out of bound exception
            }

            return numberTotal;
        }
    }
}