using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;
using System.Linq;

namespace AdventOfCode.Day22
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
            logger.Information($"=== Day 22 ===");

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
            logger.Information("PART 1 - Calculating the winner's score (normal Combat)");

            var player1Deck = new List<int>();
            var player2Deck = new List<int>();

            this.DealCards(player1Deck, player2Deck);

            var i = 1;
            while (player1Deck.Count > 0 && player2Deck.Count > 0 && i < 10000)
            {
                this.ResolveRound(player1Deck, player2Deck, i);
                i++;
            }
            if (player1Deck.Count > player2Deck.Count)
            {
                logger.Information($"Player 1 wins with a score of {this.CalculateScore(player1Deck)}");
            }
            else
            {
                logger.Information($"Player 2 wins with a score of {this.CalculateScore(player2Deck)}");
            }
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Calculating the winner's score (recursive Combat)");

            var player1Deck = new List<int>();
            var player2Deck = new List<int>();

            this.DealCards(player1Deck, player2Deck);

            int gameCount = 0;
            this.StartRecursiveCombatGame(player1Deck, player2Deck, gameCount);
        }

        private void DealCards(List<int> player1Deck, List<int> player2Deck)
        {
            var dealingToPlayer1 = true;
            foreach (var line in this.input)
            {
                if (line.Contains("Player 1") || line.Equals(string.Empty))
                {
                    // Do nothing
                }
                else if (line.Contains("Player 2"))
                {
                    dealingToPlayer1 = false;
                }
                else if (dealingToPlayer1)
                {
                    player1Deck.Add(Int32.Parse(line));
                }
                else
                {
                    player2Deck.Add(Int32.Parse(line));
                }
            }
        }

        private void ResolveRound(List<int> player1Deck, List<int> player2Deck, int i)
        {
            logger.Debug($"-- Round {i} --");
            logger.Debug($"Player 1's deck: {String.Join(", ", player1Deck)}");
            logger.Debug($"Player 2's deck: {String.Join(", ", player2Deck)}");
            // Draw a card from each deck
            var player1Card = player1Deck[0];
            var player2Card = player2Deck[0];
            player1Deck.RemoveAt(0);
            player2Deck.RemoveAt(0);

            logger.Debug($"Player 1 plays: {player1Card}");
            logger.Debug($"Player 2 plays: {player2Card}");

            // Find out who wins
            if (player1Card > player2Card)
            {
                // Player 1 won ! Put the 2 cards at the bottom of the deck, highest card first
                logger.Debug("Player 1 wins the round!");
                player1Deck.Add(player1Card);
                player1Deck.Add(player2Card);
            }
            else
            {
                // Player 2 won !
                logger.Debug("Player 2 wins the round!");
                player2Deck.Add(player2Card);
                player2Deck.Add(player1Card);
            }
        }

        private int CalculateScore(List<int> deck)
        {
            var score = 0;
            for (int i = 1; i <= deck.Count; i++)
            {
                score += i * deck[deck.Count - i];
            }

            return score;
        }

        private bool CheckIfInstantWinForPlayer1(List<int> player1Deck, List<int> player2Deck, List<List<int>> player1DeckHistory, List<List<int>> player2DeckHistory)
        {
            // Find the decks in the history that are the same size as current deck. If none, return false
            var player1DecksWithSameSize = player1DeckHistory.Where(deck => deck.Count == player1Deck.Count);
            if (!player1DecksWithSameSize.Any()) return false;

            var player2DecksWithSameSize = player2DeckHistory.Where(deck => deck.Count == player2Deck.Count);
            if (!player1DecksWithSameSize.Any()) return false;

            var foundMatchingDeck1 = false;
            var foundMatchingDeck2 = false;
            // Compare each deck in the history with the current deck for both players. SequenceEqual() also checks order
            foreach (var player1DeckInHistory in player1DecksWithSameSize)
            {
                if (player1Deck.SequenceEqual(player1DeckInHistory))
                {
                    foundMatchingDeck1 = true;
                    break;
                }
            }
            foreach (var player2DeckInHistory in player2DecksWithSameSize)
            {
                if (player2Deck.SequenceEqual(player2DeckInHistory))
                {
                    foundMatchingDeck2 = true;
                    break;
                }
            }

            // If we found an exact copy of both decks in the deck histories, return true. Else, return false
            if (foundMatchingDeck1 && foundMatchingDeck2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void StartRecursiveCombatGame(List<int> player1Deck, List<int> player2Deck, int gameCount)
        {
            var player1DeckHistory = new List<List<int>>();
            var player2DeckHistory = new List<List<int>>();

            var roundCount = 1;
            while (player1Deck.Count > 0 && player2Deck.Count > 0 && roundCount < 10000)
            {
                player1DeckHistory.Add(player1Deck);
                player2DeckHistory.Add(player2Deck);
                if (this.CheckIfInstantWinForPlayer1(player1Deck, player2Deck, player1DeckHistory, player2DeckHistory))
                {
                    logger.Debug("PLAYER 1 WINS BY SUDDEN DEATH !!!");
                    break;
                }

                if (player1Deck.Count >= player1Deck[0] && player2Deck.Count >= player2Deck[0])
                {
                    this.StartRecursiveCombatGame(player1Deck, player2Deck, roundCount);
                }
                else
                {
                    this.ResolveRound(player1Deck, player2Deck, roundCount);
                }
            }
        }
    }
}

// var player1TriggerCard = player1Deck[0];
// var player2TriggerCard = player2Deck[0];
// player1Deck.RemoveAt(0);
// player2Deck.RemoveAt(0);