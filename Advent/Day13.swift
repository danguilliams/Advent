//
//  Day13.swift
//  Advent
//
//  Created by Dan on 12/12/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*

--- Day 13: Knights of the Dinner Table ---

In years past, the holiday feast with your family hasn't gone so well. Not everyone gets along! This year, you resolve, will be different. You're going to find the optimal seating arrangement and avoid all those awkward conversations.

You start by writing up a list of everyone invited and the amount their happiness would increase or decrease if they were to find themselves sitting next to each other person. You have a circular table that will be just big enough to fit everyone comfortably, and so each person will have exactly two neighbors.

For example, suppose you have only four attendees planned, and you calculate their potential happiness as follows:

Alice would gain 54 happiness units by sitting next to Bob.
Alice would lose 79 happiness units by sitting next to Carol.
Alice would lose 2 happiness units by sitting next to David.
Bob would gain 83 happiness units by sitting next to Alice.
Bob would lose 7 happiness units by sitting next to Carol.
Bob would lose 63 happiness units by sitting next to David.
Carol would lose 62 happiness units by sitting next to Alice.
Carol would gain 60 happiness units by sitting next to Bob.
Carol would gain 55 happiness units by sitting next to David.
David would gain 46 happiness units by sitting next to Alice.
David would lose 7 happiness units by sitting next to Bob.
David would gain 41 happiness units by sitting next to Carol.
Then, if you seat Alice next to David, Alice would lose 2 happiness units (because David talks so much), but David would gain 46 happiness units (because Alice is such a good listener), for a total change of 44.

If you continue around the table, you could then seat Bob next to Alice (Bob gains 83, Alice gains 54). Finally, seat Carol, who sits next to Bob (Carol gains 60, Bob loses 7) and David (Carol gains 55, David gains 41). The arrangement looks like this:

+41 +46
+55   David    -2
Carol       Alice
+60    Bob    +54
-7  +83
After trying every other seating arrangement in this hypothetical scenario, you find that this one is the most optimal, with a total change in happiness of 330.

What is the total change in happiness for the optimal seating arrangement of the actual guest list?

Your puzzle answer was 709.

--- Part Two ---

In all the commotion, you realize that you forgot to seat yourself. At this point, you're pretty apathetic toward the whole thing, and your happiness wouldn't really go up or down regardless of who you sit next to. You assume everyone else would be just as ambivalent about sitting next to you, too.

So, add yourself to the list, and give all happiness relationships that involve you a score of 0.

What is the total change in happiness for the optimal seating arrangement that actually includes yourself?

Your puzzle answer was 668.

*/

import Foundation

class Edge {
    var to:Int
    var from:Int
    var weight:Int
    
    init(t:Int,f:Int,w:Int) {
        self.to = t
        self.from = f
        self.weight = w
    }
}

class Day13 : DayBase {
    var maxHappiness:Int = Int.min
    var maxArrangement:String = ""
    var edges:[Edge] = [Edge]()
    var nodes:[String:Int] = [String:Int]()
    var idx:[Int] = [Int]()
    let myName = "Dan"
    
    init() {
        super.init(day:13, fileName:"advent13.txt")
        
        let lines = puzzleContent.characters.split { $0 == "\n"}.map(String.init)
        // Typical line:
        // Bob would gain 11 happiness units by sitting next to Mallory.
        //  0    1     2   3      4      5    6    7      8   9   10
        //
        // Strings are too slow for comparison, so we're using ints instead
        var i = 0
        for l in lines {
            let tokens = l.characters.split { $0 == " " || $0 == "."}.map(String.init)
            assert(tokens.count == 11)
            
            if nodes[tokens[10]] == nil {
                nodes[tokens[10]] = i
                idx.append(i)
                i += 1
            }
            
            if nodes[tokens[0]] == nil{
                nodes[tokens[0]] = i
                idx.append(i)
                i += 1
            }
            
            var weight = Int(tokens[3])!
            
            if tokens[2] == "lose" {
                weight = -weight
            }
            
            edges.append(Edge(t:nodes[tokens[10]]!, f:nodes[tokens[0]]!, w:weight))
        }
    }
    
    override func DoSolve() {
        RunHappinessCalc()
        print("  Pt 1: Maximum Happiness: \(maxHappiness) Arrangement:\(maxArrangement)" )
        
        AddMyselfToTheParty()
        RunHappinessCalc()
        print("  Pt 2: Maximum Happiness: \(maxHappiness) Arrangement:\(maxArrangement)")
    }
    
    private func RunHappinessCalc() {
        ResetCounts()
        CalculateHappiness()
        while !NextPermutation(&idx) {
            CalculateHappiness()
        }
    }
    
    private func ResetCounts() {
        maxHappiness = Int.min
        idx = [Int](count:nodes.count, repeatedValue:0)
        for var i = 0 ; i < nodes.count; i++ {
            idx[i] = i
        }
    }
    
    func CalculateHappiness() {
        var happy = 0
        let n = nodes.count
        for var i = 0; i < n; i++ {
            let l = i == 0 ? n - 1 : i - 1
            let r = i == n - 1 ? 0 : i + 1
            
            happy += edges.filter{$0.from == idx[i] && $0.to == idx[l]}.first!.weight
            happy += edges.filter{$0.from == idx[i] && $0.to == idx[r]}.first!.weight
        }
        
        if happy > maxHappiness {
            maxHappiness = happy
            
            var path = ""
            for var i = 0; i < idx.count; i++ {
                path += nodes.filter{ $1 == idx[i] }.first!.0 + " "
            }
            
            maxArrangement = path
        }
    }
    
    private func AddMyselfToTheParty() {
        let me = nodes.count

        // i am ambivalence
        for n in nodes.values {
            edges.append(Edge(t:n, f:me, w:0))
            edges.append(Edge(t:me, f:n, w:0))
        }
        
        nodes[myName] = me
    }
}