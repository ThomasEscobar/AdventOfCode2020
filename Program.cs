using System;
using AdventOfCode.Logging;
using AdventOfCode.Day1;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = CustomLogging.Init("logfile.txt");
            logger.Information("Welcome to Thomas' Advent Of Code 2020 project !");

            var day1Solver = new Solver("Day1/input.txt", logger);
            day1Solver.Solve();
        }
    }
}