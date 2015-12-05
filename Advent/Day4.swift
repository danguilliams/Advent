//
//  Day4.swift
//  Advent
//
//  Created by Dan on 12/4/15.
//  Copyright © 2015 Dan. All rights reserved.
//

import Foundation

/*

--- Day 4: The Ideal Stocking Stuffer ---

Santa needs help mining some AdventCoins (very similar to bitcoins) to use as gifts for all the economically forward-thinking little girls and boys.

To do this, he needs to find MD5 hashes which, in hexadecimal, start with at least five zeroes. The input to the MD5 hash is some secret key (your puzzle input, given below) followed by a number in decimal. To mine AdventCoins, you must find Santa the lowest positive number (no leading zeroes: 1, 2, 3, ...) that produces such a hash.

For example:

If your secret key is abcdef, the answer is 609043, because the MD5 hash of abcdef609043 starts with five zeroes (000001dbbfa...), and it is the lowest such number to do so.
If your secret key is pqrstuv, the lowest number it combines with to make an MD5 hash starting with five zeroes is 1048970; that is, the MD5 hash of pqrstuv1048970 looks like 000006136ef....

Your puzzle input was ckczppom.

Now find one that starts with five zeroes.

Your puzzle answer was 117946.

--- Part Two ---

Now find one that starts with six zeroes.

Your puzzle answer was 3938038.

*/

class Day4 {
    
    func Solve() {
        print("Day 4 results:")
        Solve(5)
        Solve(6)
        print("")
        
    }
    
    private func Solve(let num0s:Int) {
        let code = "ckczppom"
        var val:Int = 0
        
        while true {
            
            let bytes = md5(string: "\(code)\(val)")
            
            let wholeBytes = num0s / 2
            
            var hasWholeDigits = true;
            for var i = 0; i < wholeBytes; i++ {
                if bytes[i] != 0 {
                    hasWholeDigits = false
                    break
                }
            }
            
            let hasHalfZero = num0s % 2 == 1 ? bytes[wholeBytes] <= 0x0F : true
            
            if hasWholeDigits && hasHalfZero {
                break;
            }
            
            val += 1
        }
        
        print("  Value for \(num0s) 0s: \(val)")
    }
}









