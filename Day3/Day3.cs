using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;
using System.IO;

namespace AdventOfCode.Day3
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
            logger.Information($"=== Day 3 ===");

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
            logger.Information("PART 1 - Going with a \"right 3, down 1\" slope, counting how many trees would be hit on the way to the bottom");

            var width = this.input.FirstOrDefault().Length;
            var length = this.input.Count;
            var treeCounter = 0;
            var output = new List<string>();

            for (int y = 0; y < length; y++)
            {
                var line = this.input[y];
                var x = (y * 3) % width;
                var squares = line.ToCharArray();
                if (squares[x] == '#')
                {
                    treeCounter++;
                    squares[x] = 'X';
                }
                else
                {
                    squares[x] = 'O';
                }
                output.Add(new string(squares));
            }

            logger.Information($"The number of trees encountered is: {treeCounter}");
            File.WriteAllLines("Day3/output.txt", output);
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Testing all the listed slopes, calculating the multiplication of the numbers of trees encountered for each slope");

            var width = this.input.FirstOrDefault().Length;
            var length = this.input.Count;

            var slopeList = new List<int[]>();
            slopeList.Add(new int[] { 1, 1 });
            slopeList.Add(new int[] { 3, 1 });
            slopeList.Add(new int[] { 5, 1 });
            slopeList.Add(new int[] { 7, 1 });
            slopeList.Add(new int[] { 1, 2 });
            double total = 1;

            foreach (var slope in slopeList)
            {
                var treeCounter = 0;
                var output = new List<string>();
                for (int y = 0; y < length; y++)
                {
                    var line = this.input[y];
                    var x = (y * slope[0] / slope[1]) % width;
                    var squares = line.ToCharArray();
                    if (y % slope[1] == 0)
                    {
                        if (squares[x] == '#')
                        {
                            treeCounter++;
                            squares[x] = 'X';
                        }
                        else
                        {
                            squares[x] = 'O';
                        }
                    }
                    output.Add(new string(squares));
                }
                logger.Information($"For the slope ({slope[0]},{slope[1]}), the number of trees encountered is: {treeCounter}");
                total = total * treeCounter;
                File.WriteAllLines($"Day3/output_slope{slope[0]}-{slope[1]}.txt", output);
            }

            logger.Information($"The total is {total}");
        }
    }
}