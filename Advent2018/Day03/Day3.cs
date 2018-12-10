using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    /*
     * --- Day 3: No Matter How You Slice It ---
     * The Elves managed to locate the chimney-squeeze prototype fabric for Santa's suit (thanks to someone who helpfully wrote its box IDs on the wall of the warehouse in the middle of the night). Unfortunately, anomalies are still affecting them - nobody can even agree on how to cut the fabric.
     * 
     * The whole piece of fabric they're working on is a very large square - at least 1000 inches on each side.
     * 
     * Each Elf has made a claim about which area of fabric would be ideal for Santa's suit. All claims have an ID and consist of a single rectangle with edges parallel to the edges of the fabric. Each claim's rectangle is defined as follows:
     * 
     * The number of inches between the left edge of the fabric and the left edge of the rectangle.
     * The number of inches between the top edge of the fabric and the top edge of the rectangle.
     * The width of the rectangle in inches.
     * The height of the rectangle in inches.
     * A claim like #123 @ 3,2: 5x4 means that claim ID 123 specifies a rectangle 3 inches from the left edge, 2 inches from the top edge, 5 inches wide, and 4 inches tall. Visually, it claims the square inches of fabric represented by # (and ignores the square inches of fabric represented by .) in the diagram below:
     * 
     * ...........
     * ...........
     * ...#####...
     * ...#####...
     * ...#####...
     * ...#####...
     * ...........
     * ...........
     * ...........
     * The problem is that many of the claims overlap, causing two or more claims to cover part of the same areas. For example, consider the following claims:
     * 
     * #1 @ 1,3: 4x4
     * #2 @ 3,1: 4x4
     * #3 @ 5,5: 2x2
     * Visually, these claim the following areas:
     * 
     * ........
     * ...2222.
     * ...2222.
     * .11XX22.
     * .11XX22.
     * .111133.
     * .111133.
     * ........
     * The four square inches marked with X are claimed by both 1 and 2. (Claim 3, while adjacent to the others, does not overlap either of them.)
     * 
     * If the Elves all proceed with their own plans, none of them will have enough fabric. How many square inches of fabric are within two or more claims?
     * 
     * --- Part Two ---
     * Amidst the chaos, you notice that exactly one claim doesn't overlap by even a single square inch of fabric with any other claim. If you can somehow draw attention to it, maybe the Elves will be able to make Santa's suit after all!
     * 
     * For example, in the claims above, only claim 3 is intact after all claims are made.
     * 
     * What is the ID of the only claim that doesn't overlap?
     */
    public class Day3 : Day
    {
        public Day3()
        {
            string[] input = (string[])ReadInput("Day03/Day3Input.txt");
            Claims = new List<Claim>(input.Length);
            foreach(string str in input)
            {
                Claims.Add(new Claim(str));
            }
            FabricSize = 1000;
            Fabric = new int[FabricSize, FabricSize];
        }


        public override int PuzzleDay => 3;

        public int[,] Fabric { get; private set; }

        public int FabricSize { get; private set; }

        public IList<Claim> Claims { get; private set; }

        protected override string Part1()
        {
            foreach(Claim c in Claims)
            {
                ApplyClaim(c);
            }

            int sum = 0;
            for (int i = 0; i < FabricSize; i++)
            {
                for (int j = 0; j < FabricSize; j++)
                {
                    if (Fabric[i,j] > 1)
                    {
                        sum++;
                    }
                }
            }
            return sum.ToString();
        }

        protected override string Part2()
        {
            // all claims are applied from part one.
            // this time, instead of applying each claim, we check each claim after all have been applied.
            // the claim whose values are all '1' is the claim with no overlaps.
            Claim validClaim = null;
            foreach(Claim c in Claims)
            {
                if(ValidateClaim(c))
                {
                    validClaim = c;
                    break;
                }
            }

            return validClaim != null ? validClaim.ID : "No valid claim found :(";
        }

        private void ApplyClaim(Claim c)
        {
            for(int i = c.Column; i < c.Column + c.Width; i++)
            {
                for(int j = c.Row; j < c.Row + c.Height; j++)
                {
                    Fabric[i, j] += 1;
                }
            }
        }

        private bool ValidateClaim(Claim c)
        {
            for (int i = c.Column; i < c.Column + c.Width; i++)
            {
                for (int j = c.Row; j < c.Row + c.Height; j++)
                {
                    if (Fabric[i, j] != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public class Claim
        {
            public Claim (string input)
            {
                // input is formatted like so: 
                // #1 @ 45,64: 22x22
                // #id @ Col,Row: WxH
                string[] tokens = input.Split('@', ',', ':', 'x');
                ID = tokens[0];
                Column = int.Parse(tokens[1]);
                Row = int.Parse(tokens[2]);
                Width = int.Parse(tokens[3]);
                Height = int.Parse(tokens[4]);
            }

            public string ID { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }


}
