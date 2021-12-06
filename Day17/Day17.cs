using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day17
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
            logger.Information($"=== Day 17 ===");

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
            logger.Information("PART 1 - Conway's Game of Life 3D");

            // Declaring and initialising the cube (doesn't need to be a cube, could start as a "slice", and grow with Z smaller)
            var cube = this.InitiateCube(this.input.Count, true);

            // var count120 = this.CaculateNewState(1, 2, 0, cube);

            this.PrintSlices(cube);

            for (int i = 0; i < 1; i++)
            {
                cube = this.CalculateNextGeneration(cube);
                logger.Debug($"Cube {i} is {cube.Count} big");
            }

            this.PrintSlices(cube);

            var count = this.CountAlive(cube);
            logger.Information($"Count of alive cells is {count}"); // 11 for step 1 of example
        }

        private void PrintSlices(List<List<List<int>>> cube)
        {
            for (int z = 0; z < cube.Count; z++)
            {
                logger.Debug($"z = {z}");
                for (int y = 0; y < cube.Count; y++)
                {
                    var line = "";
                    for (int x = 0; x < cube.Count; x++)
                    {
                        line += cube[x][y][z];
                    }
                    logger.Debug(line);
                }
            }
        }

        private List<List<List<int>>> InitiateCube(int size, bool firstCube = false)
        {
            List<List<List<int>>> cube = new List<List<List<int>>>();
            for (int x = 0; x < size; x++)
            {
                cube.Add(new List<List<int>>());
                for (int y = 0; y < size; y++)
                {
                    cube[x].Add(new List<int>());
                    for (int z = 0; z < size; z++)
                    {
                        if (firstCube)
                        {
                            if (this.input[y][x] == '#' && z == 1)
                            {
                                cube[x][y].Add(1);
                            }
                            else
                            {
                                cube[x][y].Add(0);
                            }
                        }
                        else
                        {
                            cube[x][y].Add(0);
                        }
                    }
                }
            }

            return cube;
        }

        private int CountAlive(List<List<List<int>>> cube)
        {
            var sum = 0;
            for (int x = 0; x < cube.Count; x++)
            {
                for (int y = 0; y < cube.Count; y++)
                {
                    for (int z = 0; z < cube.Count; z++)
                    {
                        sum += cube[x][y][z];
                    }
                }
            }

            return sum;
        }

        private List<List<List<int>>> CalculateNextGeneration(List<List<List<int>>> cube)
        {
            var newCube = this.InitiateCube(cube.Count + 2);

            for (int x = 0; x < newCube.Count; x++)
            {
                for (int y = 0; y < newCube.Count; y++)
                {
                    for (int z = 0; z < newCube.Count; z++)
                    {
                        newCube[x][y][z] = this.CaculateNewState(x - 1, y - 1, z - 1, cube);
                    }
                }
            }

            return newCube;
        }

        private int CaculateNewState(int X, int Y, int Z, List<List<List<int>>> cube)
        {
            var neighbours = 0;
            int currentState;
            try
            {
                currentState = cube[X][Y][Z];
            }
            catch
            {
                // Cell that was previously out of bounds is considered dead
                currentState = 0;
            }
            for (int x = X - 1; x <= X + 1; x++)
            {
                for (int y = Y - 1; y <= Y + 1; y++)
                {
                    for (int z = Z - 1; z <= Z + 1; z++)
                    {
                        try
                        {
                            neighbours += cube[x][y][z];
                        }
                        catch
                        {
                            // Ignore out of bounds
                        }
                    }
                }
            }


            var newState = 0;
            if (currentState == 1 && (neighbours == 3 || neighbours == 4))
            {
                newState = 1;
            }
            else if (currentState == 0 && neighbours == 4)
            {
                newState = 1;
            }

            logger.Debug($"The new state of ({X},{Y},{Z}) is {newState} because it was {currentState} and has {neighbours} neighbours");
            return newState;
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - ");

        }
    }
}