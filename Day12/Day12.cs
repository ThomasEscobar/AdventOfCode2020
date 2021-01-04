using System;
using System.Diagnostics;
using System.Collections.Generic;
using Serilog.Core;
using AdventOfCode.ToolBox;

namespace AdventOfCode.Day12
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
            logger.Information($"=== Day 12 ===");

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
            logger.Information("PART 1 - Finding the final ship position and calculating the Manhattan distance");

            // Initialise the ship at (0,0), facing East (angle = 0)
            var ship = new Ship();

            foreach (var line in this.input)
            {
                var action = line[0];
                var value = Int32.Parse(line.Substring(1));

                ship.ApplyAction(action, value);
            }

            logger.Information($"The final position of the ship is ({ship.x};{ship.y}), and the Manhattan distance is {ship.GetManhattanDistance()}");
        }

        private void SolvePart2()
        {
            logger.Information("PART 2 - Finding the final ship position and calculating the Manhattan distance, using a waypoint");

            // Initialise the ship at (0,0), and the waypoint at (10,1)
            var ship = new Ship();
            var waypoint = new Waypoint(10, 1);

            foreach (var line in this.input)
            {
                var action = line[0];
                var value = Int32.Parse(line.Substring(1));

                waypoint.ApplyAction(action, value, ship);
            }

            logger.Information($"The final position of the ship is ({ship.x};{ship.y}), and the Manhattan distance is {ship.GetManhattanDistance()}");
        }

        public class Ship
        {
            public int x;
            public int y;
            public int angle;

            public Ship(int x = 0, int y = 0, int angle = 0)
            {
                this.x = x;
                this.y = y;
                this.angle = angle;
            }

            public void ApplyAction(char action, int value)
            {
                if (action == 'N' || action == 'S' || action == 'E' || action == 'W')
                {
                    this.MoveShip(action, value);
                }
                else if (action == 'L' || action == 'R')
                {
                    this.RotateShip(action, value);
                }
                else if (action == 'F')
                {
                    this.MoveShipForward(value);
                }
                else
                {
                    throw new Exception($"Unknown action: {action}");
                }
            }

            private void MoveShip(char direction, int value)
            {
                switch (direction)
                {
                    case 'N':
                        this.y += value;
                        break;
                    case 'S':
                        this.y -= value;
                        break;
                    case 'E':
                        this.x += value;
                        break;
                    case 'W':
                        this.x -= value;
                        break;
                }
            }

            private void RotateShip(char direction, int value)
            {
                if (direction == 'L')
                {
                    this.angle += value;
                }
                else
                {
                    this.angle -= value;
                }
                if (this.angle < 0)
                {
                    this.angle += 360;
                }
                else if (this.angle >= 360)
                {
                    this.angle -= 360;
                }
            }

            public void MoveShipForward(int value)
            {
                switch (this.angle)
                {
                    case 90:
                        this.MoveShip('N', value);
                        break;
                    case 270:
                        this.MoveShip('S', value);
                        break;
                    case 0:
                        this.MoveShip('E', value);
                        break;
                    case 180:
                        this.MoveShip('W', value);
                        break;
                    default:
                        throw new Exception($"Unexpected angle: {this.angle}");
                }
            }

            public void MoveToWaypoint(Waypoint wp)
            {
                this.x += wp.x;
                this.y += wp.y;
            }

            public int GetManhattanDistance()
            {
                return Math.Abs(this.x) + Math.Abs(this.y);
            }


        }

        public class Waypoint
        {
            // The coordonates of the waypoint are relative to the ship
            public int x;
            public int y;

            public Waypoint(int x = 0, int y = 0)
            {
                this.x = x;
                this.y = y;
            }

            public void ApplyAction(char action, int value, Ship ship)
            {
                if (action == 'N' || action == 'S' || action == 'E' || action == 'W')
                {
                    this.MoveWaypoint(action, value);
                }
                else if (action == 'L' || action == 'R')
                {
                    this.RotateWaypoint(action, value);
                }
                else if (action == 'F')
                {
                    for (int i = 0; i < value; i++)
                    {
                        ship.MoveToWaypoint(this);
                    }
                }
                else
                {
                    throw new Exception($"Unknown action: {action}");
                }
            }

            public void MoveWaypoint(char direction, int value)
            {
                switch (direction)
                {
                    case 'N':
                        this.y += value;
                        break;
                    case 'S':
                        this.y -= value;
                        break;
                    case 'E':
                        this.x += value;
                        break;
                    case 'W':
                        this.x -= value;
                        break;
                }
            }

            public void RotateWaypoint(char direction, int value)
            {
                var rotationCount = value / 90;
                for (int i = 0; i < rotationCount; i++)
                {
                    if (direction == 'R')
                    {
                        this.Rotate90DegresClockwise();
                    }
                    else
                    {
                        this.Rotate90DegresTrigo();
                    }
                }
            }

            public void Rotate90DegresTrigo()
            {
                var newX = -this.y;
                var newY = this.x;

                this.x = newX;
                this.y = newY;
            }

            public void Rotate90DegresClockwise()
            {
                var newX = this.y;
                var newY = -this.x;

                this.x = newX;
                this.y = newY;
            }
        }
    }
}