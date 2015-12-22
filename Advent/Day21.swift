//
//  Day21.swift
//  Advent
//
//  Created by Dan on 12/21/15.
//  Copyright © 2015 Dan. All rights reserved.
//
/*
--- Day 21: RPG Simulator 20XX ---

Little Henry Case got a new video game for Christmas. It's an RPG, and he's stuck on a boss. He needs to know what equipment to buy at the shop. He hands you the controller.

In this game, the player (you) and the enemy (the boss) take turns attacking. The player always goes first. Each attack reduces the opponent's hit points by at least 1. The first character at or below 0 hit points loses.

Damage dealt by an attacker each turn is equal to the attacker's damage score minus the defender's armor score. An attacker always does at least 1 damage. So, if the attacker has a damage score of 8, and the defender has an armor score of 3, the defender loses 5 hit points. If the defender had an armor score of 300, the defender would still lose 1 hit point.

Your damage score and armor score both start at zero. They can be increased by buying items in exchange for gold. You start with no items and have as much gold as you need. Your total damage or armor is equal to the sum of those stats from all of your items. You have 100 hit points.

Here is what the item shop is selling:

Weapons:    Cost  Damage  Armor
Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0

Armor:      Cost  Damage  Armor
Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5

Rings:      Cost  Damage  Armor
Damage +1    25     1       0
Damage +2    50     2       0
Damage +3   100     3       0
Defense +1   20     0       1
Defense +2   40     0       2
Defense +3   80     0       3
You must buy exactly one weapon; no dual-wielding. Armor is optional, but you can't use more than one. You can buy 0-2 rings (at most one for each hand). You must use any items you buy. The shop only has one of each item, so you can't buy, for example, two rings of Damage +3.

For example, suppose you have 8 hit points, 5 damage, and 5 armor, and that the boss has 12 hit points, 7 damage, and 2 armor:

The player deals 5-2 = 3 damage; the boss goes down to 9 hit points.
The boss deals 7-5 = 2 damage; the player goes down to 6 hit points.
The player deals 5-2 = 3 damage; the boss goes down to 6 hit points.
The boss deals 7-5 = 2 damage; the player goes down to 4 hit points.
The player deals 5-2 = 3 damage; the boss goes down to 3 hit points.
The boss deals 7-5 = 2 damage; the player goes down to 2 hit points.
The player deals 5-2 = 3 damage; the boss goes down to 0 hit points.
In this scenario, the player wins! (Barely.)

You have 100 hit points. The boss's actual stats are in your puzzle input. What is the least amount of gold you can spend and still win the fight?
*/

import Foundation

class Fighter {
    var damage:Int = 0
    var armor:Int = 0
    var health:Int = 0
    
    init(d:Int, a:Int, h:Int) {
        self.damage = d
        self.armor = a
        self.health = h
    }
}

class Item {
    var cost:Int = 0
    var damage:Int = 0
    var armor:Int = 0
    
    init(c:Int, d:Int, a:Int) {
        self.damage = d
        self.armor = a
        self.cost = c
    }
}

func == (lhs: Item, rhs: Item) -> Bool {
    return lhs.cost == rhs.cost && lhs.armor == rhs.armor && lhs.damage == rhs.damage
}

func != (lhs: Item, rhs: Item) -> Bool {
    return !(lhs==rhs)
}


class Day21 : DayBase {
    /* 
    Boss stats:
        Hit Points: 104
        Damage: 8
        Armor: 1
    */
    var boss:Fighter
    var player:Fighter
    /*
    Weapons:    Cost  Damage  Armor
    Dagger        8     4       0
    Shortsword   10     5       0
    Warhammer    25     6       0
    Longsword    40     7       0
    Greataxe     74     8       0
    
    Armor:      Cost  Damage  Armor
    Leather      13     0       1
    Chainmail    31     0       2
    Splintmail   53     0       3
    Bandedmail   75     0       4
    Platemail   102     0       5
    
    Rings:      Cost  Damage  Armor
    Damage +1    25     1       0
    Damage +2    50     2       0
    Damage +3   100     3       0
    Defense +1   20     0       1
    Defense +2   40     0       2
    Defense +3   80     0       3
    */
    var weapons:[Item] =
                [Item(c:8 ,d:4,a:0),
                 Item(c:10,d:5,a:0),
                 Item(c:25,d:6,a:0),
                 Item(c:40,d:7,a:0),
                 Item(c:74,d:8,a:0)]
    
