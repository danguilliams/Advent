//
//  Day5.swift
//  Advent
//
//  Created by Dan on 12/5/15.
//  Copyright © 2015 Dan. All rights reserved.
//  from www.adventofcode.com
/*

--- Day 5: Doesn't He Have Intern-Elves For This? ---

Santa needs help figuring out which strings in his text file are naughty or nice.

A nice string is one with all of the following properties:

It contains at least three vowels (aeiou only), like aei, xazegov, or aeiouaeiouaeiou.
It contains at least one letter that appears twice in a row, like xx, abcdde (dd), or aabbccdd (aa, bb, cc, or dd).
It does not contain the strings ab, cd, pq, or xy, even if they are part of one of the other requirements.
For example:

ugknbfddgicrmopn is nice because it has at least three vowels (u...i...o...), a double letter (...dd...), and none of the disallowed substrings.
aaa is nice because it has at least three vowels and a double letter, even though the letters used by different rules overlap.
jchzalrnumimnmhp is naughty because it has no double letter.
haegwjzuvuyypxyu is naughty because it contains the string xy.
dvszwmarrgswjxmb is naughty because it contains only one vowel.
How many strings are nice?

Your puzzle answer was 255.

*/

import Foundation

class Day5 : DayBase {
    
    init() {
        super.init(day:5, filePath:"/Users/danielguilliams/Documents/Playground/advent5.txt")
    }
    
    override func DoSolve() {
        let lines = puzzleContent.characters.split { $0 == "\n" || $0 == "\r\n" }.map(String.init)
        Solve1(lines)
        Solve2(lines)
        print("")
    }
    
    private func Solve1(let lines: [String]) {
        
        var niceCount:Int = 0
        
        for s in lines {
            
            var dupChars:Bool = false;
            var lastChar:Character = "-"
            
            for c in s.characters {
                if c == lastChar {
                    dupChars = true;
                    break
                }
                lastChar = c
            }
            
            let enoughVowels:Bool = s.characters.filter { "aeiou".characters.contains($0) }.count >= 3
            
            let noBadStrings = !s.containsString("ab") && !s.containsString("cd") &&
                !s.containsString("pq") && !s.containsString("xy")
            
            if dupChars && enoughVowels && noBadStrings {
                niceCount += 1
            }
        }
        
        print("  Part 1 'nice' strings: \(niceCount)")
        
    }
    
    /* 
    
    --- Part Two ---
    
    Realizing the error of his ways, Santa has switched to a better model of determining whether a string is naughty or nice. None of the old rules apply, as they are all clearly ridiculous.
    
    Now, a nice string is one with all of the following properties:
    
    It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
    It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
    For example:
    
    qjhvhtzxzqqjkmpb is nice because is has a pair that appears twice (qj) and a letter that repeats with exactly one letter between them (zxz).
    xxyxx is nice because it has a pair that appears twice and a letter that repeats with one between, even though the letters used by each rule overlap.
    uurcxstgmygtbstg is naughty because it has a pair (tg) but no repeat with a single letter between them.
    ieodomkazucvgmuy is naughty because it has a repeating letter with one between (odo), but no pair that appears twice.
    How many strings are nice under these new rules?
    
    Your puzzle answer was 55.
    
    */
    
    private func Solve2(let lines: [String]) {
        
        var niceCount:Int = 0
        
        for s in lines {
            var pairs:Dictionary = [Pair : Int]()
            var hasPair:Bool = false
            var hasDupe:Bool = false
            var lc:Character = "-"
            var i:Int = 0;
            
            for c in s.characters {
                
                let p = Pair(a: lc, b: c)
                
                if pairs[p] == nil {
                    pairs[p] = i - 1 // set index to index of first character in the pair
                } else if pairs[p]! + 2 < i {
                    hasPair = true
                }
                
                if i > 1 && s[s.startIndex.advancedBy(i)] == s[s.startIndex.advancedBy(i - 2)] {
                    hasDupe = true
                }
                
                if hasPair && hasDupe {
                    niceCount += 1
                    break
                }
                
                i += 1
                lc = c
            }
            
        }
        
        print("  Part 2 'nice' strings: \(niceCount)")
    }
    
}