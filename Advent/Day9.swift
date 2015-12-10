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

class Edge {
    let from:String
    let to:String
    let weight:Int
    
    init(from:String, to:String, weight:Int) {
        self.from = from
        self.to = to
        self.weight = weight
    }
}


class Day9 : DayBase {
    
    var edges:[Edge] = [Edge]()
    var nodes:[String] = [String]()
    var distances:[Int] = [Int]()
    var paths:[String:Int] = [String:Int]()
    var count:Int = 0
    
    init() {
        super.init(day:9, fileName:"advent9.txt")
        
        let lines = puzzleContent.characters.split { $0 == "\n"}.map(String.init)
        
        for l in lines {
            let tokens = l.characters.split { $0 == " "}.map(String.init)
            assert(tokens.count == 5)

            edges.append(Edge(from:tokens[0], to:tokens[2], weight:Int(tokens[4])!))
            edges.append(Edge(from:tokens[2], to:tokens[0], weight:Int(tokens[4])!))
            
            if !nodes.contains(tokens[2]) {
                nodes.append(tokens[2])
            }
        }
    }
    
    override func DoSolve(){
        // graph is completely connected
        // need to get permutations of nodes
        
        /*
        Heap's algorithm pseudocode - from wikipedia
        
        procedure generate(n : integer, A : array of any):
            if n = 1 then
                output(A)
            else
                for i := 0; i < n - 1; i += 1 do
                    generate(n - 1, A)
                    if n is even then
                        swap(A[i], A[n-1])
                    else
                        swap(A[0], A[n-1])
                    end if
                end for
                generate(n - 1, A)
            end if
        
        */
        
        let n = nodes.count
        Generate(n, s: nodes,onPermute: AddLength)
        
        distances.sortInPlace()
        
        print("unique paths: \(paths.count)")
        //print("duplicate paths: \(paths.values.filter{$0 > 0}.count)")
        print("  Shortest: \(distances.first!)")
        print("  Longest: \(distances.last!)")
        print("")
    }
    
    func AddLength(s:[String]) {
        var distance = 0
        
        for var i = 0; i < s.count - 2; i++ {
            distance += edges.filter{$0.to == s[i] && $0.from == s[i + 1]}.first!.weight
        }
        distances.append(distance)
        
        let path = s.joinWithSeparator(",")
        
         if paths[path] != nil {
            paths[path]! += 1
         } else {
            paths[path] = 0
        }
    }
    
    private func Generate(n:Int, var s:[String], onPermute: ([String]) -> Void ){
        if n == 1 {
            onPermute(s)
        } else {
            for var i = 0; i < n - 1; i += 1 {
                Generate(n - 1, s: s, onPermute:onPermute)
                if n % 2 == 0 {
                    swap(&s[i], &s[n-1])
                } else {
                    swap(&s[0], &s[n-1])
                }
            }
            
            Generate(n - 1, s: s, onPermute:onPermute)
        }
    }
    
    private func Swap<T>(inout s1:T, inout s2:T) {
        let tmp:T = s1
        s1 = s2
        s2 = tmp
    }

}