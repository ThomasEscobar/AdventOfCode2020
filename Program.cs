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

            // Day9
            new Day9.Solver("Day9/input.txt", logger).Solve();

            // Day10
            new Day10.Solver("Day10/input.txt", logger).Solve();

            // Day11
            new Day11.Solver("Day11/input.txt", logger).Solve();

            // Day12
            new Day12.Solver("Day12/input.txt", logger).Solve();

            // Day13
            new Day13.Solver("Day13/input.txt", logger).Solve();

            // Day14 (commented as it takes 12s for part 2)
            // new Day14.Solver("Day14/input.txt", logger).Solve(); 

            // Day15
            new Day15.Solver("Day15/input.txt", logger).Solve();

            // Day16
            new Day16.Solver("Day16/input.txt", logger).Solve();

            // // Day17 (commented as not finished yet, on a break)
            // new Day17.Solver("Day17/example.txt", logger).Solve();

            // Day22
            new Day22.Solver("Day22/example.txt", logger).Solve();

            // // DayX
            // new DayX.Solver("DayX/input.txt", logger).Solve();

            // ...

            // Day25
            new Day25.Solver("Day25/input.txt", logger).Solve();
        }
    }
}