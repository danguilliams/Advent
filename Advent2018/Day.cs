using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    /// <summary>
    /// Puzzle base class containing boilerplate functions to help initialize puzzles and time solution execution
    /// </summary>
    public abstract class Day
    {
        public Day()
        {
            Timer = new Stopwatch();
        }

        private Stopwatch Timer { get; set; }

        public abstract int PuzzleDay { get; }

        public void Solve()
        {
            Timer.Start();
            string part1 = Part1();
            Timer.Stop();
            TimeSpan part1Time = Timer.Elapsed;
            Console.WriteLine($"Day{PuzzleDay}:Part 1 finished in {part1Time.ToString()} - solution:");
            Console.WriteLine(part1);
            Timer.Restart();
            string part2 = Part2();
            Timer.Stop();
            TimeSpan part2Time = Timer.Elapsed;
            Console.WriteLine($"Day{PuzzleDay}:Part 2 finished in {part1Time.ToString()} - solution:");
            Console.WriteLine(part2);
        }

        public string[] ReadInput(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllLines(fileName);
            }
            else
            {
                throw new FileNotFoundException($"{fileName} was not found");
            }
        }

        protected abstract string Part1();

        protected abstract string Part2();
    }

}
