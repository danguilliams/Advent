//
//  Day12.swift
//  Advent
//
//  Created by Dan on 12/11/15.
//  Copyright © 2015 Dan. All rights reserved.
//
/*

--- Day 12: JSAbacusFramework.io ---

Santa's Accounting-Elves need help balancing the books after a recent order. Unfortunately, their accounting software uses a peculiar storage format. That's where you come in.

They have a JSON document which contains a variety of things: arrays ([1,2,3]), objects ({"a":1, "b":2}), numbers, and strings. Your first job is to simply find all of the numbers throughout the document and add them together.

For example:

[1,2,3] and {"a":2,"b":4} both have a sum of 6.
[[[3]]] and {"a":{"b":4},"c":-1} both have a sum of 3.
{"a":[-1,1]} and [-1,{"a":1}] both have a sum of 0.
[] and {} both have a sum of 0.
You will not encounter any strings containing numbers.
    
    What is the sum of all numbers in the document?

Your puzzle answer was 156366.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

Uh oh - the Accounting-Elves have realized that they double-counted everything red.

Ignore any object (and all of its children) which has any property with the value "red". Do this only for objects ({...}), not arrays ([...]).

[1,2,3] still has a sum of 6.
[1,{"c":"red","b":2},3] now has a sum of 4, because the middle object is ignored.
{"d":"red","e":[1,2,3,4],"f":5} now has a sum of 0, because the entire structure is ignored.
[1,"red",5] has a sum of 6, because "red" in an array has no effect.

*/

import Foundation

class Day12 : DayBase {
    
    init() {
        super.init(day:12, fileName:"advent12.txt")
    }
    
    override func DoSolve() {
        Solve1()
        Solve2()
    }
    
    private func Solve1() {
        let tokens = puzzleContent.characters.split{ "[]\",:{}".characters.contains($0)}.map(String.init)
    
        var sum = 0
        for t in tokens {
            let val = Int(t)
            if val != nil {
                sum += val!
            }
        }
    
        print("  Part 1 sum: \(sum)")
    }
    
    private func Solve2() {
        do {
            let data = try NSJSONSerialization.JSONObjectWithData(puzzleContent.dataUsingEncoding(NSUTF8StringEncoding)!, options: .AllowFragments) as! [AnyObject]
    
            let sum = Sum(data)
            
            print("  Part 2 sum: \(sum)")
            
            } catch {
                print("Part 2 failed!")
        }
    }
    
    private func Sum(a:[AnyObject]) -> Int {
        var sum:Int = 0
        for o in a {
            if let val = o as? NSNumber {
                sum += val.integerValue
            } else {
                sum += CastAndSum(o)
            }
            
        }
        
        return sum
    }
    
    private func Sum(a:[String:AnyObject]) -> Int {
        var sum:Int = 0
        
        for (_,v) in a {
            if let color = v as? NSString {
                if color == "red" {
                    return 0
                }
            }
            else if let val = v as? NSNumber {
                sum += val.integerValue
            }
            else {
                sum += CastAndSum(v)
            }
        }

        return sum
    }
    
    private func CastAndSum(a:AnyObject) -> Int {
        if let arr = a as? [AnyObject] {
            return Sum(arr)
        }
        else if let dict = a as? [String:AnyObject] {
            return Sum(dict)
        }
        else {
            return 0
        }
    }
}