//
//  Day8.swift
//  Advent
//
//  Created by Dan on 12/7/15.
//  Copyright © 2015 Dan. All rights reserved.
//
/* 

--- Day 8: Matchsticks ---

Space on the sleigh is limited this year, and so Santa will be bringing his list as a digital copy. He needs to know how much space it will take up when stored.

It is common in many programming languages to provide a way to escape special characters in strings. For example, C, JavaScript, Perl, Python, and even PHP handle special characters in very similar ways.

However, it is important to realize the difference between the number of characters in the code representation of the string literal and the number of characters in the in-memory string itself.

For example:

"" is 2 characters of code (the two double quotes), but the string contains zero characters.
"abc" is 5 characters of code, but 3 characters in the string data.
"aaa\"aaa" is 10 characters of code, but the string itself contains six "a" characters and a single, escaped quote character, for a total of 7 characters in the string data.
"\x27" is 6 characters of code, but the string itself contains just one - an apostrophe ('), escaped using hexadecimal notation.
Santa's list is a file that contains many double-quoted string literals, one on each line. The only escape sequences used are \\ (which represents a single backslash), \" (which represents a lone double-quote character), and \x plus two hexadecimal characters (which represents a single character with that ASCII code).

Disregarding the whitespace in the file, what is the number of characters of code for string literals minus the number of characters in memory for the values of the strings in total for the entire file?

For example, given the four strings above, the total number of characters of string code (2 + 5 + 10 + 6 = 23) minus the total number of characters in memory for string values (0 + 3 + 7 + 1 = 11) is 23 - 11 = 12.

Your puzzle answer was 1350.

--- Part Two ---

Now, let's go the other way. In addition to finding the number of characters of code, you should now encode each code representation as a new string and find the number of characters of the new encoded representation, including the surrounding double quotes.

For example:

"" encodes to "\"\"", an increase from 2 characters to 6.
"abc" encodes to "\"abc\"", an increase from 5 characters to 9.
"aaa\"aaa" encodes to "\"aaa\\\"aaa\"", an increase from 10 characters to 16.
"\x27" encodes to "\"\\x27\"", an increase from 6 characters to 11.
Your task is to find the total number of characters to represent the newly encoded strings minus the number of characters of code in each original string literal. For example, for the strings above, the total encoded length (6 + 9 + 16 + 11 = 42) minus the characters in the original code representation (23, just like in the first part of this puzzle) is 42 - 23 = 19.

Your puzzle answer was 2085.

*/

import Foundation

class Day8 : DayBase {
    
    init() {
        super.init(day:8, filePath:"/Users/danielguilliams/Documents/Playground/advent8.txt")
    }
    
    override func DoSolve(){
        let lines = puzzleContent.characters.split { $0 == "\n"}.map(String.init)
        
        var originalTotal:Int = 0
        var escapedTotal:Int = 0
        var encodedTotal:Int = 0
        
        for l in lines {
            originalTotal += l.length
            escapedTotal += EscapedLength(l)
            encodedTotal += EncodedTotal(l)
        }
     
        print("  Original Length: \(originalTotal)")
        print("  Pt1: Escaped Length: \(escapedTotal)")
        print("  Pt1: Difference \(originalTotal - escapedTotal)")
        print("  Pt2: Encoded Length \(encodedTotal)")
        print("  Pt2: Difference \(encodedTotal - originalTotal)")
    }
    
    private func EncodedTotal(str:String) -> Int {
        var i:Int = 0
        var len:Int = 0
        
        while i < str.characters.count {
            len++
            
            if str[i] == "\\" || str[i] == "\"" {
                // escape sequence - add one to the length 
                len++
            }
            
            i++
        }
        
        return len + 2 // add two to account for new "'s on each end
    }
    
    private func EscapedLength(str:String) -> Int {
        var i:Int = 0
        var len:Int = 0
        while i < str.characters.count {
            len++
            if str[i] == "\\" && i + 1 < str.length && str[i+1] == "x" {
                // hex char escape sequence '\xNN' - add three to move past it
                i += 3
            }
            else if str[i] == "\\" {
                // regular char escape sequence - add one to move past it
                i++
            }
            
            i++
        }
        
        // subtract two for quotes at start/finish
        
        return len - 2
    }

}