//
//  Day11.swift
//  Advent
//
//  Created by Dan on 12/10/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*
--- Day 11: Corporate Policy ---

Santa's previous password expired, and he needs help choosing a new one.

To help him remember his new password after the old one expires, Santa has devised a method of coming up with a password based on the previous one. Corporate policy dictates that passwords must be exactly eight lowercase letters (for security reasons), so he finds his new password by incrementing his old password string repeatedly until it is valid.

Incrementing is just like counting with numbers: xx, xy, xz, ya, yb, and so on. Increase the rightmost letter one step; if it was z, it wraps around to a, and repeat with the next letter to the left until one doesn't wrap around.

Unfortunately for Santa, a new Security-Elf recently started, and he has imposed some additional password requirements:

Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.
Passwords must contain at least two pairs of letters, like aa, bb, or zz.
For example:

hijklmmn meets the first requirement (because it contains the straight hij) but fails the second requirement requirement (because it contains i and l).
abbceffg meets the third requirement (because it repeats bb and ff) but fails the first requirement.
abbcegjk fails the third requirement, because it only has one double letter (bb).
The next password after abcdefgh is abcdffaa.
The next password after ghijklmn is ghjaabcc, because you eventually skip all the passwords that start with ghi..., since i is not allowed.
Given Santa's current password (your puzzle input), what should his next password be?

Your puzzle input is hepxcrrq.

Your puzzle answer was hepxxyzz.

--- Part Two ---

Santa's password expired again. What's the next one?

Your puzzle answer was heqaabcc.

*/

import Foundation

class Day11 : DayBase {
    
    let chars:[Character:Int] = ["a":0,"b":1,"c":2,"d":3,"e":4,"f":5,"g":6,"h":7,"i":8,"j":9,"k":10,"l":11,"m":12,"n":13,"o":14,"p":15,"q":16,"r":17,"s":18,"t":19,"u":20,"v":21,"w":22,"x":23,"y":24,"z":25]
    
    var arr:[Int] = [Int](count: 8, repeatedValue:0)
    
    init() {
        super.init(day:11, content:"hepxcrrq")
    }
    
    override func DoSolve() {
        let pwd = NextPassword("hepxcrrq")
        print("  First new password: \(pwd)")
        let pwd2 = NextPassword(pwd)
        print("  Second new password: \(pwd2)")
    }
    
    private func NextPassword(s:String) -> String {
        Encode(s)
        Increment()
        while !(HasStraight() && HasNoBadLetters() && HasTwoDuplicates()) {
            Increment()
        }
        
        return Decode()
    }
    
    private func Encode(s:String) {
        var i = 0
        for c in s.characters {
            arr[i] = chars[c]!
            i += 1
        }
    }
    
    private func Decode() -> String {
        var s = ""
        for var i=0;i < arr.count; i += 1 {
            s.append(GetLetter(arr[i]))
        }
        return s
    }
    
    private func GetLetter(i:Int) -> Character {
        for (c,n) in chars {
            if n == i {
                return c
            }
        }
        
        return "!"
    }
    
    private func Increment() {
        var i = arr.count - 1
        var done = false
        
        while !done && i >= 0 {
            arr[i] += 1
            if arr[i] < 26 {
                done = true
            } else {
                arr[i] = 0
            }
            
            i -= 1
        }
        
    }
    
    private func HasStraight() -> Bool {
        for var i = 0 ; i < arr.count - 2 ; i++ {
            if  arr[i] < 24 &&
                arr[i] + 1 == arr[i + 1] &&
                arr[i] + 2 == arr[i + 2] {
                    return true
            }
        }
        return false
    }
    
    private func HasNoBadLetters() -> Bool {
        for n in arr {
            if n == 8 || n == 11 || n == 14 {
                return false
            }
        }
        
        return true
    }
    
    private func HasTwoDuplicates() -> Bool {
        var idx = (-1,-1)
        for var i = 0; i < arr.count - 1; i++ {
            if arr[i] == arr[i+1] {
                idx.0 >= 0 ? (idx.1 = i) : (idx.0 = i)
            }
        }
        
        return idx.0 >= 0 && idx.1 >= 2 && idx.1 - idx.0 > 1
    }
}



