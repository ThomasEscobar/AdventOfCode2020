using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Serilog.Core;
using AdventOfCode.ToolBox;
using AdventOfCode.Day7.Models;

namespace AdventOfCode.Day7
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
            logger.Information($"=== Day 7 ===");

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
            logger.Information("PART 1 - Calculating the number of different bags that can contain a shiny gold bag");

            var validColorList = new List<string> { "shiny gold" };
            var newValidColorList = new List<string>();

            do
            {
                newValidColorList = this.GetNewValidColorList(validColorList);
                foreach (var newColor in newValidColorList)
                {
                    validColorList.Add(newColor);
                }
            } while (newValidColorList.Any());

            logger.Information($"There are {validColorList.Count - 1 } valid different bags !");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Calculating the number of bags inside the shiny gold bag");

            var shinyBag = new BagModel("shiny gold");

            this.AddAllInnersBags(shinyBag);

            logger.Information($"The {shinyBag.color} bag has {shinyBag.CountBagsInside()} bags inside it !");
        }

        private List<string> GetNewValidColorList(List<string> currentValidColorsList)
        {
            var newValidColors = new List<string>();
            foreach (var line in this.input)
            {
                var colors = this.ParseColors(line);
                var containerBag = colors.FirstOrDefault();
                var containedBags = colors.GetRange(1, colors.Count - 1);
                if (this.CheckIfValidBag(containedBags, currentValidColorsList) && !currentValidColorsList.Contains(containerBag))
                {
                    newValidColors.Add(containerBag);
                }
            }

            return newValidColors;
        }

        private bool CheckIfValidBag(List<string> containedBags, List<string> currentValidColorsList)
        {
            foreach (var containedBag in containedBags)
            {
                foreach (var validColor in currentValidColorsList)
                {
                    if (validColor.Equals(containedBag)) return true;
                }
            }
            return false;
        }

        private List<string> ParseColors(string line)
        {
            var matches = Regex.Matches(line, "([^ ]+ [^ ]+) bag");
            return matches.Select(m => m.Groups[1].Value).ToList();
        }

        private List<BagModel> CreateBagsFromLine(string line)
        {
            var bagList = new List<BagModel>();
            var matches = Regex.Matches(line, @"(\d+) ([^ ]+ [^ ]+) bag");
            foreach (var match in matches.ToList())
            {
                bagList.Add(new BagModel
                {
                    count = Int32.Parse(match.Groups[1].Value),
                    color = match.Groups[2].Value
                });
            }

            return bagList;
        }

        private List<BagModel> FindInnerBags(BagModel bag, List<string> lines)
        {
            var innerBags = new List<BagModel>();
            foreach (var line in lines.Where(l => l.Contains(bag.color)))
            {
                var colors = this.ParseColors(line);
                if (colors.FirstOrDefault().Equals(bag.color))
                {
                    innerBags = this.CreateBagsFromLine(line);
                }
            }

            return innerBags;
        }

        private void AddAllInnersBags(BagModel bag)
        {
            // Add it's own inner bags
            var newInnerBags = FindInnerBags(bag, this.input);
            foreach (var newInnerBag in newInnerBags)
            {
                bag.AddBags(newInnerBag, newInnerBag.count);
            }

            // Call same method on inner bags
            foreach (var innerBag in bag.innerBags)
            {
                AddAllInnersBags(innerBag);
            }
        }
    }
}