    var armors:[Item] =
               [Item(c:13 ,d:0,a:1),
                Item(c:31 ,d:0,a:2),
                Item(c:53 ,d:0,a:3),
                Item(c:75 ,d:0,a:4),
                Item(c:102,d:0,a:5)]
    
    var rings:[Item] =
              [Item(c:25 ,d:1,a:0),
               Item(c:50 ,d:2,a:0),
               Item(c:100,d:3,a:0),
               Item(c:20, d:0,a:1),
               Item(c:40, d:0,a:2),
               Item(c:80, d:0,a:3)]
    
    var minCost = Int.max // min cost to win
    var maxCost = Int.min // max cost to still lose
    
    init() {
        self.boss = Fighter(d:8,a:1,h:104)
        self.player = Fighter(d:0,a:0,h:0)
        super.init(day:21, content:"")
    }
    
    override func DoSolve() {

        // list of items we are equipped with. max of 4 items (weapon, armor, and two rings)
        // weapon always at index 0, armor at 1, and rings at 2 & 3 -> armor and rings may be nil
        var loadout = [Item?](count:4, repeatedValue:nil)
        
        for weapon in weapons {
            // checking only weapon
            loadout[0] = weapon
            CheckLoadout(loadout)
            
            // checking weapon with rings
            for ring in rings {
                loadout[2] = ring
                
                for ring2 in rings {
                    // ensure we aren't using this ring already
                    if loadout[2]! != ring2 {
                        loadout[3] = ring2
                    } else {
                        loadout[3] = nil
                    }
                    CheckLoadout(loadout)
                }
            }
            
            // reset rings before adding armor
            loadout[2] = nil
            loadout[3] = nil
            
            // adding armor
            for armor in armors {
                loadout[1] = armor
                CheckLoadout(loadout)
                
                for ring in rings {
                    loadout[2] = ring
                    
                    for ring2 in rings {
                        // ensure we haven't used this ring already
                        if loadout[2]! != ring2 {
                            loadout[3] = ring2
                        } else {
                            loadout[3] = nil
                        }
                        CheckLoadout(loadout)
                    }
                }
            }
            // reset everything but the weapon
            loadout[1] = nil
            loadout[2] = nil
            loadout[3] = nil
        }
        
        print(" Pt 1: Min cost to win: \(minCost) gold")
        print(" Pt 2: Max cost to lose: \(maxCost) gold")
    }
    
    private func CheckLoadout(loadout:[Item?]) {
        let cost = Cost(loadout)
        let win = Fight(loadout)
        if cost < self.minCost && win {
            self.minCost = cost
        }
        if cost > self.maxCost && !win {
            self.maxCost = cost
        }
    }
    
    private func Cost(loadout:[Item?]) -> Int {
        var sum = 0;
        loadout.forEach { if $0 != nil {sum += $0!.cost}}
        return sum
    }
    
    // fights the two, returns true if the player wins
    private func Fight(loadout:[Item?]) -> Bool {
        ResetBoss()
        ApplyItems(loadout)
        let pDamage = boss.armor > player.damage ? 1 : player.damage - boss.armor
        let bDamage = player.armor > boss.damage ? 1 : boss.damage - player.armor
        while true {
            boss.health -= pDamage
            if boss.health <= 0 {
                return true
            }
            player.health -= bDamage
            if player.health <= 0 {
                return false
            }
        }
    }
    
    private func ResetBoss() {
        boss.health = 104
        boss.armor = 1
        boss.damage = 8
    }
    
    private func ApplyItems(toApply:[Item?]) {
        player.health = 100
        player.damage = 0
        player.armor = 0
        for i in toApply {
            if i != nil {
                player.armor += i!.armor
                player.damage += i!.damage
            }
        }
    }
}