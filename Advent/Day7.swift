//
//  Day7.swift
//  Advent
//
//  Created by Dan on 12/6/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*

*/

import Foundation

class Day7 : DayBase {
    
    init () {
        super.init(day: 7, content:"")
    }
    
    override func DoSolve() {
        let lines = puzzleContent.characters.split{ $0 == "\n" || $0 == "\r\n" }.map(String.init)
        
        for l in lines {
            print(l)
        }
    }
}