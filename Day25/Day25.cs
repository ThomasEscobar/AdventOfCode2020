using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day25
{
    public class Solver
    {
        private readonly Logger logger;
        private List<string> input;
        private int subject;
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
            this.subject = 7;
        }

        public void Solve()
        {
            logger.Information($"=== Day 25 ===");

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
            logger.Information("PART 1 - Finding the encryption key established by the handshake between the door and the card");

            var doorPublicKey = Int64.Parse(this.input[0]);
            var cardPublicKey = Int64.Parse(this.input[1]);
            long encryptionKey = 0;

            var doorLoopSize = this.FindLoopSizeOptimized(this.subject, doorPublicKey, 100000000); // Times out at 100000
            if (doorLoopSize != -1)
            {
                encryptionKey = this.TransformNumber(cardPublicKey, doorLoopSize);
            }
            else
            {
                var cardLoopSize = this.FindLoopSizeOptimized(this.subject, cardPublicKey, 100000000); // Times out at 100000
                if (cardLoopSize != -1)
                {
                    encryptionKey = this.TransformNumber(doorPublicKey, cardLoopSize);
                }
                else
                {
                    logger.Error("Both timed out at 1000000...");
                }
            }


            logger.Information($"The encryption key is {encryptionKey} !");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - No part 2 today !");
        }

        private long TransformNumber(long subject, long loopSize)
        {
            long transformedNumber = 1;

            for (long i = 0; i < loopSize; i++)
            {
                transformedNumber *= subject;
                transformedNumber = transformedNumber % 20201227;
            }

            return transformedNumber;
        }

        private long FindLoopSize(long subject, long transformedNumber, long loopLimit)
        {
            var loopSize = 0;
            var done = false;

            while (!done)
            {
                var temp = this.TransformNumber(subject, loopSize);
                if (temp == transformedNumber)
                {
                    done = true;
                }
                else if (loopSize < loopLimit)
                {
                    loopSize++;
                }
                else
                {
                    done = true;
                    logger.Warning($"Timed out after {loopSize} attempts...");
                    loopSize = -1;
                }
            }

            return loopSize;
        }

        public long FindLoopSizeOptimized(long subject, long transformedNumber, long loopLimit)
        {
            long loopSize = 1;
            long tempTransformedNumber = 1;
            var done = false;

            while (!done)
            {
                tempTransformedNumber *= subject;
                tempTransformedNumber = tempTransformedNumber % 20201227;

                if (tempTransformedNumber == transformedNumber)
                {
                    done = true;
                }
                else if (loopSize < loopLimit)
                {
                    loopSize++;
                }
                else
                {
                    done = true;
                    logger.Warning($"Timed out after {loopSize} attempts...");
                    loopSize = -1;
                }
            }

            return loopSize;
        }
    }
}