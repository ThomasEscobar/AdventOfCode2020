using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;
using System.Linq;

namespace AdventOfCode.Day15
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
            logger.Information($"=== Day 15 ===");

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
            logger.Information("PART 1 - Finding the 2020th number spoken");

            var numberList = new List<int>();
            foreach (var number in this.input.FirstOrDefault().Split(','))
            {
                numberList.Add(Int32.Parse(number));
            }

            for (var i = numberList.Count - 1; i < 2020 - 1; i++)
            {
                numberList.Add(this.FindNextNumber(i, numberList));
            }

            logger.Information($"The 2020th number is {numberList[2020 - 1]} !");
        }

        private void SolvePart2()
        {
            // Took 12hrs or 43827600 ms
            logger.Information("PART 2 - Finding the 30000000th number spoken");

            var lastNumber = 0;
            // Dictionary of {number,lastSeenIndex}
            var numberSpokenDictionary = new Dictionary<int, int>();
            var startingNumberList = this.input.FirstOrDefault().Split(',').Select(x => Int32.Parse(x)).ToList();
            for (int i = 0; i < startingNumberList.Count; i++)
            {
                numberSpokenDictionary.Add(startingNumberList[i], i);
                lastNumber = startingNumberList[i];
            }

            for (var i = numberSpokenDictionary.Count - 1; i < 10 - 1; i++)
            {
                logger.Debug("-----------");
                logger.Debug($"i = {i}; lastNumber = {lastNumber}");
                lastNumber = this.FindNextNumber(i, lastNumber, numberSpokenDictionary);
                logger.Debug($"lastNumber is now = {lastNumber}");
                if (numberSpokenDictionary.Keys.Contains(lastNumber))
                {
                    numberSpokenDictionary[lastNumber] = i;
                }
                else
                {
                    numberSpokenDictionary.Add(lastNumber, i);
                }
                logger.Debug("-----------");
            }

            foreach (var kvp in numberSpokenDictionary)
            {
                logger.Debug($"{kvp.Key} - {kvp.Value}");
            }

            logger.Information($"The 30000000th number is {lastNumber} !");
        }

        private int FindNextNumber(int i, List<int> numberList)
        {
            var lastNumber = numberList[i];
            var lastNumberIndex = numberList.LastIndexOf(lastNumber, i - 1);

            return lastNumberIndex == -1 ? 0 : i - lastNumberIndex;
        }

        private int FindNextNumber(int i, int lastNumber, Dictionary<int, int> numberDictionary)
        {
            logger.Debug("==========");
            foreach (var kvp in numberDictionary)
            {
                logger.Debug($"{kvp.Key} - {kvp.Value}");
            }
            logger.Debug("==========");

            if (numberDictionary.Keys.Contains(lastNumber))
            {
                logger.Debug($"Returning {i - numberDictionary[lastNumber]} (i = {i}, lastNumber = {lastNumber}, numberDictionary[lastNumber] = {numberDictionary[lastNumber]}");
                return i - numberDictionary[lastNumber];
            }
            else
            {
                logger.Debug($"New number, returning 0");
                return 0;
            }
        }
    }
}
