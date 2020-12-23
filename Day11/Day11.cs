using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;


namespace AdventOfCode.Day11
{
    public class Solver
    {
        private readonly Logger logger;
        private List<string> input;
        private bool stabilized;
        private bool genFile = false;
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
            this.stabilized = true;
        }

        public void Solve()
        {
            logger.Information($"=== Day 11 ===");

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
            logger.Information("PART 1 - Finding the number of occupied seats once the layout has stabilized");

            var finalGeneration = this.FindStabilizedGeneration();
            var occupiedSeatCount = 0;
            foreach (var line in finalGeneration)
            {
                foreach (var character in line)
                {
                    if (character == '#') occupiedSeatCount++;
                }
            }

            logger.Information($"They are {occupiedSeatCount} seats occupied on the last generation");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Finding the number of occupied seats on the last generation using the new visibility model");

            var finalGeneration = this.FindStabilizedGeneration(true);
            var occupiedSeatCount = 0;
            foreach (var line in finalGeneration)
            {
                foreach (var character in line)
                {
                    if (character == '#') occupiedSeatCount++;
                }
            }

            logger.Information($"They are {occupiedSeatCount} seats occupied on the last generation with the new visibility model");

            // var testResult = this.CountAdjacentOccupiedSeats(1, 1, this.input, true);
            // logger.Information($"The test result is {testResult}");
        }

        private char GetNewSeatState(int seatX, int seatY, List<string> previousGeneration, bool advancedModel = false)
        {
            var adjacentSeats = this.CountAdjacentOccupiedSeats(seatX, seatY, previousGeneration, advancedModel);
            var thresholdOccupiedSeats = advancedModel ? 5 : 4;
            var currentState = previousGeneration[seatY][seatX];
            if (adjacentSeats == 0 && currentState == 'L')
            {
                this.stabilized = false;
                return '#';
            }
            else if (adjacentSeats >= thresholdOccupiedSeats && currentState == '#')
            {
                this.stabilized = false;
                return 'L';
            }
            else
            {
                return currentState;
            }
        }

