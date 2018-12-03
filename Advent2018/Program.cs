using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    public class Program
    {
        static void Main(string[] args)
        {
            Day[] days = new Day[] 
                {
                    //new Day1(),
                    new Day2()
                };

            Console.WriteLine("Starting solutions");
            foreach(Day day in days)
            {
                day.Solve();
            }
            Console.WriteLine("Finished");
            Console.WriteLine();
        }
    }
}
