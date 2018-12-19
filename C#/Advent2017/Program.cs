using System;
using System.Collections.Generic;
using AdventOfCode2017.Puzzles;

namespace AdventOfCode2017
{
    class Program
    {
        private static List<Puzzle> Puzzles = new List<Puzzle>()
        {
            //new Day1(),
            //new Day2(),
            //new Day3(),
            new Day4()
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2017");

            foreach (Puzzle p in Puzzles)
            {
                p.Solve();
            }

        }


    }
}
