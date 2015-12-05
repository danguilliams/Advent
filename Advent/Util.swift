//
//  Util.swift
//  Advent
//
//  Created by Dan on 12/5/15.
//  Copyright © 2015 Dan. All rights reserved.
//

import Foundation

func md5(string string: String) -> [UInt8] {
    var digest = [UInt8](count: Int(CC_MD5_DIGEST_LENGTH), repeatedValue: 0)
    if let data = string.dataUsingEncoding(NSUTF8StringEncoding) {
        CC_MD5(data.bytes, CC_LONG(data.length), &digest)
    }
    
    return digest
}

struct Pair : Hashable {
    var a:Character
    var b:Character
    
    var hashValue: Int {
        return a.hashValue ^ b.hashValue
    }
}

func == (lhs: Pair, rhs: Pair) -> Bool {
    return lhs.a == rhs.a && lhs.b == rhs.b
}

struct House : Hashable
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

func == (lhs: House, rhs: House) -> Bool {
    return lhs.x == rhs.x && lhs.y == rhs.y
}