        private int CountAdjacentOccupiedSeats(int seatX, int seatY, List<string> previousGeneration, bool advancedModel = false)
        {
            var adjacentOccupiedSeatCount = 0;

            // Part 1 - Counting adjacent occupied seats
            if (!advancedModel)
            {
                for (int y = seatY - 1; y <= seatY + 1; y++)
                {
                    for (int x = seatX - 1; x <= seatX + 1; x++)
                    {
                        if (x == seatX && y == seatY)
                        {
                            // Skip the seat coordonates
                            continue;
                        }
                        if (x < 0 || y < 0 || x >= previousGeneration[0].Length || y >= previousGeneration.Count)
                        {
                            // Skip out of bounds coordonates
                            continue;
                        }
                        if (previousGeneration[y][x] == '#')
                        {
                            adjacentOccupiedSeatCount++;
                        }
                    }
                }
            }
            // Part 2 - Counting visible occupied seats
            else
            {
                // Direction N - y decreases
                for (int y = seatY; y >= 0; y--)
                {
                    if (y == seatY)
                    {
                        // Skip the seat coordonates
                        continue;
                    }
                    if (previousGeneration[y][seatX] == 'L')
                    {
                        break;
                    }
                    if (previousGeneration[y][seatX] == '#')
                    {
                        adjacentOccupiedSeatCount++;
                        break;
                    }
                }

                // Direction S - y increases
                for (int y = seatY; y < previousGeneration.Count; y++)
                {
                    if (y == seatY)
                    {
                        // Skip the seat coordonates
                        continue;
                    }
                    if (previousGeneration[y][seatX] == 'L')
                    {
                        break;
                    }
                    if (previousGeneration[y][seatX] == '#')
                    {
                        adjacentOccupiedSeatCount++;
                        break;
                    }
                }

                // Direction E - x increases
                for (int x = seatX; x < previousGeneration[0].Length; x++)
                {
                    if (x == seatX)
                    {
                        // Skip the seat coordonates
                        continue;
                    }
                    if (previousGeneration[seatY][x] == 'L')
                    {

                        break;
                    }
                    if (previousGeneration[seatY][x] == '#')
                    {
                        adjacentOccupiedSeatCount++;
                        break;
                    }
                }

                // Direction W - x decreases
                for (int x = seatX; x >= 0; x--)
                {
                    if (x == seatX)
                    {
                        // Skip the seat coordonates
                        continue;
                    }
                    if (previousGeneration[seatY][x] == 'L')
                    {
                        break;
                    }
                    if (previousGeneration[seatY][x] == '#')
                    {
                        adjacentOccupiedSeatCount++;
                        break;
                    }
                }

                // Direction NE - y decreases, x increases
                var i = 1;
                while (true)
                {
                    var x = seatX + i;
                    var y = seatY - i;

                    // Stop the loop if we reach an edge
                    if (x < 0 || y < 0 || x >= previousGeneration[0].Length || y >= previousGeneration.Count)
                    {
                        break;
                    }

                    // Stop the loop if we find a seat
                    if (previousGeneration[y][x] == 'L')
                    {
                        break;
                    }
                    if (previousGeneration[y][x] == '#')
                    {
                        adjacentOccupiedSeatCount++;
                        break;
                    }

                    // Just in case
                    if (i > 100)
                    {
                        logger.Error($"You shouldn't be here...");
                        break;
                    }

                    i++;
                }

                // Direction SE - y increases, x increases
                i = 1;
                while (true)
                {
                    var x = seatX + i;
                    var y = seatY + i;

                    // Stop the loop if we reach an edge
                    if (x < 0 || y < 0 || x >= previousGeneration[0].Length || y >= previousGeneration.Count)
                    {
                        break;
                    }

                    // Stop the loop if we find a seat
                    if (previousGeneration[y][x] == 'L')
                    {
                        break;
                    }
                    if (previousGeneration[y][x] == '#')
                    {
                        adjacentOccupiedSeatCount++;
                        break;
                    }

                    // Just in case
                    if (i > 100)
                    {
                        logger.Error($"You shouldn't be here...");
                        break;
                    }

                    i++;
                }

                // Direction SW - y increases, x decreases
                i = 1;
                while (true)
                {
                    var x = seatX - i;
                    var y = seatY + i;

                    // Stop the loop if we reach an edge
                    if (x < 0 || y < 0 || x >= previousGeneration[0].Length || y >= previousGeneration.Count)
                    {
                        break;
                    }

                    // Stop the loop if we find a seat
                    if (previousGeneration[y][x] == 'L')
                    {
                        break;
                    }
                    if (previousGeneration[y][x] == '#')
                    {
                        adjacentOccupiedSeatCount++;
                        break;
                    }

                    // Just in case
                    if (i > 100)
                    {
                        logger.Error($"You shouldn't be here...");
                        break;
                    }

                    i++;
                }

                // Direction NW - y decreases, x decreases
                i = 1;
                while (true)
                {
                    var x = seatX - i;
                    var y = seatY - i;

                    // Stop the loop if we reach an edge
                    if (x < 0 || y < 0 || x >= previousGeneration[0].Length || y >= previousGeneration.Count)
                    {
                        break;
                    }

                    // Stop the loop if we find a seat
                    if (previousGeneration[y][x] == 'L')
                    {
                        break;
                    }
                    if (previousGeneration[y][x] == '#')
                    {
                        adjacentOccupiedSeatCount++;
                        break;
                    }

                    // Just in case
                    if (i > 100)
                    {
                        logger.Error($"You shouldn't be here...");
                        break;
                    }

                    i++;
                }
            }
            return adjacentOccupiedSeatCount;
        }

        private List<string> CalculateNextGeneration(List<String> previousGeneration, bool advancedModel = false)
        {
            var newGeneration = new List<string>();

            for (int y = 0; y < previousGeneration.Count; y++)
            {
                var newLine = string.Empty;
                for (int x = 0; x < previousGeneration[0].Length; x++)
                {
                    newLine += this.GetNewSeatState(x, y, previousGeneration, advancedModel);
                }
                newGeneration.Add(newLine);
            }

            return newGeneration;
        }

        private List<string> FindStabilizedGeneration(bool advancedModel = false)
        {
            var currentGeneration = this.input;
            var i = 1;

            do
            {
                this.stabilized = true;
                currentGeneration = this.CalculateNextGeneration(currentGeneration, advancedModel);
                if (this.genFile && advancedModel)
                {
                    var partNumber = advancedModel ? "2" : "1";
                    File.WriteAllLines($"Day11/Part{partNumber}/part{partNumber}_generation{i}.txt", currentGeneration);
                }
                i++;
            } while (stabilized != true && i <= 100);

            return currentGeneration;
        }
    }
}