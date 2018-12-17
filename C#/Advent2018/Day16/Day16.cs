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
        private List<Operation> Ops;

        protected override void ProcessInput()
        {
            Samples = new List<Sample>();
            Instructions = new List<Instruction>();
            Ops = new List<Operation>();
            int sampleNum = 0;

            for(int i = 0; i < Input.Length; i++)
            {
                if(Input[i].StartsWith("Before"))
                {
                    Samples.Add(new Sample(sampleNum++, Input[i], Input[i+1], Input[i+2]));
                    i += 2;
                }
                else if (Input[i].Length > 2 && Input[i].Length < 10)
                {
                    Instructions.Add(new Instruction(Input[i]));
                }
            }

            // in i => i[1] = A, i[2] = B, i[3] = C
            //addr (add register) stores into register C the result of adding register A and register B.
            Ops.Add(new Operation("addr", (r, i) => {
                r[i[3]] = r[i[1]] + r[i[2]];
            }));

            //addi (add immediate) stores into register C the result of adding register A and value B.
            Ops.Add(new Operation("addi", (r, i) => {
                r[3] = r[i[1]] + i[2];
            }));

            Ops.Add(new Operation("mulr", (r, i) => {
            }));

            Ops.Add(new Operation("muli", (r, i) => {
            }));

            Ops.Add(new Operation("banr", (r, i) => {
            }));

            Ops.Add(new Operation("bani", (r, i) => {
            }));

            Ops.Add(new Operation("borr", (r, i) => {
            }));

            Ops.Add(new Operation("bori", (r, i) => {
            }));

            Ops.Add(new Operation("setr", (r, i) => {
            }));

            Ops.Add(new Operation("seti", (r, i) => {
            }));

            Ops.Add(new Operation("gtir", (r, i) => {
            }));

            Ops.Add(new Operation("gtri", (r, i) => {
            }));

            Ops.Add(new Operation("gtrr", (r, i) => {
            }));

            Ops.Add(new Operation("eqir", (r, i) => {
            }));

            Ops.Add(new Operation("eqri", (r, i) => {
            }));

            Ops.Add(new Operation("eqrr", (r, i) => {
            }));

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
            public IOp(int[] instruction)
            {
                Id = instruction[0];
                I = instruction;
            }

            public int Id;
            public int[] I;
            public void Execute(int[] r, Operation op)
            {
                op.Op(r, I);
            }
        }

        private class Operation
        {
            public Operation(string id, Action<int[], int[]> op)
            {
                Id = id;
                Op = op;
            }
            public Action<int[], int[]> Op;
            public string Id;
        }

        private class Sample
        {
            public Sample(int id, string before, string op, string after)
            {
                Before = ParseOps(before);
                Op = new IOp(ParseOps(op));
                After = ParseOps(after);
            }

            public int Id;
            public int[] Before;
            public IOp Op;
            public int[] After;
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
