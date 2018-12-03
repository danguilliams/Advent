//
//  Day18.swift
//  Advent
//
//  Created by Dan on 12/18/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*--- Day 18: Like a GIF For Your Yard ---

After the million lights incident, the fire code has gotten stricter: now, at most ten thousand lights are allowed. You arrange them in a 100x100 grid.

Never one to let you down, Santa again mails you instructions on the ideal lighting configuration. With so few lights, he says, you'll have to resort to animation.

Start by setting your lights to the included initial configuration (your puzzle input). A # means "on", and a . means "off".

Then, animate your grid in steps, where each step decides the next configuration based on the current one. Each light's next state (either on or off) depends on its current state and the current states of the eight lights adjacent to it (including diagonals). Lights on the edge of the grid might have fewer than eight neighbors; the missing ones always count as "off".

For example, in a simplified 6x6 grid, the light marked A has the neighbors numbered 1 through 8, and the light marked B, which is on an edge, only has the neighbors marked 1 through 5:

1B5...
234...
......
..123.
..8A4.
..765.
The state a light should have next is based on its current state (on or off) plus the number of neighbors that are on:

A light which is on stays on when 2 or 3 neighbors are on, and turns off otherwise.
A light which is off turns on if exactly 3 neighbors are on, and stays off otherwise.
All of the lights update simultaneously; they all consider the same current state before moving to the next.

Here's a few steps from an example configuration of another 6x6 grid:

Initial state:
.#.#.#
...##.
#....#
..#...
#.#..#
####..

After 1 step:
..##..
..##.#
...##.
......
#.....
#.##..

After 2 steps:
..###.
......
..###.
......
.#....
.#....

After 3 steps:
...#..
......
...#..
..##..
......
......

After 4 steps:
......
......
..##..
..##..
......
......
After 4 steps, this example has four lights on.

In your grid of 100x100 lights, given your initial configuration, how many lights are on after 100 steps?

Your puzzle answer was 821.

--- Part Two ---

You flip the instructions over; Santa goes on to point out that this is all just an implementation of Conway's Game of Life. At least, it was, until you notice that something's wrong with the grid of lights you bought: four lights, one in each corner, are stuck on and can't be turned off. The example above will actually run like this:

Initial state:
##.#.#
...##.
#....#
..#...
#.#..#
####.#

After 1 step:
#.##.#
####.#
...##.
......
#...#.
#.####

After 2 steps:
#..#.#
#....#
.#.##.
...##.
.#..##
##.###

After 3 steps:
#...##
####.#
..##.#
......
##....
####.#

After 4 steps:
#.####
#....#
...#..
.##...
#.....
#.#..#

After 5 steps:
##.###
.##..#
.##...
.##...
#.#...
##...#
After 5 steps, this example now has 17 lights on.

In your grid of 100x100 lights, given your initial configuration, but with the four corners always in the on state, how many lights are on after 100 steps?

Your puzzle answer was 886.

*/

import Foundation

class Day18 : DayBase {
    
    var now:[[Bool]] = [[Bool]](count: 100, repeatedValue:[Bool](count: 100, repeatedValue: false))
    var next:[[Bool]] = [[Bool]](count: 100, repeatedValue:[Bool](count: 100, repeatedValue: false))

    let dirs:[(Int,Int)] = [(-1,-1),  (0,-1),  (1,-1),
                            (-1, 0),/*(0, 0),*/(1, 0),
                            (-1, 1),  (0, 1),  (1, 1)]
    init () {
        super.init(day: 18, fileName:"advent18.txt")
        ResetLights()
    }
    
    override func DoSolve() {
        Run(100, stuckCorners:false)
        print("  Pt. 1: Number of lights on: \(CountLightsOn())")
        ResetLights()
        Run(100, stuckCorners:true)
        print("  Pt. 2: Number of lights on: \(CountLightsOn())")
    }
    
    private func Run(iterations:Int, stuckCorners:Bool) {
        for _ in 1...iterations {
            if(stuckCorners) { SetCornersOn() }
            RunStep()
        }
        
        if(stuckCorners) {  SetCornersOn() }
    }
    
    private func RunStep() {
        for x in 0..<100 {
            for y in 0..<100 {
                SetNextState(x,y:y)
            }
        }
    
        let tmp = now
        now = next;
        next = tmp
    }
    
    private func IsOn(x:Int,y:Int) -> Bool {
        return InBounds(x,y:y) && (now[x][y])
    }
    
    private func SetNextState(x:Int, y:Int) {
        var on = 0
        dirs.forEach({ if IsOn(x+$0.0, y: y+$0.1){ on += 1 }})
        if now[x][y] { // light already on
            next[x][y] = on == 2 || on == 3
        } else { // light was off
            next[x][y] = on == 3
        }
    }
    
    private func InBounds(x:Int, y:Int) -> Bool {
        return (x >= 0) && (x < 100) && (y >= 0) && (y < 100)
    }
    
    private func SetCornersOn() {
        now[0][0] = true
        now[0][99] = true
        now[99][0] = true
        now[99][99] = true
    }
    
    private func ResetLights() {
        let lines = puzzleContent.characters.split{$0 == "\n"}.map(String.init)
        
        var i = 0
        for l in lines {
            var j = 0
            for c in l.characters {
                now[i][j] = c == "#" ? true : false
                j += 1
            }
            i += 1
        }
    }
    
    private func CountLightsOn() -> Int {
        var count = 0;
        now.forEach({count += $0.filter({$0 == true}).count})
        return count
    }
}