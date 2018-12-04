using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    public class Day4 : Day
    {
        public Day4()
        {
            string[] input = ReadInput("Day4/Day4Input.txt");
            IList<LogEntry> logs = new List<LogEntry>(input.Length);
            foreach (string str in input)
            {
                logs.Add(new LogEntry(str));
            }

            logs = logs.OrderBy(l => l.When).ToList();

        }

        public override int PuzzleDay => 4;

        protected override string Part1()
        {
            throw new NotImplementedException();
        }

        protected override string Part2()
        {
            throw new NotImplementedException();
        }

        public class LogEntry
        {
            public LogEntry(string input)
            {
                // "[1518-09-09 00:04] Guard #1543 begins shift"
                When = DateTime.Parse(input.Substring(1, 16).Replace("1518", "2018"));
                What = input.Substring(18).Trim();
            }

            public DateTime When { get; private set; }

            public string What { get; private set; }
        }

       
    }
}
