//
//  Day10.swift
//  Advent
//
//  Created by Dan on 12/10/15.
//  Copyright © 2015 Dan. All rights reserved.
//
/*
--- Day 10: Elves Look, Elves Say ---

Today, the Elves are playing a game called look-and-say. They take turns making sequences by reading aloud the previous sequence and using that reading as the next sequence. For example, 211 is read as "one two, two ones", which becomes 1221 (1 2, 2 1s).

Look-and-say sequences are generated iteratively, using the previous value as input for the next step. For each step, take the previous value, and replace each run of digits (like 111) with the number of digits (3) followed by the digit itself (1).

For example:

1 becomes 11 (1 copy of digit 1).
11 becomes 21 (2 copies of digit 1).
21 becomes 1211 (one 2 followed by one 1).
1211 becomes 111221 (one 1, one 2, and two 1s).
111221 becomes 312211 (three 1s, two 2s, and one 1).
Starting with the digits in your puzzle input, apply this process 40 times. What is the length of the result?

Your puzzle answer was 252594.

--- Part Two ---

Neat, right? You might also enjoy hearing John Conway talking about this sequence (that's Conway of Conway's Game of Life fame).

Now, starting again with the digits in your puzzle input, apply this process 50 times. What is the length of the new result?

Your puzzle answer was 3579328.

Final iteration length: 3579328
Day 10 Finished. Elapsed time: 22.0038089752197 seconds -> printing every iteration results
Day 10 Finished. Elapsed time: 15.4062349796295 seconds -> only printing final results

*/

import Foundation

class Day10 : DayBase {
    var iterations:Int
    
    init() {
        iterations = 50
        super.init(day:10, content:"1113222113")
    }
    
    override func DoSolve() {
        
        let test = GetNewString("111221")
        assert("312211" == test)
        
        var str = puzzleContent

        for var i = 1; i <= iterations; i++ {
            str = GetNewString(str)
            //print("iteration #: \(i) length:\(str.length)")
        }
        
        print("Final iteration length: \(str.length)")
        
    }
    
    func GetNewString(s:String) -> String {
        var new = ""
        var cur = s.characters.first!
        var count = 0
        for c in s.characters {
            if cur == c {
                count++
            } else {
                new.appendContentsOf("\(count)")
                new.append(cur)
                cur = c
                count = 1
            }
        }
        // ensure we add the last count and char
        new.appendContentsOf("\(count)")
        new.append(cur)
    
        return new;
    }

}