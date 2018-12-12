using System;
using System.Diagnostics;

namespace AdventOfCode2018
{
    public class Program
    {
        static void Main(string[] args)
        {
            Day[] days = new Day[]
                {
                    //new Day1(),
                    //new Day2(),
                    //new Day3(),
                    //new Day4(),
                    //new Day5(),
                    //new Day6(),
                    //new Day7(),
                    //new Day8(),
                    //new Day9(),
                    //new Day10(),
                    //new Day11(),
                    //new Day12(),
                    new Day13(),
                };

            WriteIntro(days);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (Day day in days)
            {
                day.Solve();
            }
            sw.Stop();
            WriteOutro(sw.Elapsed);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void WriteIntro(Day[] days)
        {
            WriteBaubles(10, false);
            WriteBaubles(10);
            Console.WriteLine();
            WriteBaubles(6);
            Console.Write("   Advent of Code 2018  ");
            WriteBaubles(6, false);
            Console.WriteLine();
            WriteBaubles(10, false);
            WriteBaubles(10);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write($"Solving ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(days.Length);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" {(days.Length == 1 ? "puzzle" : "puzzles")}, starting with Day [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(days[0].PuzzleDay);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("]");
            Console.WriteLine();
        }

        private static void WriteOutro(TimeSpan elapsed)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Finished in");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" {elapsed.TotalSeconds} ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("s");
            Console.WriteLine();
        }

        private static void WriteBaubles(int count, bool left = true)
        {
            ConsoleColor[] colors = new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.White };
            string baubles = "@oO*&QS";

            int baubleIdx = DateTime.Now.Millisecond % baubles.Length;
            int colorIdx = DateTime.Now.Millisecond % colors.Length;
            for (int i = 0; i < count; i++)
            {
                WriteBough(left);
                Console.ForegroundColor = colors[colorIdx];
                Console.Write(baubles[baubleIdx]);
                baubleIdx = (baubleIdx + 2) % baubles.Length ;
                colorIdx = (colorIdx + 1) % colors.Length;
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }

        private static void WriteBough(bool left)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (!left)
            {
                Console.Write(">>");
            }
            else
            {
                Console.Write("<<");
            }
        }
    }
}
