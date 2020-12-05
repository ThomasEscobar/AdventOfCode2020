using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day5
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
            logger.Information($"=== Day 5 ===");

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
            logger.Information("PART 1 - Finding highest seat ID on a boarding pass");

            var maxId = 0;

            foreach (var line in this.input)
            {
                var letterRows = line.Substring(0, 7);
                var letterColumns = line.Substring(7, 3);
                var row = this.FindDichotomyResult(0, 127, letterRows);
                var column = this.FindDichotomyResult(0, 7, letterColumns);
                var id = this.CalculateSeatId(row, column);

                if (id > maxId) maxId = id;
            }

            logger.Information($"The highest seat ID is: {maxId}");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Finding the ID of the empty seat");

            var listPossibleIds = new List<int>();
            for (int i = 0; i < 1024; i++)
            {
                listPossibleIds.Add(i);
            }

            foreach (var line in this.input)
            {
                var letterRows = line.Substring(0, 7);
                var letterColumns = line.Substring(7, 3);
                var row = this.FindDichotomyResult(0, 127, letterRows);
                var column = this.FindDichotomyResult(0, 7, letterColumns);
                var id = this.CalculateSeatId(row, column);

                listPossibleIds.Remove(id);
            }

            for (int i = 0; i < 1024; i++)
            {
                if (listPossibleIds[i + 1] != listPossibleIds[i] + 1)
                {
                    logger.Information($"The seat of your ID (empty seat, not surrounded by empty seats) is: {listPossibleIds[i + 1]}");
                    return;
                }
            }
        }


        private int CalculateSeatId(int row, int column)
        {
            return row * 8 + column;
        }

        private int[] Dichotomy(int min, int max, char letter)
        {
            var boundaries = new int[2];
            // Keep lower half
            if (letter == 'F' || letter == 'L')
            {
                boundaries[0] = min;
                boundaries[1] = (min + max) / 2; // Integer division, then round down
            }
            // Keep upper half
            else if (letter == 'B' || letter == 'R')
            {
                boundaries[0] = (min + max) / 2 + (min + max) % 2; // Integer division, then round up
                boundaries[1] = max;
            }
            else
            {
                throw new Exception($"Unexpected letter: {letter}");
            }

            return boundaries;
        }

        private int FindDichotomyResult(int min, int max, string letters)
        {
            foreach (var letter in letters)
            {
                var newBoundaries = Dichotomy(min, max, letter);
                min = newBoundaries[0];
                max = newBoundaries[1];
            }

            if (min == max)
            {
                return min;
            }
            else
            {
                throw new Exception($"Dichotomy didn't work, couldn't find a result: min = {min} ; max = {max}");
            }
        }
    }
}