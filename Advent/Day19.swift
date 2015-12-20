//
//  Day19.swift
//  Advent
//
//  Created by Dan on 12/19/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*
--- Day 19: Medicine for Rudolph ---

Rudolph the Red-Nosed Reindeer is sick! His nose isn't shining very brightly, and he needs medicine.

Red-Nosed Reindeer biology isn't similar to regular reindeer biology; Rudolph is going to need custom-made medicine. Unfortunately, Red-Nosed Reindeer chemistry isn't similar to regular reindeer chemistry, either.

The North Pole is equipped with a Red-Nosed Reindeer nuclear fusion/fission plant, capable of constructing any Red-Nosed Reindeer molecule you need. It works by starting with some input molecule and then doing a series of replacements, one per step, until it has the right molecule.

However, the machine has to be calibrated before it can be used. Calibration involves determining the number of molecules that can be generated in one step from a given starting point.

For example, imagine a simpler machine that supports only the following replacements:

H => HO
H => OH
O => HH
Given the replacements above and starting with HOH, the following molecules could be generated:

HOOH (via H => HO on the first H).
HOHO (via H => HO on the second H).
OHOH (via H => OH on the first H).
HOOH (via H => OH on the second H).
HHHH (via O => HH).
So, in the example above, there are 4 distinct molecules (not five, because HOOH appears twice) after one replacement from HOH. Santa's favorite molecule, HOHOHO, can become 7 distinct molecules (over nine replacements: six from H, and three from O).

The machine replaces without regard for the surrounding characters. For example, given the string H2O, the transition H => OO would result in OO2O.

Your puzzle input describes all of the possible replacements and, at the bottom, the medicine molecule for which you need to calibrate the machine. How many distinct molecules can be created after all the different ways you can do one replacement on the medicine molecule?

Your puzzle answer was 518.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

Now that the machine is calibrated, you're ready to begin molecule fabrication.

Molecule fabrication always begins with just a single electron, e, and applying replacements one at a time, just like the ones during calibration.

For example, suppose you have the following replacements:

e => H
e => O
H => HO
H => OH
O => HH
If you'd like to make HOH, you start with e, and then make the following replacements:

e => O to get O
O => HH to get HH
H => OH (on the second H) to get HOH
So, you could make HOH after 3 steps. Santa's favorite molecule, HOHOHO, can be made in 6 steps.

How long will it take to make the medicine? Given the available replacements and the medicine molecule in your puzzle input, what is the fewest number of steps to go from e to the medicine molecule?

Although it hasn't changed, you can still get your puzzle input.

Your puzzle answer was 200.

Both parts of this puzzle are complete! They provide two gold stars: **

*/

import Foundation

class Mod {
    var from:String
    var to:String
    
    init(f:String, t:String) {
        self.from = f
        self.to = t
    }
}

class Day19:DayBase {
    
    var unique:Set<String> = Set<String>()
    var mods:[Mod] = [Mod]()
    var molecule:String = ""
    var molList:LinkedList<String> = LinkedList<String>()
    
    init() {
        super.init(day:19, fileName:"advent19.txt")
        let lines = puzzleContent.characters.split{$0 == "\n"}.map(String.init).filter({$0 != ""})
        for l in lines {
            if l.contains("=>") { // 'replacement'
                let tokens = l.characters.split{$0 == " "}.map(String.init)
                mods.append(Mod(f: tokens[0], t:tokens[2]))
            }
            else {
                molecule = l
            }
        }
        
        ParseMolecule()
    }
    
    override func DoSolve() {
        let pt2Steps = CalcSteps()
        var current:ListNode<String>? = molList.head
        while current != nil {
            let possibleMods = mods.filter{$0.from==current!.key!}
            let original = current!.key
            for m in possibleMods {
                current!.key = m.to
                let newMolecule = GetMolecule()
                if !unique.contains(newMolecule) {
                    unique.insert(newMolecule)
                }
            }
            current!.key = original
            current = current!.next
        }
        
        print("  Pt 1: Unique Molecules: \(unique.count)")
        print("  Pt 2: Number of steps: \(pt2Steps)")
    }
    
    func ParseMolecule() {
        molList = LinkedList<String>()
        assert(molecule.length > 2)
        for i in 1..<molecule.length {
            let one = molecule[i-1]
            let two = molecule[i]
            
            // only going to add something if the first char is upper case
            if UpperCase.characters.contains(one) {
                if LowerCase.characters.contains(two) {
                    // 'T' and 'i', etc, add both chars as a single node
                    molList.Add("\(one)\(two)")
                } else {
                    // 'H' and 'H', etc - just add first char as single node
                    molList.Add("\(one)")
                }
            }
        }
    }
    
    func GetMolecule() -> String {
        var current:ListNode<String>? = molList.head
        var result = ""
        while current != nil {
            result += current!.key!
            current = current!.next
        }
        
        return result
    }
    
    // This calculation is based on the solution provided at
    // https://www.reddit.com/r/adventofcode/comments/3xflz8/day_19_solutions/cy4etju
    func CalcSteps() -> Int {
        // count the 'Rn' and 'Ar' elements
        var pElemCount = 0;
        var yElemCount = 0;
        
        var current:ListNode<String>? = molList.head
        while current != nil {
            let elem = current!.key
            if elem == "Ar" ||
               elem == "Rn" {
                   pElemCount += 1
            } else if elem == "Y" {
                yElemCount += 1
            }
            current = current!.next
        }
        
        return molList.size - pElemCount - 2*yElemCount - 1
    }
}