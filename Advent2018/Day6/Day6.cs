using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    /*
     * --- Day 6: Chronal Coordinates ---
     * The device on your wrist beeps several times, and once again you feel like you're falling.
     * 
     * "Situation critical," the device announces. "Destination indeterminate. Chronal interference detected. Please specify new target coordinates."
     * 
     * The device then produces a list of coordinates (your puzzle input). Are they places it thinks are safe or dangerous? It recommends you check manual page 729. The Elves did not give you a manual.
     * 
     * If they're dangerous, maybe you can minimize the danger by finding the coordinate that gives the largest distance from the other points.
     * 
     * Using only the Manhattan distance, determine the area around each coordinate by counting the number of integer X,Y locations that are closest to that coordinate (and aren't tied in distance to any other coordinate).
     * 
     * Your goal is to find the size of the largest area that isn't infinite. For example, consider the following list of coordinates:
     * 
     * 1, 1
     * 1, 6
     * 8, 3
     * 3, 4
     * 5, 5
     * 8, 9
     * If we name these coordinates A through F, we can draw them on a grid, putting 0,0 at the top left:
     * 
     * ..........
     * .A........
     * ..........
     * ........C.
     * ...D......
     * .....E....
     * .B........
     * ..........
     * ..........
     * ........F.
     * This view is partial - the actual grid extends infinitely in all directions. Using the Manhattan distance, each location's closest coordinate can be determined, shown here in lowercase:
     * 
     * aaaaa.cccc
     * aAaaa.cccc
     * aaaddecccc
     * aadddeccCc
     * ..dDdeeccc
     * bb.deEeecc
     * bBb.eeee..
     * bbb.eeefff
     * bbb.eeffff
     * bbb.ffffFf
     * Locations shown as . are equally far from two or more coordinates, and so they don't count as being closest to any.
     * 
     * In this example, the areas of coordinates A, B, C, and F are infinite - while not shown here, their areas extend forever outside the visible grid. However, the areas of coordinates D and E are finite: D is closest to 9 locations, and E is closest to 17 (both including the coordinate's location itself). Therefore, in this example, the size of the largest area is 17.
     * 
     * What is the size of the largest area that isn't infinite?
     */
    public class Day6 : Day
    {
        public Day6()
        {
            Input = ReadInput("Day6/Day6Input.txt");
            Coords = new List<Coord>(Input.Length);
        }

        public IList<Coord> Coords { get; private set; }

        private string[] Input { get; set; }

        public override int PuzzleDay => 6;

        protected override void ProcessInput()
        {
            int id = 1;
            foreach(string s in Input)
            {
                string[] tokens = s.Split(',');
                Coords.Add(new Coord(int.Parse(tokens[0]), int.Parse(tokens[1]), id++));
            }
        }

        protected override string Part1()
        {
            int largestDim = Coords.Max(c => Math.Max(c.X, c.Y));
            int gridSize = largestDim + 2;
            for(int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    int closestId = GetClosestCoordId(x, y);
                    if (closestId > 0)
                    {
                        Coord closest = Coords.First(c => c.Id == closestId);
                        closest.Count += 1;
                        if (x == 0 || y == 0 || x == gridSize - 1 || y == gridSize - 1)
                        {
                            closest.Infinite = true;
                        }
                    }
                }
            }

            Coord best = Coords.OrderByDescending(c => c.Count).First();

            return $"Best point ID: {best.Id}, size {best.Count}";
        }

        private int GetClosestCoordId(int x, int y)
        {
            int closestDistance = int.MaxValue;
            int closestId = -1; // -1 = more than 1 Coord equidistant
            foreach(Coord c in Coords)
            {
                if (c.X == x && c.Y == y)
                {
                    return -1;
                }

                int dist = GetManhattanDistance(x, y, c.X, c.Y);
                if (dist < closestDistance)
                {
                    closestId = c.Id;
                    closestDistance = dist;
                }
                else if (dist == closestDistance)
                {
                    closestId = 0;
                }
            }

            return closestId;
        }

        private int GetManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        }

        protected override string Part2()
        {
            return "unsolved";
        }

        public class Coord
        {
            public Coord(int x, int y, int id)
            {
                X = x;
                Y = y;
                Id = id;

            }

            public int X { get; private set; }
            public int Y { get; private set; }
            public int Id { get; private set; }
            public int Count { get; set; }
            public bool Infinite { get; set; }
        }
    }
}
