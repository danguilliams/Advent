//
//  Day17.swift
//  Advent
//
//  Created by Dan on 12/16/15.
//  Copyright © 2015 Dan. All rights reserved.
//

import Foundation

class Day17 : DayBase {

    var containers:[Int] = [Int]()
    let size = 150
    
    init () {
        super.init(day: 17, fileName:"advent17.txt")
        self.containers = puzzleContent.characters.split {$0 == "\n"}.map({Int(String($0))!})
    }
    
    override func DoSolve() {
        var matches = 0
        for i in 2...containers.count {
            let newMatches = CountCombinations(i, arr: containers)
            matches += newMatches
            print("Container Count: \(i) New Matches: \(newMatches) Total Matches: \(matches)")
        }
        
        print("Total Matches: \(matches)")
    }


    private func CountCombinations(size:Int, arr:[Int]) -> Int {
        var matches = 0
        let resultArr = [Int](count:size, repeatedValue:0)
        CombinationsRecurse(arr, len: size, start: 0, result: resultArr, matchCount: &matches)
        return matches
    }
    
    private func CombinationsRecurse(arr:[Int], len:Int, start:Int, var result:[Int], inout matchCount:Int) {
        if len == 0 {
            if result.reduce(0, combine: +) == 150 {
                matchCount += 1
            }
        }
        else {
            for i in start...arr.count-len {
                result[result.count - len] = arr[i]
                CombinationsRecurse(arr, len: len - 1, start: i + 1, result: result, matchCount: &matchCount)
            }
        }
    }
}