//
//  Day9.swift
//  Advent
//
//  Created by Dan on 12/8/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*
--- Day 9: All in a Single Night ---

Every year, Santa manages to deliver all of his presents in a single night.

This year, however, he has some new locations to visit; his elves have provided him the distances between every pair of locations. He can start and end at any two (different) locations he wants, but he must visit each location exactly once. What is the shortest distance he can travel to achieve this?

For example, given the following distances:

London to Dublin = 464
London to Belfast = 518
Dublin to Belfast = 141
The possible routes are therefore:

Dublin -> London -> Belfast = 982
London -> Dublin -> Belfast = 605
London -> Belfast -> Dublin = 659
Dublin -> Belfast -> London = 659
Belfast -> Dublin -> London = 605
Belfast -> London -> Dublin = 982
The shortest of these is London -> Dublin -> Belfast = 605, and so the answer is 605 in this example.

What is the distance of the shortest route?

Your puzzle answer was 207.

--- Part Two ---

The next year, just to show off, Santa decides to take the route with the longest distance instead.

He can still start and end at any two (different) locations he wants, and he still must visit each location exactly once.

For example, given the distances above, the longest route would be 982 via (for example) Dublin -> London -> Belfast.

What is the distance of the longest route?

Your puzzle answer was 804.

*/

import Foundation

class Day9 : DayBase {
    
    var edges:[Edge2] = [Edge2]()
    var nodes:[String] = [String]()
    var count:Int = 0
    var maxDistance:Int = Int.min
    var minDistance:Int = Int.max
    
    init() {
        super.init(day:9, fileName:"advent9.txt")
        
        let lines = puzzleContent.characters.split { $0 == "\n"}.map(String.init)
        
        for l in lines {
            let tokens = l.characters.split { $0 == " "}.map(String.init)
            assert(tokens.count == 5)

            edges.append(Edge2(from:tokens[0], to:tokens[2], weight:Int(tokens[4])!))
            edges.append(Edge2(from:tokens[2], to:tokens[0], weight:Int(tokens[4])!))
            
            if !nodes.contains(tokens[2]) {
                nodes.append(tokens[2])
            }
        }
    }
    
    override func DoSolve(){
        // graph is completely connected
        // need to get permutations of nodes
        nodes.sortInPlace()
        
        CalcLength()
        while !NextPermutation(&nodes) {
            CalcLength()
            
        }
        print("\(count)")
        print("  Pt 1: Shortest Route: \(minDistance)")
        print("  Pt 2: Longest Route: \(maxDistance)")
    }
    
    func CalcLength() {
        count++
        var distance = 0
        
        for var i = 0; i < nodes.count - 2; i++ {
            distance += edges.filter{$0.to == nodes[i + 1] && $0.from == nodes[i]}.first!.weight
        }
        
        if distance < minDistance {
            minDistance = distance
        }
        
        if distance > maxDistance {
            maxDistance = distance
        }
        
    }
}