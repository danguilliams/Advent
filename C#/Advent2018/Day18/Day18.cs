using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    /*
     * --- Day 18: Settlers of The North Pole ---
     * On the outskirts of the North Pole base construction project, many Elves are collecting lumber.
     * 
     * The lumber collection area is 50 acres by 50 acres; each acre can be either open ground (.), trees (|), or a lumberyard (#). You take a scan of the area (your puzzle input).
     * 
     * Strange magic is at work here: each minute, the landscape looks entirely different. In exactly one minute, an open acre can fill with trees, a wooded acre can be converted to a lumberyard, or a lumberyard can be cleared to open ground (the lumber having been sent to other projects).
     * 
     * The change to each acre is based entirely on the contents of that acre as well as the number of open, wooded, or lumberyard acres adjacent to it at the start of each minute. Here, "adjacent" means any of the eight acres surrounding that acre. (Acres on the edges of the lumber collection area might have fewer than eight adjacent acres; the missing acres aren't counted.)
     * 
     * In particular:
     * 
     * An open acre will become filled with trees if three or more adjacent acres contained trees. Otherwise, nothing happens.
     * An acre filled with trees will become a lumberyard if three or more adjacent acres were lumberyards. Otherwise, nothing happens.
     * An acre containing a lumberyard will remain a lumberyard if it was adjacent to at least one other lumberyard and at least one acre containing trees. Otherwise, it becomes open.
     * These changes happen across all acres simultaneously, each of them using the state of all acres at the beginning of the minute and changing to their new form by the end of that same minute. Changes that happen during the minute don't affect each other.
     * 
     * For example, suppose the lumber collection area is instead only 10 by 10 acres with this initial configuration:
     * 
     * Initial state:
     * .#.#...|#.
     * .....#|##|
     * .|..|...#.
     * ..|#.....#
     * #.#|||#|#|
     * ...#.||...
     * .|....|...
     * ||...#|.#|
     * |.||||..|.
     * ...#.|..|.
     * 
     * After 1 minute:
     * .......##.
     * ......|###
     * .|..|...#.
     * ..|#||...#
     * ..##||.|#|
     * ...#||||..
     * ||...|||..
     * |||||.||.|
     * ||||||||||
     * ....||..|.
     * 
     * After 2 minutes:
     * .......#..
     * ......|#..
     * .|.|||....
     * ..##|||..#
     * ..###|||#|
     * ...#|||||.
     * |||||||||.
     * ||||||||||
     * ||||||||||
     * .|||||||||
     * 
     * After 3 minutes:
     * .......#..
     * ....|||#..
     * .|.||||...
     * ..###|||.#
     * ...##|||#|
     * .||##|||||
     * ||||||||||
     * ||||||||||
     * ||||||||||
     * ||||||||||
     * 
     * After 4 minutes:
     * .....|.#..
     * ...||||#..
     * .|.#||||..
     * ..###||||#
     * ...###||#|
     * |||##|||||
     * ||||||||||
     * ||||||||||
     * ||||||||||
     * ||||||||||
     * 
     * After 5 minutes:
     * ....|||#..
     * ...||||#..
     * .|.##||||.
     * ..####|||#
     * .|.###||#|
     * |||###||||
     * ||||||||||
     * ||||||||||
     * ||||||||||
     * ||||||||||
     * 
     * After 6 minutes:
     * ...||||#..
     * ...||||#..
     * .|.###|||.
     * ..#.##|||#
     * |||#.##|#|
     * |||###||||
     * ||||#|||||
     * ||||||||||
     * ||||||||||
     * ||||||||||
     * 
     * After 7 minutes:
     * ...||||#..
     * ..||#|##..
     * .|.####||.
     * ||#..##||#
     * ||##.##|#|
     * |||####|||
     * |||###||||
     * ||||||||||
     * ||||||||||
     * ||||||||||
     * 
     * After 8 minutes:
     * ..||||##..
     * ..|#####..
     * |||#####|.
     * ||#...##|#
     * ||##..###|
     * ||##.###||
     * |||####|||
     * ||||#|||||
     * ||||||||||
     * ||||||||||
     * 
     * After 9 minutes:
     * ..||###...
     * .||#####..
     * ||##...##.
     * ||#....###
     * |##....##|
     * ||##..###|
     * ||######||
     * |||###||||
     * ||||||||||
     * ||||||||||
     * 
     * After 10 minutes:
     * .||##.....
     * ||###.....
     * ||##......
     * |##.....##
     * |##.....##
     * |##....##|
     * ||##.####|
     * ||#####|||
     * ||||#|||||
     * ||||||||||
     * After 10 minutes, there are 37 wooded acres and 31 lumberyards. Multiplying the number of wooded acres by the number of lumberyards gives the total resource value after ten minutes: 37 * 31 = 1147.
     * 
     * What will the total resource value of the lumber collection area be after 10 minutes?
     */
    public class Day18 : Day
    {
        public override int PuzzleDay => 18;
        private char[,] Cur;
        private char[,] Next;
        private int YardWidth;
        private int YardHeight;
        private const char Open = '.';
        private const char Trees = '|';
        private const char Lumber = '#';
        private Tuple<int, int> CursorPos;

        private string[] testInput = {
                ".#.#...|#.",
                ".....#|##|",
                ".|..|...#.",
                "..|#.....#",
                "#.#|||#|#|",
                "...#.||...",
                ".|....|...",
                "||...#|.#|",
                "|.||||..|.",
                "...#.|..|."};

        protected override void ProcessInput()
        {
            CursorPos = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
        }

        private void Reset(string[] input)
        {
            YardHeight = input.Length;
            YardWidth = input[0].Length;
            Cur = new char[YardWidth, YardHeight];
            Next = new char[YardWidth, YardHeight];

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input.Length; x++)
                {
                    Cur[x, y] = input[y][x];
                }
            }

            Console.WindowHeight = Math.Max(Math.Min(YardHeight + YardHeight / 3, 52), Console.WindowHeight);
            Console.CursorVisible = false;
        }

        private void RunPuzzle(int minutes, bool printYard = false)
        {
            if(printYard) PrintYard(0);
            for (int minute = 1; minute <= minutes; minute++)
            {
                for (int x = 0; x < YardWidth; x++)
                {
                    for (int y = 0; y < YardHeight; y++)
                    {
                        Next[x, y] = GetNextIteration(x, y);
                    }
                }

                char[,] tmp = Cur;
                Cur = Next;
                Next = tmp;
                if(printYard) PrintYard(minute);
                if (minute % 10000 == 0) Console.Write('.');
                
            }
        }

        protected override string Part1()
        {
            Reset(testInput);
            RunPuzzle(1000, true);

            int treeCt = 0;
            int lumberCt = 0;
            foreach(char c in Cur)
            {
                if (c == Trees) treeCt++;
                if (c == Lumber) lumberCt++;
            }

            return $"{treeCt} trees x {lumberCt} lumber = {treeCt * lumberCt}";
        }

        protected override string Part2()
        {
            int wow = 1000000000;
            // dumb brute force ughhh
            Reset(Input);
            RunPuzzle(wow);
            int treeCt = 0;
            int lumberCt = 0;
            foreach (char c in Cur)
            {
                if (c == Trees) treeCt++;
                if (c == Lumber) lumberCt++;
            }

            return $"{treeCt} trees x {lumberCt} lumber = {treeCt * lumberCt}";
        }

        private IEnumerable<char> GetNeighbors(int x, int y)
        {
            if (y > 0)
            {
                if (x > 0) yield return Cur[x - 1, y - 1];
                yield return Cur[x, y - 1];
                if (x < YardWidth - 1) yield return Cur[x + 1, y - 1];
            }

            if (x > 0) yield return Cur[x - 1, y];
            if (x < YardWidth - 1) yield return Cur[x + 1, y];

            if (y < YardHeight - 1)
            {
                if (x > 0) yield return Cur[x - 1, y + 1];
                yield return Cur[x, y + 1];
                if (x < YardWidth - 1) yield return Cur[x + 1, y + 1];
            }
        }

        private char GetNextIteration(int x, int y)
        {
            var neighbors = GetNeighbors(x, y).ToList();
            char next = Open;
            switch (Cur[x, y])
            {
                case Open:
                    if (neighbors.Count(c => c == Trees) >= 3) next = Trees;
                    else next = Open;
                    break;
                case Trees:
                    if (neighbors.Count(c => c == Lumber) >= 3) next = Lumber;
                    else next = Trees;
                    break;
                case Lumber:
                    if (neighbors.Count(c => c == Lumber) > 0 && neighbors.Count(c => c == Trees) > 0) next = Lumber;
                    else next = Open;
                    break;
                default:
                    break;
            }

            return next;
        }

        private void PrintYard(int minute)
        {
            Console.CursorLeft = CursorPos.Item1;
            Console.CursorTop = CursorPos.Item2 + 1;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"After minute {minute}:");
            for(int y = 0; y < YardHeight; y++)
            {
                for(int x = 0; x < YardWidth; x++)
                {
                    Console.ForegroundColor = GetColor(Cur[x, y]);
                    Console.Write(Cur[x, y]);
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private ConsoleColor GetColor(char c)
        {
            switch(c)
            {
                case Open:
                    return ConsoleColor.White;
                case Trees:
                    return ConsoleColor.Green;
                case Lumber:
                    return ConsoleColor.DarkYellow;
                default:
                    return ConsoleColor.White;
            }
        }

    }
}
