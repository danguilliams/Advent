using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    /*
     * --- Day 10: The Stars Align ---
     * It's no use; your navigation system simply isn't capable of providing walking directions in the arctic circle, and certainly not in 1018.
     * 
     * The Elves suggest an alternative. In times like these, North Pole rescue operations will arrange points of light in the sky to guide missing Elves back to base. Unfortunately, the message is easy to miss: the points move slowly enough that it takes hours to align them, but have so much momentum that they only stay aligned for a second. If you blink at the wrong time, it might be hours before another message appears.
     * 
     * You can see these points of light floating in the distance, and record their position in the sky and their velocity, the relative change in position per second (your puzzle input). The coordinates are all given from your perspective; given enough time, those positions and velocities will move the points into a cohesive message!
     * 
     * Rather than wait, you decide to fast-forward the process and calculate what the points will eventually spell.
     * 
     * For example, suppose you note the following points:
     * 
     * position=< 9,  1> velocity=< 0,  2>
     * position=< 7,  0> velocity=<-1,  0>
     * position=< 3, -2> velocity=<-1,  1>
     * position=< 6, 10> velocity=<-2, -1>
     * position=< 2, -4> velocity=< 2,  2>
     * position=<-6, 10> velocity=< 2, -2>
     * position=< 1,  8> velocity=< 1, -1>
     * position=< 1,  7> velocity=< 1,  0>
     * position=<-3, 11> velocity=< 1, -2>
     * position=< 7,  6> velocity=<-1, -1>
     * position=<-2,  3> velocity=< 1,  0>
     * position=<-4,  3> velocity=< 2,  0>
     * position=<10, -3> velocity=<-1,  1>
     * position=< 5, 11> velocity=< 1, -2>
     * position=< 4,  7> velocity=< 0, -1>
     * position=< 8, -2> velocity=< 0,  1>
     * position=<15,  0> velocity=<-2,  0>
     * position=< 1,  6> velocity=< 1,  0>
     * position=< 8,  9> velocity=< 0, -1>
     * position=< 3,  3> velocity=<-1,  1>
     * position=< 0,  5> velocity=< 0, -1>
     * position=<-2,  2> velocity=< 2,  0>
     * position=< 5, -2> velocity=< 1,  2>
     * position=< 1,  4> velocity=< 2,  1>
     * position=<-2,  7> velocity=< 2, -2>
     * position=< 3,  6> velocity=<-1, -1>
     * position=< 5,  0> velocity=< 1,  0>
     * position=<-6,  0> velocity=< 2,  0>
     * position=< 5,  9> velocity=< 1, -2>
     * position=<14,  7> velocity=<-2,  0>
     * position=<-3,  6> velocity=< 2, -1>
     * Each line represents one point. Positions are given as <X, Y> pairs: X represents how far left (negative) or right (positive) the point appears, while Y represents how far up (negative) or down (positive) the point appears.
     * 
     * At 0 seconds, each point has the position given. Each second, each point's velocity is added to its position. So, a point with velocity <1, -2> is moving to the right, but is moving upward twice as quickly. If this point's initial position were <3, 9>, after 3 seconds, its position would become <6, 3>.
     * 
     * Over time, the points listed above would move like this:
     * 
     * Initially:
     * ........#.............
     * ................#.....
     * .........#.#..#.......
     * ......................
     * #..........#.#.......#
     * ...............#......
     * ....#.................
     * ..#.#....#............
     * .......#..............
     * ......#...............
     * ...#...#.#...#........
     * ....#..#..#.........#.
     * .......#..............
     * ...........#..#.......
     * #...........#.........
     * ...#.......#..........
     * 
     * After 1 second:
     * ......................
     * ......................
     * ..........#....#......
     * ........#.....#.......
     * ..#.........#......#..
     * ......................
     * ......#...............
     * ....##.........#......
     * ......#.#.............
     * .....##.##..#.........
     * ........#.#...........
     * ........#...#.....#...
     * ..#...........#.......
     * ....#.....#.#.........
     * ......................
     * ......................
     * 
     * After 2 seconds:
     * ......................
     * ......................
     * ......................
     * ..............#.......
     * ....#..#...####..#....
     * ......................
     * ........#....#........
     * ......#.#.............
     * .......#...#..........
     * .......#..#..#.#......
     * ....#....#.#..........
     * .....#...#...##.#.....
     * ........#.............
     * ......................
     * ......................
     * ......................
     * 
     * After 3 seconds:
     * ......................
     * ......................
     * ......................
     * ......................
     * ......#...#..###......
     * ......#...#...#.......
     * ......#...#...#.......
     * ......#####...#.......
     * ......#...#...#.......
     * ......#...#...#.......
     * ......#...#...#.......
     * ......#...#..###......
     * ......................
     * ......................
     * ......................
     * ......................
     * 
     * After 4 seconds:
     * ......................
     * ......................
     * ......................
     * ............#.........
     * ........##...#.#......
     * ......#.....#..#......
     * .....#..##.##.#.......
     * .......##.#....#......
     * ...........#....#.....
     * ..............#.......
     * ....#......#...#......
     * .....#.....##.........
     * ...............#......
     * ...............#......
     * ......................
     * ......................
     * After 3 seconds, the message appeared briefly: HI. Of course, your message will be much longer and will take many more seconds to appear.
     * 
     * What message will eventually appear in the sky?
     */
    public class Day10 : Day
    {
        public Day10()
        {
            Input = ReadInput("Day10/Day10Input.txt");
            Lights = new List<Light>();
        }

        public override int PuzzleDay => 10;

        private string[] Input { get; set; }

        private List<Light> Lights { get; set; }

        protected override void ProcessInput()
        {
            foreach(string s in Input)
            {
                // position=<-52775,  31912> velocity=< 5, -3>
                string[] t = s.Split('<', ',', '>');
                Lights.Add(new Light(t[1], t[2], t[4], t[5]));
            }
        }
        protected override string Part1()
        {
            int minXRange = int.MaxValue;
            int minYRange = int.MaxValue;
            int minXIter = 0;
            int minYIter = 0;
            for (int i = 0; i < 12000; i++)
            {
                Lights.ForEach(l => l.SetPosAfterMoves(i));
                int xRange = Lights.Max(l => l.X) - Lights.Min(l => l.X);
                int yRange = Lights.Max(l => l.Y) - Lights.Min(l => l.Y);
                if(xRange < minXRange)
                {
                    minXRange = xRange;
                    minXIter = i;
                }

                if (yRange < minYRange)
                {
                    minYRange = yRange;
                    minYIter = i;
                }
            }

            string result = PrintLights(minXIter);

            return result;
        }

        private string PrintLights(int iteration)
        {
            Lights.ForEach(l => l.SetPosAfterMoves(iteration));

            int minX = Lights.Min(l => l.X);
            int minY = Lights.Min(l => l.Y);
            int maxX = Lights.Max(l => l.X);
            int maxY = Lights.Max(l => l.Y);
            int xRange = maxX - minX + 1;
            int yRange = maxY - minY + 1;
            char[,] grid = new char[xRange, yRange];
            for (int i = 0; i < xRange; i++)
            {
                for (int j = 0; j < yRange; j++)
                {
                    grid[i, j] = '.';
                }
            }

            foreach(Light l in Lights)
            {
                grid[l.X - minX, l.Y - minY] = '#';
            }

            string result = "\n";
            for (int i = 0; i < yRange; i++)
            {
                string line = "   ";
                for (int j = 0; j < xRange; j++)
                {
                    line += grid[j, i];
                }
                result += line + "\n";
            }

            return result;
        }

        
        protected override string Part2()
        {
            return "unfinished";
        }

        public class Light
        {
            public Light(string x, string y, string dx, string dy)
            {
                X = int.Parse(x);
                Y = int.Parse(y);
                StartX = X;
                StartY = Y;
                dX = int.Parse(dx);
                dY = int.Parse(dy);
            }

            public int X { get; private set; }
            public int Y { get; private set; }

            public int dX { get; private set; }
            public int dY { get; private set; }

            private int StartX { get; set; }
            private int StartY { get; set; }

            public void SetPosAfterMoves(int moves)
            {
                X = StartX + moves * dX;
                Y = StartY + moves * dY;
            }
        }


    }
}
