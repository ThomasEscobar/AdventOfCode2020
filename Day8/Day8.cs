using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;
using AdventOfCode.Day8.Models;

namespace AdventOfCode.Day8
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
            logger.Information($"=== Day 8 ===");

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
            logger.Information("PART 1 - Calculating the value of the accumulator when we hit an infinite loop");

            var commandList = new List<CommandModel>();

            foreach (var line in this.input)
            {
                commandList.Add(this.CreateCommand(line));
            }

            var result = this.RunCommandList(commandList);

            if (!result.completed)
            {
                logger.Information("The program didn't complete... We hit an infinite loop");
            }
            else
            {
                logger.Information("The program ran to completion !");
            }

            logger.Information($"At that time, the accumulator value is {result.value}");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Finding which command needs to be \"swapped\" to terminate the program");

            var commandList = new List<CommandModel>();
            var indexSwap = 0;

            foreach (var line in this.input)
            {
                commandList.Add(this.CreateCommand(line));
            }

            var completed = false;
            do
            {
                var newCommandList = new List<CommandModel>();
                // Copy the original list
                newCommandList = commandList.ConvertAll(c => new CommandModel(c.type, c.value));

                // Swap one "jmp" or "nop" command to the other type
                for (int i = indexSwap; i < newCommandList.Count; i++)
                {
                    if (newCommandList[i].type.Equals("jmp") || newCommandList[i].type.Equals("nop"))
                    {
                        this.SwapCommandType(newCommandList[i]);
                        // Save the index of the last command swapped
                        indexSwap = i + 1;
                        break;
                    }
                }

                var result = this.RunCommandList(newCommandList);
                if (result.completed)
                {
                    completed = true;
                    logger.Information("The program ran to completion !");
                    logger.Information($"The swapped command was at index {indexSwap}. The program completed, and the accumalor value is {result.value}");
                }
            }
            while (!completed);
        }

        private CommandModel CreateCommand(string line)
        {
            return new CommandModel
            {
                type = line.Split(' ')[0],
                value = Int32.Parse(line.Split(' ')[1])
            };
        }

        private void SwapCommandType(CommandModel command)
        {
            if (command.type.Equals("jmp"))
            {
                command.type = "nop";
            }
            else if (command.type.Equals("nop"))
            {
                command.type = "jmp";
            }
        }

        private Result RunCommandList(List<CommandModel> commandList)
        {
            var accumulator = 0;
            var commandIndex = 0;
            var infiniteLoop = false;
            var completed = false;

            do
            {
                var currentCommand = commandList[commandIndex];
                if (!currentCommand.executed)
                {
                    switch (currentCommand.type)
                    {
                        case "acc":
                            accumulator += currentCommand.value;
                            commandIndex++;
                            break;
                        case "jmp":
                            commandIndex += currentCommand.value;
                            break;
                        case "nop":
                            // Do nothing
                            commandIndex++;
                            break;
                        default:
                            throw new Exception($"Unexpected type of command: {currentCommand.type}");
                    }
                    currentCommand.executed = true;
                }
                else
                {
                    // logger.Information($"We have already run the command at index {commandIndex}: infinite loop detected !");
                    infiniteLoop = true;
                }

                if (commandIndex == commandList.Count - 1)
                {
                    completed = true;
                }
            } while (!infiniteLoop && !completed);

            return new Result { value = accumulator, completed = completed };
        }

        public class Result
        {
            public int value = 0;
            public bool completed = false;
        }
    }
}