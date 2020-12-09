using AdventOfCode.Logging;

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

            // Day 3
            new Day3.Solver("Day3/input.txt", logger).Solve();

            // Day 4
            new Day4.Solver("Day4/input.txt", logger).Solve();

            // Day 5
            new Day5.Solver("Day5/input.txt", logger).Solve();

            // Day 6
            new Day6.Solver("Day6/input.txt", logger).Solve();

            // Day 7
            new Day7.Solver("Day7/input.txt", logger).Solve();

            // Day8
            new Day8.Solver("Day8/input.txt", logger).Solve();
        }
    }
}