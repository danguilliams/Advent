using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

            //addr (add register) stores into register C the result of adding register A and register B.
            Ops.Add(new Operation("addr", (r, i) => {
                r[i.C] = r[i.A] + r[i.B];
            }));

            //addi (add immediate) stores into register C the result of adding register A and value B.
            Ops.Add(new Operation("addi", (r, i) => {
                r[i.C] = r[i.A] + i.B;
            }));

            //mulr(multiply register) stores into register C the result of multiplying register A and register B.
            Ops.Add(new Operation("mulr", (r, i) => {
                r[i.C] = r[i.A] * r[i.B];
            }));

            //muli(multiply immediate) stores into register C the result of multiplying register A and value B.
            Ops.Add(new Operation("muli", (r, i) => {
                r[i.C] = r[i.A] * i.B;
            }));

            //banr(bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
            Ops.Add(new Operation("banr", (r, i) => {
                r[i.C] = r[i.A] & r[i.B];
            }));

            //bani(bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
            Ops.Add(new Operation("bani", (r, i) => {
                r[i.C] = r[i.A] & i.B;
            }));

            //borr(bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
            Ops.Add(new Operation("borr", (r, i) => {
                r[i.C] = r[i.A] | r[i.B];
            }));

            //bori(bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
            Ops.Add(new Operation("bori", (r, i) => {
                r[i.C] = r[i.A] | i.B;
            }));

            //setr(set register) copies the contents of register A into register C. (Input B is ignored.)
            Ops.Add(new Operation("setr", (r, i) => {
                r[i.C] = r[i.A];
            }));

            //seti(set immediate) stores value A into register C. (Input B is ignored.)
            Ops.Add(new Operation("seti", (r, i) => {
                r[i.C] = i.A;
            }));

            //gtir(greater - than immediate / register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
            Ops.Add(new Operation("gtir", (r, i) => {
                r[i.C] = i.A > r[i.B] ? 1 : 0; 
            }));

            //gtri(greater - than register / immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            Ops.Add(new Operation("gtri", (r, i) => {
                r[i.C] = r[i.A] > i.B ? 1 : 0;
            }));

            //gtrr(greater - than register / register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            Ops.Add(new Operation("gtrr", (r, i) => {
                r[i.C] = r[i.A] > r[i.B] ? 1 : 0;
            }));

            //eqir(equal immediate / register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            Ops.Add(new Operation("eqir", (r, i) => {
                r[i.C] = i.A == r[i.B] ? 1 : 0;
            }));

            //eqri(equal register / immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            Ops.Add(new Operation("eqri", (r, i) => {
                r[i.C] = r[i.A] == i.B ? 1 : 0;
            }));

            //eqrr(equal register / register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            Ops.Add(new Operation("eqrr", (r, i) => {
                r[i.C] = r[i.A] == r[i.B] ? 1 : 0;
            }));

        }

        protected override string Part1()
        {
            int totalPolyopSamples = 0;
            int[] regs = new int[] { 0, 0, 0, 0 };
            foreach(Sample s in Samples)
            {
                int matches = 0;

                foreach (Operation op in Ops)
                {
                    // init registers
                    for (int i = 0; i < 4; i++)
                    {
                        regs[i] = s.Before[i];
                    }
                    // do the op
                    op.Execute(regs, s.Instr);
                    // check if output matches
                    bool isMatch = true;
                    for (int i = 0; i < 4; i++)
                    {
                        if (regs[i] != s.After[i])
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        matches++;
                        s.MatchingOps.Add(op);
                    }
                }

                if (matches >= 3)
                {
                    totalPolyopSamples++;
                }
            }

            return $"{totalPolyopSamples} samples that match 3+ opcodes";
        }

        protected override string Part2()
        {
            Dictionary<int, Operation> opDict = new Dictionary<int, Operation>();
            List<Sample> remaining = Samples.ToList();
            // handle simple cases where we know for sure which op code a instruction is
            while (remaining.Any(s => s.MatchingOps.Count == 1))
            {
                Sample unique = remaining.Where(s => s.MatchingOps.Count == 1).Last();
                int currentOpCode = unique.Instr.OpCode;

                Operation op = Ops.First(o => o == unique.MatchingOps[0]);
                if(!opDict.ContainsKey(currentOpCode))
                {
                    opDict[currentOpCode] = op;
                }
                // remove this op from any other samples that matched
                foreach (Sample s in remaining)
                {
                    s.MatchingOps.Remove(op);
                }
                remaining.RemoveAll(s => s.Instr.OpCode == currentOpCode);
            }

            if(opDict.Count != Ops.Count)
            {
                throw new Exception("Op Codes have not all been determined!");
            }

            int[] regs = { 0, 0, 0, 0 };
            foreach(Instruction i in Instructions)
            {
                opDict[i.OpCode].Execute(regs, i);
            }

            return $"Register 0 = {regs[0]}";
        }

        [DebuggerDisplay("{Id}")]
        private class Operation
        {
            public Operation(string id, Action<int[], Instruction> op)
            {
                Id = id;
                Execute = op;
            }
            public Action<int[], Instruction> Execute;
            public string Id;
        }

        private class Sample
        {
            public Sample(int id, string before, string instr, string after)
            {
                Before = ParseOps(before);
                Instr = new Instruction(instr);
                After = ParseOps(after);
                MatchingOps = new List<Operation>();
                Id = id;
            }

            public int Id;
            public int[] Before;
            public Instruction Instr;
            public int[] After;
            public List<Operation> MatchingOps;
        }

        [DebuggerDisplay("Op:{OpCode} A:{A},B:{B},C:{C}")]
        private class Instruction
        {
            public Instruction(string instr)
            {
                P = ParseOps(instr);
                if(P.Length != 4)
                {
                    throw new Exception("Incorrect Op legth!");
                }
            }

            private int[] P;
            public int OpCode => P[0];
            public int A => P[1];
            public int B => P[2];
            public int C => P[3];
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
