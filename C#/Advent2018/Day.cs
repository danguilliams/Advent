using System;
using System.Diagnostics;
using System.IO;

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
            Input = ReadInput($"Day{(PuzzleDay > 9 ? PuzzleDay.ToString() : "0" + PuzzleDay.ToString())}/Day{PuzzleDay}Input.txt");
        }

        protected string[] Input { get; set; }

        private Stopwatch Timer { get; set; }

        public abstract int PuzzleDay { get; }

        public void Solve()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Day [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(PuzzleDay);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("]");
            Console.ForegroundColor = ConsoleColor.White;
            Timer.Start();
            ProcessInput();
            string part1 = Part1();
            Timer.Stop();
            WritePartResults(1, Timer.Elapsed, part1);
            Timer.Restart();
            string part2 = Part2();
            Timer.Stop();
            WritePartResults(2, Timer.Elapsed, part2);
            Console.WriteLine();
        }

        private void WritePartResults(int partNum, TimeSpan duration, string solution)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" Pt ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(partNum);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" took ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(duration.TotalMilliseconds);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" ms:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t");
            Console.WriteLine(solution);
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

        protected virtual void ProcessInput()
        {

        }

        protected abstract string Part1();

        protected abstract string Part2();
    }

}
