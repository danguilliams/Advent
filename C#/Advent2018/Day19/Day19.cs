using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    /*
     * --- Day 19: Go With The Flow ---
     * With the Elves well on their way constructing the North Pole base, you turn your attention back to understanding the inner workings of programming the device.
     * 
     * You can't help but notice that the device's opcodes don't contain any flow control like jump instructions. The device's manual goes on to explain:
     * 
     * "In programs where flow control is required, the instruction pointer can be bound to a register so that it can be manipulated directly. This way, setr/seti can function as absolute jumps, addr/addi can function as relative jumps, and other opcodes can cause truly fascinating effects."
     * 
     * This mechanism is achieved through a declaration like #ip 1, which would modify register 1 so that accesses to it let the program indirectly access the instruction pointer itself. To compensate for this kind of binding, there are now six registers (numbered 0 through 5); the five not bound to the instruction pointer behave as normal. Otherwise, the same rules apply as the last time you worked with this device.
     * 
     * When the instruction pointer is bound to a register, its value is written to that register just before each instruction is executed, and the value of that register is written back to the instruction pointer immediately after each instruction finishes execution. Afterward, move to the next instruction by adding one to the instruction pointer, even if the value in the instruction pointer was just updated by an instruction. (Because of this, instructions must effectively set the instruction pointer to the instruction before the one they want executed next.)
     * 
     * The instruction pointer is 0 during the first instruction, 1 during the second, and so on. If the instruction pointer ever causes the device to attempt to load an instruction outside the instructions defined in the program, the program instead immediately halts. The instruction pointer starts at 0.
     * 
     * It turns out that this new information is already proving useful: the CPU in the device is not very powerful, and a background process is occupying most of its time. You dump the background process' declarations and instructions to a file (your puzzle input), making sure to use the names of the opcodes rather than the numbers.
     * 
     * For example, suppose you have the following program:
     * 
     * #ip 0
     * seti 5 0 1
     * seti 6 0 2
     * addi 0 1 0
     * addr 1 2 3
     * setr 1 0 0
     * seti 8 0 4
     * seti 9 0 5
     * When executed, the following instructions are executed. Each line contains the value of the instruction pointer at the time the instruction started, the values of the six registers before executing the instructions (in square brackets), the instruction itself, and the values of the six registers after executing the instruction (also in square brackets).
     * 
     * ip=0 [0, 0, 0, 0, 0, 0] seti 5 0 1 [0, 5, 0, 0, 0, 0]
     * ip=1 [1, 5, 0, 0, 0, 0] seti 6 0 2 [1, 5, 6, 0, 0, 0]
     * ip=2 [2, 5, 6, 0, 0, 0] addi 0 1 0 [3, 5, 6, 0, 0, 0]
     * ip=4 [4, 5, 6, 0, 0, 0] setr 1 0 0 [5, 5, 6, 0, 0, 0]
     * ip=6 [6, 5, 6, 0, 0, 0] seti 9 0 5 [6, 5, 6, 0, 0, 9]
     * In detail, when running this program, the following events occur:
     * 
     * The first line (#ip 0) indicates that the instruction pointer should be bound to register 0 in this program. This is not an instruction, and so the value of the instruction pointer does not change during the processing of this line.
     * The instruction pointer contains 0, and so the first instruction is executed (seti 5 0 1). It updates register 0 to the current instruction pointer value (0), sets register 1 to 5, sets the instruction pointer to the value of register 0 (which has no effect, as the instruction did not modify register 0), and then adds one to the instruction pointer.
     * The instruction pointer contains 1, and so the second instruction, seti 6 0 2, is executed. This is very similar to the instruction before it: 6 is stored in register 2, and the instruction pointer is left with the value 2.
     * The instruction pointer is 2, which points at the instruction addi 0 1 0. This is like a relative jump: the value of the instruction pointer, 2, is loaded into register 0. Then, addi finds the result of adding the value in register 0 and the value 1, storing the result, 3, back in register 0. Register 0 is then copied back to the instruction pointer, which will cause it to end up 1 larger than it would have otherwise and skip the next instruction (addr 1 2 3) entirely. Finally, 1 is added to the instruction pointer.
     * The instruction pointer is 4, so the instruction setr 1 0 0 is run. This is like an absolute jump: it copies the value contained in register 1, 5, into register 0, which causes it to end up in the instruction pointer. The instruction pointer is then incremented, leaving it at 6.
     * The instruction pointer is 6, so the instruction seti 9 0 5 stores 9 into register 5. The instruction pointer is incremented, causing it to point outside the program, and so the program ends.
     * What value is left in register 0 when the background process halts?
     */
    public class Day19 : Day
    {
        public override int PuzzleDay => 19;
        private List<Instruction> Instructions = new List<Instruction>();
        private Dictionary<string, Action<int[],Instruction>> Ops = new Dictionary<string, Action<int[],Instruction>>(16);
        private int IP = 0;
        protected override void ProcessInput()
        {
            int nextId = 0;
            foreach(string s in Input)
            {
                if(s.Contains('#'))
                {
                    // #ip 3
                    IP = int.Parse(s.Substring(3));
                }
                else
                {
                    Instructions.Add(new Instruction(nextId++, s));
                }
            }

            //addr (add register) stores into register C the result of adding register A and register B.
            Ops["addr"] = (r, i) => { r[i.C] = r[i.A] + r[i.B]; };

            //addi (add immediate) stores into register C the result of adding register A and value B.
            Ops["addi"] = (r, i) => { r[i.C] = r[i.A] + i.B; };

            //mulr(multiply register) stores into register C the result of multiplying register A and register B.
            Ops["mulr"] = (r, i) => { r[i.C] = r[i.A] * r[i.B]; };

            //muli(multiply immediate) stores into register C the result of multiplying register A and value B.
            Ops["muli"] = (r, i) => { r[i.C] = r[i.A] * i.B; };

            //banr(bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
            Ops["banr"] = (r, i) => {  r[i.C] = r[i.A] & r[i.B]; };

            //bani(bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
            Ops["bani"] = (r, i) => { r[i.C] = r[i.A] & i.B; };

            //borr(bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
            Ops["borr"] = (r, i) => { r[i.C] = r[i.A] | r[i.B]; };

            //bori(bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
            Ops["bori"] = (r, i) => { r[i.C] = r[i.A] | i.B; };

            //setr(set register) copies the contents of register A into register C. (Input B is ignored.)
            Ops["setr"] = (r, i) => { r[i.C] = r[i.A]; };

            //seti(set immediate) stores value A into register C. (Input B is ignored.)
            Ops["seti"] = (r, i) => { r[i.C] = i.A; };

            //gtir(greater - than immediate / register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
            Ops["gtir"] = (r, i) => { r[i.C] = i.A > r[i.B] ? 1 : 0; };

            //gtri(greater - than register / immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            Ops["gtri"] = (r, i) => { r[i.C] = r[i.A] > i.B ? 1 : 0; };

            //gtrr(greater - than register / register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            Ops["gtrr"] = (r, i) => { r[i.C] = r[i.A] > r[i.B] ? 1 : 0; };

            //eqir(equal immediate / register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            Ops["eqir"] = (r, i) => { r[i.C] = i.A == r[i.B] ? 1 : 0; };

            //eqri(equal register / immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            Ops["eqri"] = (r, i) => { r[i.C] = r[i.A] == i.B ? 1 : 0; };

            //eqrr(equal register / register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            Ops["eqrr"] = (r, i) => { r[i.C] = r[i.A] == r[i.B] ? 1 : 0; };
        }

        [DebuggerDisplay("{Id} {IType} {A} {B} {C}")]
        private class Instruction
        {
            public string OpName;
            public int Id, A, B, C;
            public Action<int[], Instruction> Op;  
        }


        protected override string Part1()
        {
            foreach(Instruction i in Instructions)
            {

            }
            return "unsolved";
        }

        protected override string Part2()
        {
            return "unsolved";
        }
    }
}
