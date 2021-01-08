using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;
using System.Linq;

namespace AdventOfCode.Day14
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
            logger.Information($"=== Day 14 ===");

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
            logger.Information("PART 1 - Calculating the sum of all values in memory after the inizialisation program finishes");

            var currentMask = string.Empty;
            var memDictionary = new Dictionary<int, long>();

            foreach (var line in this.input)
            {
                if (line.Contains("mask"))
                {
                    currentMask = line.Split("mask = ")[1];
                }
                else
                {
                    var memIndex = Int32.Parse(line.Split('[', ']')[1]);
                    var value = Int64.Parse(line.Split("] = ")[1]);
                    var binaryNumber = this.ConvertToBinary(value);
                    var maskedBinaryNumber = this.ApplyMaskToBinary(binaryNumber, currentMask);
                    var maskedDecimalNumber = this.ConvertToDecimal(maskedBinaryNumber);

                    if (!memDictionary.Keys.ToList<int>().Contains(memIndex))
                    {
                        memDictionary.Add(memIndex, maskedDecimalNumber);
                    }
                    else
                    {
                        memDictionary[memIndex] = maskedDecimalNumber;
                    }
                }
            }

            logger.Information($"The sum of all the values in memory is equal to {memDictionary.Values.Sum()}");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Calculating the sum of all values in memory after the new inizialisation program finishes");

            var currentMask = string.Empty;
            var memDictionary = new Dictionary<long, long>();

            foreach (var line in this.input)
            {
                if (line.Contains("mask"))
                {
                    currentMask = line.Split("mask = ")[1];
                }
                else
                {
                    var memIndex = Int32.Parse(line.Split('[', ']')[1]);
                    var value = Int64.Parse(line.Split("] = ")[1]);
                    var binaryIndex = this.ConvertToBinary(memIndex);
                    var maskedBinaryIndexWithFloating = this.ApplyMaskToBinary(binaryIndex, currentMask, true);
                    var listMaskedBinaryIndices = this.FindAllBinaryCombinaisons(maskedBinaryIndexWithFloating);

                    foreach (var maskedBinaryIndex in listMaskedBinaryIndices)
                    {
                        var maskedDecimalIndex = this.ConvertToDecimal(maskedBinaryIndex);

                        if (!memDictionary.Keys.ToList<long>().Contains(maskedDecimalIndex))
                        {
                            memDictionary.Add(maskedDecimalIndex, value);
                        }
                        else
                        {
                            memDictionary[maskedDecimalIndex] = value;
                        }
                    }
                }
            }

            logger.Information($"The sum of all the values in memory is equal to {memDictionary.Values.Sum()}");
        }

        public long ConvertToDecimal(string binaryNumber, int binarySize = 36)
        {
            long decimalNumber = 0;

            for (int i = 0; i < binaryNumber.Length; i++)
            {
                decimalNumber += binaryNumber[i] == '1' ? (long)Math.Pow(2, binarySize - 1 - i) : 0;
            }

            return decimalNumber;
        }

        public string ConvertToBinary(long decimalNumber, int binarySize = 36)
        {
            var binaryNumber = string.Empty;
            long rest = decimalNumber;

            for (int i = binarySize - 1; i >= 0; i--)
            {
                binaryNumber += rest / (long)Math.Pow(2, i);
                rest = rest % (long)Math.Pow(2, i);
            }

            return binaryNumber;
        }

        public string ApplyMaskToBinary(string binaryNumber, string mask, bool withFloatingBits = false, int binarySize = 36)
        {
            if (binaryNumber.Length != binarySize || mask.Length != binarySize)
            {
                throw new Exception($"Number or mask don't match the expected binary size of {binarySize}");
            }

            var newBinaryNumber = string.Empty;
            for (int i = 0; i < binaryNumber.Length; i++)
            {
                if (!withFloatingBits)
                {
                    newBinaryNumber += mask[i] != 'X' ? mask[i] : binaryNumber[i];
                }
                else
                {
                    newBinaryNumber += mask[i] != '0' ? mask[i] : binaryNumber[i];
                }
            }

            return newBinaryNumber;
        }

        public List<string> FindAllBinaryCombinaisons(string maskedBinaryIndexWithFloating, int binarySize = 36)
        {
            var listPossibleBinaryNumbers = new List<string>();
            listPossibleBinaryNumbers.Add(maskedBinaryIndexWithFloating);

            for (int i = 0; i < binarySize; i++)
            {
                // Skip if there is no 'X' on this index
                if (listPossibleBinaryNumbers.FirstOrDefault()[i] != 'X')
                {
                    continue;
                }

                // If there is, add 2 new possible numbers for each number in the list, with a 0 and a 1 instead of the X
                var tmpList = new List<string>();
                foreach (var binaryNumberWithFloating in listPossibleBinaryNumbers)
                {
                    var tmpNumber0 = string.Empty;
                    var tmpNumber1 = string.Empty;
                    for (int j = 0; j < binarySize; j++)
                    {
                        if (binaryNumberWithFloating[j] == 'X' && i == j)
                        {
                            tmpNumber0 += '0';
                            tmpNumber1 += '1';
                        }
                        else
                        {
                            tmpNumber0 += binaryNumberWithFloating[j];
                            tmpNumber1 += binaryNumberWithFloating[j];
                        }
                    }
                    tmpList.Add(tmpNumber0);
                    tmpList.Add(tmpNumber1);
                }

                listPossibleBinaryNumbers = !tmpList.Any() ? listPossibleBinaryNumbers : tmpList;
            }

            return listPossibleBinaryNumbers;
        }
    }
}