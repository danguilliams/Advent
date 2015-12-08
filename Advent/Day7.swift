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

--- Part Two ---

Now, take the signal you got on wire a, override wire b to that signal, and reset the other wires (including wire a). What new signal is ultimately provided to wire a?

Your puzzle answer was 3176.

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
        instructions.filter{ $0.targetWireId == "b" }.forEach { $0.evaluated = true }
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
            wires[instruction.targetWireId] = Wire(id:instruction.targetWireId)
            TryAddSignal(instruction.inWire1)
            TryAddSignal(instruction.inWire2)
        }
    }
    
    func RunInstructions() {
        var remaining:Bool = true
        
        while remaining {
            
            for i in instructions {
                if(!i.evaluated)
                {
                    switch(i.op) {
                    case Op.And:
                        EvalAnd(i)
                    case Op.Or:
                        EvalOr(i)
                    case Op.Not:
                        EvalNot(i)
                    case Op.RShift:
                        EvalRShift(i)
                    case Op.LShift:
                        EvalLShift(i)
                    case Op.Pure:
                        EvalPure(i)
                    }
                }
            }
            
            let remainingCt = instructions.filter{ !$0.evaluated }.count
            remaining = remainingCt > 0
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
    
    private func EvalAnd(i:Instruction) {
        if wires[i.inWire1] != nil &&
            wires[i.inWire1]!.resolved &&
            wires[i.inWire2] != nil &&
            wires[i.inWire2]!.resolved {
            
                wires[i.targetWireId]!.signal = wires[i.inWire1]!.signal & wires[i.inWire2]!.signal
                wires[i.targetWireId]!.resolved = true
                i.evaluated = true
        }
    }
    
    private func EvalOr(i:Instruction) {
        if wires[i.inWire1] != nil &&
           wires[i.inWire1]!.resolved &&
           wires[i.inWire2] != nil &&
           wires[i.inWire2]!.resolved {
           
            wires[i.targetWireId]!.signal = wires[i.inWire1]!.signal | wires[i.inWire2]!.signal
            wires[i.targetWireId]!.resolved = true
            i.evaluated = true
        }
    }
    
    private func EvalNot(i:Instruction) {
        if wires[i.inWire1] != nil &&
            wires[i.inWire1]!.resolved {
            
                wires[i.targetWireId]!.signal = ~wires[i.inWire1]!.signal
                wires[i.targetWireId]!.resolved = true
                i.evaluated = true
        }
    }
    
    private func EvalPure(i:Instruction) {
        if wires[i.inWire1] != nil &&
            wires[i.inWire1]!.resolved {
                wires[i.targetWireId]!.signal = wires[i.inWire1]!.signal
                wires[i.targetWireId]!.resolved = true
                i.evaluated = true
        }
    }
    
    private func EvalRShift(i:Instruction) {
        if wires[i.inWire1] != nil &&
            wires[i.inWire1]!.resolved &&
            wires[i.inWire2] != nil &&
            wires[i.inWire2]!.resolved {
            
                wires[i.targetWireId]!.signal = wires[i.inWire1]!.signal >> wires[i.inWire2]!.signal
                wires[i.targetWireId]!.resolved = true
                i.evaluated = true
        }
    }
    
    private func EvalLShift(i:Instruction) {
        if wires[i.inWire1] != nil &&
            wires[i.inWire1]!.resolved &&
            wires[i.inWire2] != nil &&
            wires[i.inWire2]!.resolved {
            
                wires[i.targetWireId]!.signal = wires[i.inWire1]!.signal << wires[i.inWire2]!.signal
                wires[i.targetWireId]!.resolved = true
                i.evaluated = true
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
        
        init(id:String)
        {
            self.id = id
        }
    }
    
    
    // holds a definition of a instruction, which defines target node, and operation, and its input wires
    class Instruction {
        var targetWireId:String
        var op:Op
        var inWire1:String
        var inWire2:String
        var evaluated:Bool = false
        
        init(str:String) {
            
            let tokens = str.characters.split { $0 == " " }.map(String.init)
            
            self.targetWireId = tokens[tokens.count - 1]
            
            if tokens[0] == "NOT" {
                
                self.op = Op.Not
                self.inWire1 = tokens[1]
                self.inWire2 = self.inWire1
            } else if tokens.count == 5 {// AND, OR, RSHIFT and LSHIFT nodes
                
                self.inWire1 = tokens[0]
                self.inWire2 = tokens[2]
                
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
                self.inWire1 = tokens[0]
                self.inWire2 = inWire1
            }
        }
    }

}