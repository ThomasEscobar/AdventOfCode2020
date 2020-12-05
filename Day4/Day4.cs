using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;
using AdventOfCode.Day4.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day4
{
    public class Solver
    {
        private readonly Logger logger;
        private List<string> input;
        private List<PassportModel> passportList;
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
            this.passportList = this.CreatePassports(this.input);
        }

        public void Solve()
        {
            logger.Information($"=== Day 4 ===");

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
            logger.Information("PART 1 - Counting the number of valid passports");

            var validPassportCount = 0;

            foreach (var passport in this.passportList)
            {
                if (this.ValidatePassport(passport, false))
                {
                    validPassportCount++;
                }
            }

            logger.Information($"There are {validPassportCount} valid passports ! ({this.passportList.Count} in total)");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Counting the number of valid passports, with data validation");

            var validPassportCount = 0;

            foreach (var passport in this.passportList)
            {
                if (this.ValidatePassport(passport, true))
                {
                    validPassportCount++;
                }
            }

            logger.Information($"There are {validPassportCount} valid passports (with data validation) ! ({this.passportList.Count} in total)");
        }

        private List<PassportModel> CreatePassports(List<string> input)
        {
            var passportList = new List<PassportModel>();
            var temp = string.Empty;

            // Merge the lines about the same passport into one, creating a list of passports strings
            foreach (var line in this.input)
            {
                // Make sure to not drop the last temp passport
                if (!string.Equals(line, string.Empty) && !string.Equals(line, this.input.LastOrDefault()))
                {
                    temp += $" {line}";
                }
                else if (!temp.Equals(string.Empty))
                {
                    passportList.Add(new PassportModel(temp.Trim()));
                    temp = string.Empty;
                }
            }

            return passportList;
        }

        private bool ValidatePassport(PassportModel passport, bool dataValidation)
        {
            var bits = passport.ToString().Split(' ');
            if (!(bits.Length == 8) && !(bits.Length == 7 && !bits.Where(b => b.Contains("cid:")).ToList().Any()))
            {
                return false;
            }

            if (dataValidation)
            {
                if (passport.byr < 1920 || passport.byr > 2020) return false;
                if (passport.iyr < 2010 || passport.iyr > 2020) return false;
                if (passport.eyr < 2020 || passport.eyr > 2030) return false;

                // Bit of regex fun because... Why not
                var match = Regex.Match(passport.hgt, @"^(\d{2,3})([a-z]{2})$");
                if (!match.Groups[2].Value.Equals("in") && !match.Groups[2].Value.Equals("cm")) return false;
                if (match.Groups[2].Value.Equals("cm"))
                {
                    if (Int32.Parse(match.Groups[1].Value) < 150 || Int32.Parse(match.Groups[1].Value) > 193) return false;
                }
                else
                {
                    if (Int32.Parse(match.Groups[1].Value) < 59 || Int32.Parse(match.Groups[1].Value) > 76) return false;
                }

                // More fun ! (please help)
                match = Regex.Match(passport.hcl, "^#[a-f0-9]{6}$");
                if (!match.Success) return false;
                if (!passport.ecl.Equals("amb") && !passport.ecl.Equals("blu") && !passport.ecl.Equals("brn") && !passport.ecl.Equals("gry") && !passport.ecl.Equals("grn") && !passport.ecl.Equals("hzl") && !passport.ecl.Equals("oth")) return false;

                // ...
                match = Regex.Match(passport.pid, @"^\d{9}$");
                if (!match.Success) return false;

                // Who cares about the cid anyways
            }

            return true;
        }
    }
}