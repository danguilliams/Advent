//
//  Day7.swift
//  Advent
//
//  Created by Dan on 12/6/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*
--- Day 7: Some Assembly Required ---

This year, Santa brought little Bobby Tables a set of wires and bitwise logic gates! Unfortunately, little Bobby is a little under the recommended age range, and he needs help assembling the circuit.

Each wire has an identifier (some lowercase letters) and can carry a 16-bit signal (a number from 0 to 65535). A signal is provided to each wire by a gate, another wire, or some specific value. Each wire can only get a signal from one source, but can provide its signal to multiple destinations. A gate provides no signal until all of its inputs have a signal.

The included instructions booklet describe how to connect the parts together: x AND y -> z means to connect wires x and y to an AND gate, and then connect its output to wire z.

For example:

123 -> x means that the signal 123 is provided to wire x.
x AND y -> z means that the bitwise AND of wire x and wire y is provided to wire z.
p LSHIFT 2 -> q means that the value from wire p is left-shifted by 2 and then provided to wire q.
NOT e -> f means that the bitwise complement of the value from wire e is provided to wire f.
Other possible gates include OR (bitwise OR) and RSHIFT (right-shift). If, for some reason, you'd like to emulate the circuit instead, almost all programming languages (for example, C, JavaScript, or Python) provide operators for these gates.

For example, here is a simple circuit:

123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i
After it is run, these are the signals on the wires:

d: 72
e: 507
f: 492
g: 114
h: 65412
i: 65079
x: 123
y: 456
In little Bobby's kit's instructions booklet (provided as your puzzle input), what signal is ultimately provided to wire a?

Your puzzle answer was 3176.

--- Part Two ---

Now, take the signal you got on wire a, override wire b to that signal, and reset the other wires (including wire a). What new signal is ultimately provided to wire a?

Your puzzle answer was 14710.

*/

import Foundation

class Day7 : DayBase {
    
    var instructions:[Instruction] = [Instruction]()
    var wires:[String:Wire] = [String:Wire]()
    
    init () {
        super.init(day: 7, filePath:"/Users/danielguilliams/Documents/Playground/advent7.txt")
    }
    
    override func DoSolve() {
        DoSolve1()
        DoSolve2()
    }
    
    func DoSolve2() {
        Reload()
        let newB = Wire(id: "b")
        newB.resolved = true;
        newB.signal = 3176
        wires["b"] = newB
        instructions.filter{ $0.out == "b" }.forEach { $0.evaluated = true }
        RunInstructions()
        print("  Pt 2 Signal on wire 'a': \(wires["a"]!.signal)")
    }
    
    func DoSolve1() {
        Reload()
        RunInstructions()
        print("  Pt 1 Signal on wire 'a': \(wires["a"]!.signal)")
    }
    
    func Reload() {
        instructions = [Instruction]()
        wires = [String:Wire]()
        let lines = puzzleContent.characters.split{ $0 == "\n" || $0 == "\r\n" }.map(String.init)
        
        for l in lines {
            let instruction = Instruction(str: l)
            instructions.append(instruction)
            wires[instruction.out] = Wire(id:instruction.out)
            TryAddSignal(instruction.in1)
            TryAddSignal(instruction.in2)
        }
    }
    
    func RunInstructions() {
        var remaining:Bool = true
        
        while remaining {
            
            for i in instructions {
                
                if !i.evaluated &&
                    wires[i.in1]!.resolved &&
                    wires[i.in2]!.resolved
                {
                    
                    switch(i.op) {
                        
                        case Op.And:
                            wires[i.out]!.signal = wires[i.in1]!.signal & wires[i.in2]!.signal
                        case Op.Or:
                            wires[i.out]!.signal = wires[i.in1]!.signal | wires[i.in2]!.signal
                        case Op.Not:
                            wires[i.out]!.signal = ~wires[i.in1]!.signal
                        case Op.RShift:
                            wires[i.out]!.signal = wires[i.in1]!.signal >> wires[i.in2]!.signal
                        case Op.LShift:
                            wires[i.out]!.signal = wires[i.in1]!.signal << wires[i.in2]!.signal
                        case Op.Pure:
                            wires[i.out]!.signal = wires[i.in1]!.signal
                    }
                    
                    wires[i.out]!.resolved = true
                    i.evaluated = true
                }
            }
            
            remaining = instructions.filter{ !$0.evaluated }.count > 0
        }
    }
    
    private func TryAddSignal(s:String) {
        let a = UInt16(s)
        if a != nil {
            let newWire = Wire(id: s)
            newWire.resolved = true;
            newWire.signal = a!
            wires[s] = newWire
        }
    }
    
    enum Op {
        case Not
        case Or
        case And
        case RShift
        case LShift
        case Pure
    }
    
    class Wire {
        var id:String = ""
        var resolved:Bool = false
        var signal:UInt16 = 0
        
        init(id:String) {
            self.id = id
        }
    }
    
    // holds a definition of a instruction, which defines target node, and operation, and its input wires
    class Instruction {
        var out:String
        var op:Op
        var in1:String
        var in2:String
        var evaluated:Bool = false
        
        init(str:String) {
            
            let tokens = str.characters.split { $0 == " " }.map(String.init)
            
            self.out = tokens[tokens.count - 1]
            
            if tokens[0] == "NOT" {
                self.op = Op.Not
                self.in1 = tokens[1]
                self.in2 = self.in1
            } else if tokens.count == 5 {// AND, OR, RSHIFT and LSHIFT nodes
                self.in1 = tokens[0]
                self.in2 = tokens[2]
                
                if tokens[1] == "AND" {
                    self.op = Op.And
                } else if tokens[1] == "OR" {
                    self.op = Op.Or
                } else if tokens[1] == "RSHIFT" {
                    self.op = Op.RShift
                } else if tokens[1] == "LSHIFT" {
                    self.op = Op.LShift
                } else {
                    self.op = Op.Or
                }
            } else { // direct value
                self.op = Op.Pure
                self.in1 = tokens[0]
                self.in2 = in1
            }
        }
    }
    
}