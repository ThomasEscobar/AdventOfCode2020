using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;
using System.Linq;

namespace AdventOfCode.Day13
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
            logger.Information($"=== Day 13 ===");

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
            logger.Information("PART 1 - Finding the earliest bus we can take and how long we have to wait for it");

            var target = Int32.Parse(this.input[0]);
            var busIds = this.input[1].Split(',').ToList().Where(t => !t.Equals("x")).Select(x => Int32.Parse(x)).ToList();

            var min = Int32.MaxValue;
            var minBusId = 0;
            foreach (var busId in busIds)
            {
                var timestamp = 0;
                while (timestamp < target)
                {
                    timestamp += busId;
                }
                if (timestamp < min)
                {
                    min = timestamp;
                    minBusId = busId;
                }
            }

            logger.Information($"The closest timestamp to the target is bus {minBusId}, which will arrive at {min}, making us wait {min - target} minutes. The multiplication result is {minBusId * (min - target)} !");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Find the earliest timestamp such that all bus IDs depart at offests matching their positions");

            // Includes the Xs to get the correct position in the list
            var busIds = this.input[1].Split(',').ToList();
            var busIdsAndPositions = new Dictionary<int, int>();

            // Save a dictionnary with each busId and its position in the list
            for (int i = 0; i < busIds.Count; i++)
            {
                if (!busIds[i].Equals("x"))
                {
                    busIdsAndPositions.Add(Int32.Parse(busIds[i]), i);
                }
            }

            var done = false;
            long timestamp = 0;
            var validBusCount = 1;
            long step = busIdsAndPositions.FirstOrDefault().Key;
            while (!done && timestamp < 10000000000000000)
            {
                // Go through all the possible timestamps, with an increasing step as we validate each busIds
                timestamp += step;

                // Check if that timestamp validates the next busId. If it does, change the step from that point onward to keep validating that busId (start skiping the timestamps that don't validate that busId)
                if ((timestamp + busIdsAndPositions.ElementAt(validBusCount).Value) % busIdsAndPositions.ElementAt(validBusCount).Key == 0)
                {
                    step *= busIdsAndPositions.ElementAt(validBusCount).Key;
                    validBusCount++;
                }

                if (validBusCount == busIdsAndPositions.Count) done = true;
            }

            logger.Information($"The first timestamp that meets the condition is {timestamp}");
        }
    }
}