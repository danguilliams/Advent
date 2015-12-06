//
//  DayBase.swift
//  Advent
//
//  Created by Dan on 12/6/15.
//  Copyright © 2015 Dan. All rights reserved.
//

import Foundation

public class DayBase {
    
    public var puzzleContent:String = ""
    public var day = 0
    
    init(day:Int, filePath:String) {
        self.day = day
        
        let filemgr = NSFileManager.defaultManager()
        assert(filemgr.fileExistsAtPath(filePath))
        puzzleContent = try! String(contentsOfFile: filePath, encoding: NSUTF8StringEncoding)
    }
    
    init(day:Int, content:String) {
        self.day = day
        puzzleContent = content
    }
    
    public func Solve() {
        print("Day \(self.day) Results")
        print("Running...")
        let startTime = NSDate()
        DoSolve()
        let elapsed = NSDate().timeIntervalSinceDate(startTime)
        print("Day \(self.day) Finished. Elapsed time: \(elapsed) seconds")
        print("")
    }
    
    func DoSolve() {
        print("Base class")
    }
}