//
//  Day16.swift
//  Advent
//
//  Created by Dan on 12/16/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*
--- Day 16: Aunt Sue ---

Your Aunt Sue has given you a wonderful gift, and you'd like to send her a thank you card. However, there's a small problem: she signed it "From, Aunt Sue".

You have 500 Aunts named "Sue".

So, to avoid sending the card to the wrong person, you need to figure out which Aunt Sue (which you conveniently number 1 to 500, for sanity) gave you the gift. You open the present and, as luck would have it, good ol' Aunt Sue got you a My First Crime Scene Analysis Machine! Just what you wanted. Or needed, as the case may be.

The My First Crime Scene Analysis Machine (MFCSAM for short) can detect a few specific compounds in a given sample, as well as how many distinct kinds of those compounds there are. According to the instructions, these are what the MFCSAM can detect:

children, by human DNA age analysis.
cats. It doesn't differentiate individual breeds.
Several seemingly random breeds of dog: samoyeds, pomeranians, akitas, and vizslas.
goldfish. No other kinds of fish.
trees, all in one group.
cars, presumably by exhaust or gasoline or something.
perfumes, which is handy, since many of your Aunts Sue wear a few kinds.
In fact, many of your Aunts Sue have many of these. You put the wrapping from the gift into the MFCSAM. It beeps inquisitively at you a few times and then prints out a message on ticker tape:

children: 3
cats: 7
samoyeds: 2
pomeranians: 3
akitas: 0
vizslas: 0
goldfish: 5
trees: 3
cars: 2
perfumes: 1

You make a list of the things you can remember about each Aunt Sue. Things missing from your list aren't zero - you simply don't remember the value.

What is the number of the Sue that got you the gift?

Your puzzle answer was 213.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

As you're about to send the thank you note, something in the MFCSAM's instructions catches your eye. Apparently, it has an outdated retroencabulator, and so the output from the machine isn't exact values - some of them indicate ranges.

In particular, the cats and trees readings indicates that there are greater than that many (due to the unpredictable nuclear decay of cat dander and tree pollen), while the pomeranians and goldfish readings indicate that there are fewer than that many (due to the modial interaction of magnetoreluctance).

What is the number of the real Aunt Sue?

*/

import Foundation

class Sue {
    // things this Sue has
    var objs = [String:Int]()
    var id = ""
    init(s:String) {
        let tokens = s.characters.split { $0 == ":" || $0 == ","}.map(String.init)
        // Sue 2: akitas: 10, perfumes: 10, children: 5
        self.id = tokens[0]
        // all 'Aunts' have 3 objects to parse
        assert(tokens.count == 7)
        for var i = 1; i < 6; i += 2 {
            objs[tokens[i].trim()] = Int(tokens[i+1].trim())!
        }
    }
}

class Day16 : DayBase {
    var aunts = [Sue]()
    let targets:[String:Int] =
       ["children": 3,
        "cats": 7,
        "samoyeds": 2,
        "pomeranians": 3,
        "akitas": 0,
        "vizslas": 0,
        "goldfish": 5,
        "trees": 3,
        "cars": 2,
        "perfumes": 1 ]

    
    init() {
        super.init(day:16, fileName:"advent16.txt")
        let lines = puzzleContent.characters.split{$0 == "\n" }.map(String.init)
        for l in lines {
            aunts.append(Sue(s:l))
        }
    }
    
    override func DoSolve() {
        let sue1 = FindMatchingSue({self.targets[$0]! == $1})
        print("  Pt 1 match: \(sue1.id)")
        let sue2 = FindMatchingSue(Part2Match)
        print("  Pt 2 match: \(sue2.id)")
    }
    
    private func EqualMatch(s: (String,Int)) -> Bool {
        return targets[s.0]! == s.1
    }
    
    private func Part2Match(s: (String,Int)) -> Bool {
        if targets[s.0] != nil {
            if s.0 == "cats" || s.0 == "trees" { // aunt must have more than the target
                return targets[s.0]! < s.1
            } else if s.0 == "pomeranians" || s.0 == "goldfish" { // aunt must have less than the target
                return targets[s.0]! > s.1
            } else {
                return targets[s.0]! == s.1
            }
        }
        
        return false
    }
    
    private func FindMatchingSue(doesMatch: ((String,Int)) -> Bool ) -> Sue {
        var matches = [Sue]()
        for sue in aunts {
            // if this sue has all items matching in the target dictionary
            if sue.objs.filter(doesMatch).count == sue.objs.count {
                matches.append(sue)
            }
        }
        return matches.first!
    }
    

}

