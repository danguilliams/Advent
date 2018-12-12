//
//  Day14.swift
//  Advent
//
//  Created by Dan on 12/14/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*

--- Day 14: Reindeer Olympics ---

This year is the Reindeer Olympics! Reindeer can fly at high speeds, but must rest occasionally to recover their energy. Santa would like to know which of his reindeer is fastest, and so he has them race.

Reindeer can only either be flying (always at their top speed) or resting (not moving at all), and always spend whole seconds in either state.

For example, suppose you have the following Reindeer:

Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.
After one second, Comet has gone 14 km, while Dancer has gone 16 km. After ten seconds, Comet has gone 140 km, while Dancer has gone 160 km. On the eleventh second, Comet begins resting (staying at 140 km), and Dancer continues on for a total distance of 176 km. On the 12th second, both reindeer are resting. They continue to rest until the 138th second, when Comet flies for another ten seconds. On the 174th second, Dancer flies for another 11 seconds.

In this example, after the 1000th second, both reindeer are resting, and Comet is in the lead at 1120 km (poor Dancer has only gotten 1056 km by that point). So, in this situation, Comet would win (if the race ended at 1000 seconds).

Given the descriptions of each reindeer (in your puzzle input), after exactly 2503 seconds, what distance has the winning reindeer traveled?

Your puzzle answer was 2696.

--- Part Two ---

Seeing how reindeer move in bursts, Santa decides he's not pleased with the old scoring system.

Instead, at the end of each second, he awards one point to the reindeer currently in the lead. (If there are multiple reindeer tied for the lead, they each get one point.) He keeps the traditional 2503 second time limit, of course, as doing otherwise would be entirely ridiculous.

Given the example reindeer from above, after the first second, Dancer is in the lead and gets one point. He stays in the lead until several seconds into Comet's second burst: after the 140th second, Comet pulls into the lead and gets his first point. Of course, since Dancer had been in the lead for the 139 seconds before that, he has accumulated 139 points by the 140th second.

After the 1000th second, Dancer has accumulated 689 points, while poor Comet, our old champion, only has 312. So, with the new scoring system, Dancer would win (if the race ended at 1000 seconds).

Again given the descriptions of each reindeer (in your puzzle input), after exactly 2503 seconds, how many points does the winning reindeer have?

Your puzzle answer was 1084.

*/

import Foundation

class Reindeer {
    var name:String
    var speed:Int
    var endurance:Int
    var rest:Int
    var points:Int = 0
    
    init(n:String, s:Int, e:Int, r:Int) {
        self.name = n
        self.speed = s
        self.endurance = e
        self.rest = r
        
    }
    
    func TravelsIn(seconds:Int) -> Int {
        let subSeconds = seconds % (endurance + rest) // length of the partial cycle
        return (seconds / (endurance + rest)) * (speed * endurance) + (subSeconds < endurance ? speed * subSeconds : speed * endurance )
    }
}

class Day14 : DayBase {
    let raceLength = 2503
    var racers = [Reindeer]()
    
    init() {
        super.init(day:14, fileName:"advent14.txt")
        
        let lines = puzzleContent.characters.split { $0 == "\n"}.map(String.init)
        
        for l in lines {
            let tokens = l.characters.split { $0 == " " || $0 == "."}.map(String.init)
            // Rudolph can fly 22 km/s for 8 seconds, but then must rest for 165 seconds.
            //    0     1   2   3  4    5  6     7     8    9   10   11   12  13    14
            racers.append(Reindeer(n:tokens[0], s:Int(tokens[3])!, e:Int(tokens[6])!, r:Int(tokens[13])!))
        }
    }
    
    override func DoSolve() {
        Solve1()
        Solve2()
    }
    
    private func Solve1() {
        racers.sortInPlace({$0.TravelsIn(raceLength) < $1.TravelsIn(raceLength)})
        let winner = racers.last!
        print("  Pt 1: Winner: \(winner.name) Distance: \(winner.TravelsIn(raceLength))")
    }
    
    private func Solve2() {
        racers.forEach({$0.points = 0})
        for i in 1...raceLength {
            racers.sortInPlace({$0.TravelsIn(i) < $1.TravelsIn(i)})
            racers.last!.points += 1
        }
        
        racers.sortInPlace({$0.points < $1.points})
        let winner = racers.last!
        print("  Pt 2: Winner \(winner.name) Points: \(winner.points)")
    }
    
}