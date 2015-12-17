//
//  Day15.swift
//  Advent
//
//  Created by Dan on 12/14/15.
//  Copyright © 2015 Dan. All rights reserved.
//

/*
--- Day 15: Science for Hungry People ---

Today, you set out on the task of perfecting your milk-dunking cookie recipe. All you have to do is find the right balance of ingredients.

Your recipe leaves room for exactly 100 teaspoons of ingredients. You make a list of the remaining ingredients you could use to finish the recipe (your puzzle input) and their properties per teaspoon:

capacity (how well it helps the cookie absorb milk)
durability (how well it keeps the cookie intact when full of milk)
flavor (how tasty it makes the cookie)
texture (how it improves the feel of the cookie)
calories (how many calories it adds to the cookie)
You can only measure ingredients in whole-teaspoon amounts accurately, and you have to be accurate so you can reproduce your results in the future. The total score of a cookie can be found by adding up each of the properties (negative totals become 0) and then multiplying together everything except calories.

For instance, suppose you have these two ingredients:

Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3
Then, choosing to use 44 teaspoons of butterscotch and 56 teaspoons of cinnamon (because the amounts of each ingredient must add up to 100) would result in a cookie with the following properties:

A capacity of 44*-1 + 56*2 = 68
A durability of 44*-2 + 56*3 = 80
A flavor of 44*6 + 56*-2 = 152
A texture of 44*3 + 56*-1 = 76

Multiplying these together (68 * 80 * 152 * 76, ignoring calories for now) results in a total score of 62842880, which happens to be the best score possible given these ingredients. If any properties had produced a negative total, it would have instead become zero, causing the whole score to multiply to zero.

Given the ingredients in your kitchen and their properties, what is the total score of the highest-scoring cookie you can make?

Your puzzle answer was 222870.

--- Part Two ---

Your cookie recipe becomes wildly popular! Someone asks if you can make another recipe that has exactly 500 calories per cookie (so they can use it as a meal replacement). Keep the rest of your award-winning process the same (100 teaspoons, same ingredients, same scoring system).

For example, given the ingredients above, if you had instead selected 40 teaspoons of butterscotch and 60 teaspoons of cinnamon (which still adds to 100), the total calorie count would be 40*8 + 60*3 = 500. The total score would go down, though: only 57600000, the best you can do in such trying circumstances.

Given the ingredients in your kitchen and their properties, what is the total score of the highest-scoring cookie you can make with a calorie total of 500?

Your puzzle answer was 117936.

*/

import Foundation

class Ingredient {
    let cap:Int
    let dur:Int
    let flav:Int
    let tex:Int
    let cal:Int
    let name:String
    
    init(s:String) {
        let tokens = s.characters.split { $0 == " " || $0 == ","}.map(String.init)
        // Sugar: capacity 3, durability 0, flavor 0, texture -3, calories 2
        //   0       1     2        3    4      5  6     7     8      9    10
        
        name = tokens[0]
        cap = Int(tokens[2])!
        dur = Int(tokens[4])!
        flav = Int(tokens[6])!
        tex = Int(tokens[8])!
        cal = Int(tokens[10])!
    }
}

class Day15 : DayBase {
    
    init () {
        super.init(day:15, content:input)
    }
    
    override func DoSolve() {
        let lines = input.characters.split { $0 == "\n"}.map(String.init)
        var ingredients = [Ingredient]()
    
        for l in lines {
            let i = Ingredient(s: l)
            ingredients.append(i)
        }
        
        let pt1BestCookie = GetBestCookie(ingredients, qualified: { _ in true })
        print("  Pt 1: Best Cookie Score: \(pt1BestCookie.0),\n  Recipe: \(pt1BestCookie.1)")
        let pt2BestCookie = GetBestCookie(ingredients, qualified: { $0 == 500 })
        print("  Pt 2: Best Cookie Score: \(pt2BestCookie.0),\n  Recipe: \(pt2BestCookie.1)")    }
    
    private func GetBestCookie(i:[Ingredient], qualified:(Int) -> Bool) -> (Int, String){
        var maxCookie = Int.min
        var recipe = ""
        for a in 1...97 {
            for b in 1...97 {
                for c in 1...97 {
                    let d = 100 - a - b - c
                    // calculate calories
                    let cal = i[0].cal * a + i[1].cal * b + i[2].cal * c + i[3].cal * d
                    // check if the cookie is qualified by number of calories (only matter for Pt 2)
                    if qualified(cal) {
                        let cap = i[0].cap * a + i[1].cap * b + i[2].cap * c + i[3].cap * d
                        let dur = i[0].dur * a + i[1].dur * b + i[2].dur * c + i[3].dur * d
                        let flav = i[0].flav * a + i[1].flav * b + i[2].flav * c + i[3].flav * d
                        let tex = i[0].tex * a + i[1].tex * b + i[2].tex * c + i[3].tex * d
                        
                        let score = (cap > 0 ? cap : 0) *
                                    (dur > 0 ? dur : 0) *
                                    (flav > 0 ? flav : 0) *
                                    (tex > 0 ? tex : 0)
                        
                        if score > maxCookie {
                            maxCookie = score
                            recipe = "\(i[0].name)\(a) tsp, \(i[1].name)\(b) tsp, \(i[2].name)\(c) tsp, \(i[3].name)\(d) tsp"
                        }
                    }
                }
            }
        }
        
        return (maxCookie, recipe)
    }

    let input = "Sugar: capacity 3, durability 0, flavor 0, texture -3, calories 2 \n" +
                "Sprinkles: capacity -3, durability 3, flavor 0, texture 0, calories 9 \n" +
                "Candy: capacity -1, durability 0, flavor 4, texture 0, calories 1 \n" +
                "Chocolate: capacity 0, durability 0, flavor -2, texture 2, calories 8 \n"
    
    
}