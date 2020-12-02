﻿using AdventOfCode.Logging;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = CustomLogging.Init("logfile.txt");
            logger.Information("Welcome to Thomas' Advent Of Code 2020 project !");

            // Day 1
            new Day1.Solver("Day1/input.txt", logger).Solve();

            // Day 2
            new Day2.Solver("Day2/input.txt", logger).Solve();
        }
    }
}