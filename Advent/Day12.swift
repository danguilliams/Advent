//
//  Day12.swift
//  Advent
//
//  Created by Dan on 12/11/15.
//  Copyright © 2015 Dan. All rights reserved.
//

import Foundation

class Day12 : DayBase {
    
    init() {
        super.init(day:12, content:"advent12.txt")
    }
    
    override func DoSolve() {
        let tokens = puzzleContent.characters.split{ "[]\",:{}".characters.contains($0)}.map(String.init)
        
        var sum = 0
        for t in tokens {
            let val = Int(t)
            if val != nil {
                sum += val!
            }
        }
        
        print("\(sum)")
    }
    
}