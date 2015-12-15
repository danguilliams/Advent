//
//  Util.swift
//  Advent
//
//  Created by Dan on 12/5/15.
//  Copyright © 2015 Dan. All rights reserved.
//

import Foundation

/*
    Calculates a string's MD5 hash, treating them as UTF8 characters 

    If the conversion to UTF8 i
*/
func md5(string string: String) -> [UInt8] {
    var digest = [UInt8](count: Int(CC_MD5_DIGEST_LENGTH), repeatedValue: 0)
    if let data = string.dataUsingEncoding(NSUTF8StringEncoding) {
        CC_MD5(data.bytes, CC_LONG(data.length), &digest)
    }
    
    return digest
}

/*
    Pair of two characters, a and b
*/
struct Pair : Hashable {
    var a:Character
    var b:Character
    
    var hashValue: Int {
        return a.hashValue ^ b.hashValue
    }
}

func ==(lhs: Pair, rhs: Pair) -> Bool {
    return lhs.a == rhs.a && lhs.b == rhs.b
}

/*
    Pair of two ints, x and y
*/
struct Coord : Hashable
{
    var x:Int
    var y:Int
    
    var hashValue: Int {
        return x.hashValue ^ y.hashValue
    }
    
    init(x:Int, y:Int)
    {
        self.x = x
        self.y = y
    }
    
}

func == (lhs: Coord, rhs: Coord) -> Bool {
    return lhs.x == rhs.x && lhs.y == rhs.y
}

private func Swap<T>(inout s1:T, inout s2:T) {
    let tmp:T = s1
    s1 = s2
    s2 = tmp
}

class Edge2 {
    let from:String
    let to:String
    let weight:Int
    
    init(from:String, to:String, weight:Int) {
        self.from = from
        self.to = to
        self.weight = weight
    }
}

/* 
Generate permutations of the array. From http://stackoverflow.com/questions/11208446/generating-permutations-of-a-set-most-efficiently

To use, sort the array in increasing order, then call NextPermutation until true is returned. 

*/
func NextPermutation(inout s:[Int]) -> Bool {
    
    var done = true
    
    for (var i = s.count - 1; i > 0; i--) {
        var curr = s[i];
        
        if curr < s[i - 1] {
            continue;
        }
        
        done = false;

        var currIndex = i;

        for var j = i + 1; j < s.count; j++ {
            if s[j] < curr && s[j] > s[i - 1] {
                curr = s[j];
                currIndex = j;
            }
        }
        
        s[currIndex] = s[i - 1];
        s[i - 1] = curr;

        for var j = s.count - 1; j > i; j--, i++ {
            let tmp = s[j];
            s[j] = s[i];
            s[i] = tmp;
        }
        
        break;
    }

    return done;
}

func NextPermutation(inout s:[String]) -> Bool {
    
    var done = true
    
    for (var i = s.count - 1; i > 0; i--) {
        var curr = s[i];
        
        if curr < s[i - 1] {
            continue;
        }
        
        done = false;
        
        var currIndex = i;
        
        for var j = i + 1; j < s.count; j++ {
            if s[j] < curr && s[j] > s[i - 1] {
                curr = s[j];
                currIndex = j;
            }
        }
        
        s[currIndex] = s[i - 1];
        s[i - 1] = curr;
        
        for var j = s.count - 1; j > i; j--, i++ {
            let tmp = s[j];
            s[j] = s[i];
            s[i] = tmp;
        }
        
        break;
    }
    
    return done;
}

