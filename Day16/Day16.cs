using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;
using AdventOfCode.Day16.Models;
using System.Text.RegularExpressions;
using System.Linq;

namespace AdventOfCode.Day16
{
    public class Solver
    {
        private readonly Logger logger;
        private List<string> input;
        private List<RuleModel> ruleList;
        private List<TicketModel> validTicketList;
        private TicketModel yourTicket;
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
            this.ruleList = new List<RuleModel>();
            this.yourTicket = null;
        }

        public void Solve()
        {
            logger.Information($"=== Day 16 ===");

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
            logger.Information("PART 1 - Find the ticket scanning error rate for the nearby tickets");

            var ticketList = new List<TicketModel>();

            // Create list of rules and tickets from input
            foreach (var line in this.input)
            {
                if (line.Contains("or"))
                {
                    var match = Regex.Match(line, @"(\D*): (\d*)-(\d*) or (\d*)-(\d*)");
                    var ruleName = match.Groups[1].Value;
                    var min1 = Int32.Parse(match.Groups[2].Value);
                    var max1 = Int32.Parse(match.Groups[3].Value);
                    var min2 = Int32.Parse(match.Groups[4].Value);
                    var max2 = Int32.Parse(match.Groups[5].Value);
                    var newRule = new RuleModel(ruleName, min1, max1, min2, max2);
                    this.ruleList.Add(newRule);
                }
                else if (line.Contains(','))
                {
                    var newTicket = new TicketModel(line.Split(',').Select(x => Int32.Parse(x)).ToList());
                    if (this.yourTicket == null)
                    {
                        this.yourTicket = newTicket;
                    }
                    else
                    {
                        ticketList.Add(newTicket);
                    }
                }
            }

            if (this.ruleList.Count != ticketList.FirstOrDefault().numberList.Count)
            {
                logger.Error("Something is wrong");
                throw new Exception("The ticket size should match the number of rules !");
            }

            var totalErrorRate = 0;
            // Verify validity of each ticket number
            foreach (var ticket in ticketList)
            {
                var ticketErrorRate = 0;
                // Check the number against each rule. If it doesn't validate all of the rules, the number is invalid
                foreach (var number in ticket.numberList)
                {
                    var numberIsValid = false;
                    foreach (var rule in this.ruleList)
                    {
                        if (rule.CheckIfNumberIsValid(number))
                        {
                            numberIsValid = true;
                            break;
                        }
                    }

                    // If one number is invalid, add it to the error rate of the ticket
                    if (!numberIsValid)
                    {
                        ticketErrorRate += number;
                    }
                }

                // If the error rate of the ticket is different to 0, the ticket is invalid
                if (ticketErrorRate != 0)
                {
                    totalErrorRate += ticketErrorRate;
                    ticket.valid = false;
                }
            }

            logger.Information($"The total error rate of nearby tickets is {totalErrorRate} !");
            this.validTicketList = ticketList.Where(t => t.valid == true).ToList();
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Looking for the product of the values for the fields that start with \"departure\"");

            // Loop through all the rules
            foreach (var rule in this.ruleList)
            {
                // Loop through the ticket number positions (x)
                for (int i = 0; i < this.validTicketList.FirstOrDefault().numberList.Count; i++)
                {
                    var ruleValidatesPosition = true;
                    // Loop through the valid tickets (y)
                    foreach (var validTicket in this.validTicketList)
                    {
                        if (!rule.CheckIfNumberIsValid(validTicket.numberList[i]))
                        {
                            ruleValidatesPosition = false;
                            break;
                        }
                    }

                    // If the rule validates the entire column (all the valid tickets), add that position to the valid positions
                    if (ruleValidatesPosition)
                    {
                        rule.positionsValidated.Add(i);
                    }
                }
            }

            // Create ordered list of rules
            var dictionaryIndexRule = new Dictionary<int, RuleModel>();
            // Loop through the list of rules, going from the one that validates the least positions to the one that validates the most
            foreach (var rule in this.ruleList.OrderBy(rule => rule.positionsValidated.Count))
            {
                // Loop through the positions validated by the rule
                foreach (var validPosition in rule.positionsValidated)
                {
                    if (!dictionaryIndexRule.Keys.Contains(validPosition))
                    {
                        dictionaryIndexRule.Add(validPosition, rule);
                    }
                }
            }

            // Calculate the product
            long product = 1;
            // Go through the dictionary by ascending position 
            foreach (var position in dictionaryIndexRule.Keys.OrderBy(k => k))
            {
                if (dictionaryIndexRule[position].ruleName.Contains("departure"))
                {
                    product *= this.yourTicket.numberList[position];
                }
            }

            logger.Information($"The product of the 6 fields that contain \"departure\" for your ticket is {product}");
        }
    }
}