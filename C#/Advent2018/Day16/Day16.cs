using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    public class Day16 : Day
    {
        public override int PuzzleDay => 16;
        private List<Sample> Samples;
        private List<Instruction> Instructions;
        private List<IOp> Ops;

        protected override void ProcessInput()
        {
            Samples = new List<Sample>();
            Instructions = new List<Instruction>();
            Ops = new List<IOp>();
            for(int i = 0; i < Input.Length; i++)
            {
                if(Input[i].StartsWith("Before"))
                {
                    Samples.Add(new Sample(Input[i], Input[i+1], Input[i+2]));
                    i += 2;
                }
                else if (Input[i].Length > 2 && Input[i].Length < 10)
                {
                    Instructions.Add(new Instruction(Input[i]));
                }
            }

            Ops.Add(new IOp("addr", (r) => r));
            Ops.Add(new IOp("addi", (r) => r));
            Ops.Add(new IOp("mulr", (r) => r));
            Ops.Add(new IOp("muli", (r) => r));
            Ops.Add(new IOp("banr", (r) => r));
            Ops.Add(new IOp("bani", (r) => r));
            Ops.Add(new IOp("borr", (r) => r));
            Ops.Add(new IOp("bori", (r) => r));
            Ops.Add(new IOp("setr", (r) => r));
            Ops.Add(new IOp("seti", (r) => r));
            Ops.Add(new IOp("gtir", (r) => r));
            Ops.Add(new IOp("gtri", (r) => r));
            Ops.Add(new IOp("gtrr", (r) => r));
            Ops.Add(new IOp("eqir", (r) => r));
            Ops.Add(new IOp("eqri", (r) => r));
            Ops.Add(new IOp("eqrr", (r) => r));
        }

        protected override string Part1()
        {
            
            throw new NotImplementedException();
        }

        protected override string Part2()
        {
            throw new NotImplementedException();
        }

        private class IOp
        {
            public IOp(string id, Func<int[], int[]> o)
            {
                Id = id;
                Op = o;
            }

            public string Id;
            public Func<int[], int[]> Op;
        }


        private class Sample
        {
            public Sample(string before, string op, string after)
            {
                Before = ParseOps(before);
                Op = ParseOps(op);
                After = ParseOps(after);
            }

            int[] Before;
            int[] Op;
            int[] After;
        }


        private class Instruction
        {
            public Instruction(string instr)
            {
                Op = ParseOps(instr);
            }

            public int[] Op;
        }

        public static int[] ParseOps(string s)
        {
            if (s[0] == 'B' || s[0] == 'A')
            {
            //After:  [3, 0, 1, 1]
            //Before: [2, 0, 0, 2]
                string[] t = s.Substring(7).Split('[',',', ']');
                return new int[] { int.Parse(t[1]), int.Parse(t[2]), int.Parse(t[3]), int.Parse(t[4]) };
            }
            else
            {
                string[] t = s.Split(' ');
                return new int[] { int.Parse(t[0]), int.Parse(t[1]), int.Parse(t[2]), int.Parse(t[3]) };
            }
        }

    }
}
