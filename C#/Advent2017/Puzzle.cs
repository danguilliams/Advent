using System;
namespace AdventOfCode2017.Puzzles
{
    public abstract class Puzzle
    {
        protected string Input;

        public Puzzle(uint day)
        {
            Day = day;
        }

        // Day of the advent
        public uint Day { get; private set; }

        public string Part1Solution { get; protected set; }
        public string Part2Solution { get; protected set; }

        public void Solve()
        {
            DateTime start = DateTime.Now;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Day {Day} start");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Processing Input");
            ProcessInput();
            Console.WriteLine("  Starting Pt. 1");
            DateTime solve1 = DateTime.Now;
            Part1();
            Console.WriteLine($"  Pt. 1:{Part1Solution}");
            Console.WriteLine("  Starting Pt. 2");
            DateTime solve2 = DateTime.Now;
            Part2();
            Console.WriteLine($"  Pt. 2:{Part2Solution}");
            TimeSpan duration = DateTime.Now - start;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Day {Day} finished in {duration.TotalSeconds} seconds.");
            Console.WriteLine();
        }

        protected abstract void ProcessInput();
        protected abstract void Part1();
        protected abstract void Part2();

    }
}
