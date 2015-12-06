//
//  Day6.swift
//  Advent
//
//  Created by Dan on 12/6/15.
//  Copyright © 2015 Dan. All rights reserved.
//

import Foundation

/*
--- Day 6: Probably a Fire Hazard ---

Because your neighbors keep defeating you in the holiday house decorating contest year after year, you've decided to deploy one million lights in a 1000x1000 grid.

Furthermore, because you've been especially nice this year, Santa has mailed you instructions on how to display the ideal lighting configuration.

Lights in your grid are numbered from 0 to 999 in each direction; the lights at each corner are at 0,0, 0,999, 999,999, and 999,0. The instructions include whether to turn on, turn off, or toggle various inclusive ranges given as coordinate pairs. Each coordinate pair represents opposite corners of a rectangle, inclusive; a coordinate pair like 0,0 through 2,2 therefore refers to 9 lights in a 3x3 square. The lights all start turned off.

To defeat your neighbors this year, all you have to do is set up your lights by doing the instructions Santa sent you in order.

For example:

'turn on 0,0 through 999,999' would turn on (or leave on) every light.
'toggle 0,0 through 999,0' would toggle the first line of 1000 lights, turning off the ones that were on, and turning on the ones that were off.
'turn off 499,499 through 500,500' would turn off (or leave off) the middle four lights.

After following the instructions, how many lights are lit?

--- Part Two ---

You just finish implementing your winning light pattern when you realize you mistranslated Santa's message from Ancient Nordic Elvish.

The light grid you bought actually has individual brightness controls; each light can have a brightness of zero or more. The lights all start at zero.

The phrase turn on actually means that you should increase the brightness of those lights by 1.

The phrase turn off actually means that you should decrease the brightness of those lights by 1, to a minimum of zero.

The phrase toggle actually means that you should increase the brightness of those lights by 2.

What is the total brightness of all lights combined after following Santa's instructions?

For example:

turn on 0,0 through 0,0 would increase the total brightness by 1.
toggle 0,0 through 999,999 would increase the total brightness by 2000000.

Your puzzle answer was 14687245.


*/

enum Act {
    case On
    case Off
    case Toggle
}

class Command {
    
    var xRange:Range<Int>
    var yRange:Range<Int>
    var action:Act
    
    init(cmd:String){
        let tokens = cmd.characters.split {$0 == " "}.map(String.init)
        
        var i = 1
        if tokens[0] == "toggle" {
            action = Act.Toggle
        }
        else if tokens[1] == "off" {
            action = Act.Off
            i += 1
        } else {
            action = Act.On
            i += 1
        }
        
        let startDirs = tokens[i].characters.split { $0 == "," }.map { Int(String($0))! }
        let endDirs = tokens [i + 2].characters.split { $0 == ","}.map { Int(String($0))! }
        
        xRange = startDirs[0]...endDirs[0]
        yRange = startDirs[1]...endDirs[1]
    }
    
}

/* 

Grid for first part of problem.
Bools required to keep state for 'toggle' commands 

*/
class Grid {
    let size = 1000
    var g = [[Bool]](count: 1000, repeatedValue:[Bool](count: 1000, repeatedValue: false))
    var on:Int = 0;
    
    func Apply(let c:Command) {
        for i in c.xRange {
            for j in c.yRange {
                switch c.action{
                case Act.On:
                    if !g[i][j] {
                        g[i][j] = true
                        on++
                    }
                case Act.Off:
                    if g[i][j] {
                        g[i][j] = false
                        on--
                    }
                case Act.Toggle:
                    g[i][j] = !g[i][j]
                    g[i][j] ? on++ : on--
                }
            }
        }
    }
    
    func Count() -> Int {
        return on
    }
}

class Grid2 {
    let size = 1000
    var g = [[Int]](count: 1000, repeatedValue:[Int](count: 1000, repeatedValue: 0))
    var brightness:Int = 0;
    
    func Apply(let c:Command) {
        for i in c.xRange {
            for j in c.yRange {
                switch c.action{
                case Act.On:
                    g[i][j] += 1
                    brightness += 1
                case Act.Off:
                    if g[i][j] > 0 {
                        g[i][j] -= 1
                        brightness -= 1
                    }
                case Act.Toggle:
                    g[i][j] += 2
                    brightness += 2
                }
            }
        }
    }
    
    func Count() -> Int {
        return brightness
    }
}

class Day6 {
    
    func Solve() {
        print("Day 6 results:")
        let filemgr = NSFileManager.defaultManager()
        let inputFilePath = "/Users/danielguilliams/Documents/Playground/advent6.txt"
        
        assert(filemgr.fileExistsAtPath(inputFilePath))
        
        let content = try! String(contentsOfFile: inputFilePath, encoding: NSUTF8StringEncoding)
        let lines = content.characters.split { $0 == "\n" || $0 == "\r\n" }.map(String.init)
        
        let grid = Grid()
        let grid2 = Grid2()
        for l in lines {
            let command = Command(cmd:l)
            grid.Apply(command)
            grid2.Apply(command)
        }
        
        print("  Pt1: Number of lights on: \(grid.Count())")
        print("  Pt2: Total Brightness of lights:\(grid2.Count())")
    }

}





