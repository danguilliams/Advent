using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace AdventOfCode2018
{
    /*
     * --- Day 15: Beverage Bandits ---
     * Having perfected their hot chocolate, the Elves have a new problem: the Goblins that live in these caves will do anything to steal it. Looks like they're here for a fight.
     * 
     * You scan the area, generating a map of the walls (#), open cavern (.), and starting position of every Goblin (G) and Elf (E) (your puzzle input).
     * 
     * Combat proceeds in rounds; in each round, each unit that is still alive takes a turn, resolving all of its actions before the next unit's turn begins. On each unit's turn, it tries to move into range of an enemy (if it isn't already) and then attack (if it is in range).
     * 
     * All units are very disciplined and always follow very strict combat rules. Units never move or attack diagonally, as doing so would be dishonorable. When multiple choices are equally valid, ties are broken in reading order: top-to-bottom, then left-to-right. For instance, the order in which units take their turns within a round is the reading order of their starting positions in that round, regardless of the type of unit or whether other units have moved after the round started. For example:
     * 
     *                  would take their
     * These units:   turns in this order:
     *   #######           #######
     *   #.G.E.#           #.1.2.#
     *   #E.G.E#           #3.4.5#
     *   #.G.E.#           #.6.7.#
     *   #######           #######
     * Each unit begins its turn by identifying all possible targets (enemy units). If no targets remain, combat ends.
     * 
     * Then, the unit identifies all of the open squares (.) that are in range of each target; these are the squares which are adjacent (immediately up, down, left, or right) to any target and which aren't already occupied by a wall or another unit. Alternatively, the unit might already be in range of a target. If the unit is not already in range of a target, and there are no open squares which are in range of a target, the unit ends its turn.
     * 
     * If the unit is already in range of a target, it does not move, but continues its turn with an attack. Otherwise, since it is not in range of a target, it moves.
     * 
     * To move, the unit first considers the squares that are in range and determines which of those squares it could reach in the fewest steps. A step is a single movement to any adjacent (immediately up, down, left, or right) open (.) square. Units cannot move into walls or other units. The unit does this while considering the current positions of units and does not do any prediction about where units will be later. If the unit cannot reach (find an open path to) any of the squares that are in range, it ends its turn. If multiple squares are in range and tied for being reachable in the fewest steps, the step which is first in reading order is chosen. For example:
     * 
     * Targets:      In range:     Reachable:    Nearest:      Chosen:
     * #######       #######       #######       #######       #######
     * #E..G.#       #E.?G?#       #E.@G.#       #E.!G.#       #E.+G.#
     * #...#.#  -->  #.?.#?#  -->  #.@.#.#  -->  #.!.#.#  -->  #...#.#
     * #.G.#G#       #?G?#G#       #@G@#G#       #!G.#G#       #.G.#G#
     * #######       #######       #######       #######       #######
     * In the above scenario, the Elf has three targets (the three Goblins):
     * 
     * Each of the Goblins has open, adjacent squares which are in range (marked with a ? on the map).
     * Of those squares, four are reachable (marked @); the other two (on the right) would require moving through a wall or unit to reach.
     * Three of these reachable squares are nearest, requiring the fewest steps (only 2) to reach (marked !).
     * Of those, the square which is first in reading order is chosen (+).
     * The unit then takes a single step toward the chosen square along the shortest path to that square. If multiple steps would put the unit equally closer to its destination, the unit chooses the step which is first in reading order. (This requires knowing when there is more than one shortest path so that you can consider the first step of each such path.) For example:
     * 
     * In range:     Nearest:      Chosen:       Distance:     Step:
     * #######       #######       #######       #######       #######
     * #.E...#       #.E...#       #.E...#       #4E212#       #..E..#
     * #...?.#  -->  #...!.#  -->  #...+.#  -->  #32101#  -->  #.....#
     * #..?G?#       #..!G.#       #...G.#       #432G2#       #...G.#
     * #######       #######       #######       #######       #######
     * The Elf sees three squares in range of a target (?), two of which are nearest (!), and so the first in reading order is chosen (+). Under "Distance", each open square is marked with its distance from the destination square; the two squares to which the Elf could move on this turn (down and to the right) are both equally good moves and would leave the Elf 2 steps from being in range of the Goblin. Because the step which is first in reading order is chosen, the Elf moves right one square.
     * 
     * Here's a larger example of movement:
     * 
     * Initially:
     * #########
     * #G..G..G#
     * #.......#
     * #.......#
     * #G..E..G#
     * #.......#
     * #.......#
     * #G..G..G#
     * #########
     * 
     * After 1 round:
     * #########
     * #.G...G.#
     * #...G...#
     * #...E..G#
     * #.G.....#
     * #.......#
     * #G..G..G#
     * #.......#
     * #########
     * 
     * After 2 rounds:
     * #########
     * #..G.G..#
     * #...G...#
     * #.G.E.G.#
     * #.......#
     * #G..G..G#
     * #.......#
     * #.......#
     * #########
     * 
     * After 3 rounds:
     * #########
     * #.......#
     * #..GGG..#
     * #..GEG..#
     * #G..G...#
     * #......G#
     * #.......#
     * #.......#
     * #########
     * Once the Goblins and Elf reach the positions above, they all are either in range of a target or cannot find any square in range of a target, and so none of the units can move until a unit dies.
     * 
     * After moving (or if the unit began its turn in range of a target), the unit attacks.
     * 
     * To attack, the unit first determines all of the targets that are in range of it by being immediately adjacent to it. If there are no such targets, the unit ends its turn. Otherwise, the adjacent target with the fewest hit points is selected; in a tie, the adjacent target with the fewest hit points which is first in reading order is selected.
     * 
     * The unit deals damage equal to its attack power to the selected target, reducing its hit points by that amount. If this reduces its hit points to 0 or fewer, the selected target dies: its square becomes . and it takes no further turns.
     * 
     * Each unit, either Goblin or Elf, has 3 attack power and starts with 200 hit points.
     * 
     * For example, suppose the only Elf is about to attack:
     * 
     *        HP:            HP:
     * G....  9       G....  9  
     * ..G..  4       ..G..  4  
     * ..EG.  2  -->  ..E..     
     * ..G..  2       ..G..  2  
     * ...G.  1       ...G.  1  
     * The "HP" column shows the hit points of the Goblin to the left in the corresponding row. The Elf is in range of three targets: the Goblin above it (with 4 hit points), the Goblin to its right (with 2 hit points), and the Goblin below it (also with 2 hit points). Because three targets are in range, the ones with the lowest hit points are selected: the two Goblins with 2 hit points each (one to the right of the Elf and one below the Elf). Of those, the Goblin first in reading order (the one to the right of the Elf) is selected. The selected Goblin's hit points (2) are reduced by the Elf's attack power (3), reducing its hit points to -1, killing it.
     * 
     * After attacking, the unit's turn ends. Regardless of how the unit's turn ends, the next unit in the round takes its turn. If all units have taken turns in this round, the round ends, and a new round begins.
     * 
     * The Elves look quite outnumbered. You need to determine the outcome of the battle: the number of full rounds that were completed (not counting the round in which combat ends) multiplied by the sum of the hit points of all remaining units at the moment combat ends. (Combat only ends when a unit finds no targets during its turn.)
     * 
     * Below is an entire sample combat. Next to each map, each row's units' hit points are listed from left to right.
     * 
     * Initially:
     * #######   
     * #.G...#   G(200)
     * #...EG#   E(200), G(200)
     * #.#.#G#   G(200)
     * #..G#E#   G(200), E(200)
     * #.....#   
     * #######   
     * 
     * After 1 round:
     * #######   
     * #..G..#   G(200)
     * #...EG#   E(197), G(197)
     * #.#G#G#   G(200), G(197)
     * #...#E#   E(197)
     * #.....#   
     * #######   
     * 
     * After 2 rounds:
     * #######   
     * #...G.#   G(200)
     * #..GEG#   G(200), E(188), G(194)
     * #.#.#G#   G(194)
     * #...#E#   E(194)
     * #.....#   
     * #######   
     * 
     * Combat ensues; eventually, the top Elf dies:
     * 
     * After 23 rounds:
     * #######   
     * #...G.#   G(200)
     * #..G.G#   G(200), G(131)
     * #.#.#G#   G(131)
     * #...#E#   E(131)
     * #.....#   
     * #######   
     * 
     * After 24 rounds:
     * #######   
     * #..G..#   G(200)
     * #...G.#   G(131)
     * #.#G#G#   G(200), G(128)
     * #...#E#   E(128)
     * #.....#   
     * #######   
     * 
     * After 25 rounds:
     * #######   
     * #.G...#   G(200)
     * #..G..#   G(131)
     * #.#.#G#   G(125)
     * #..G#E#   G(200), E(125)
     * #.....#   
     * #######   
     * 
     * After 26 rounds:
     * #######   
     * #G....#   G(200)
     * #.G...#   G(131)
     * #.#.#G#   G(122)
     * #...#E#   E(122)
     * #..G..#   G(200)
     * #######   
     * 
     * After 27 rounds:
     * #######   
     * #G....#   G(200)
     * #.G...#   G(131)
     * #.#.#G#   G(119)
     * #...#E#   E(119)
     * #...G.#   G(200)
     * #######   
     * 
     * After 28 rounds:
     * #######   
     * #G....#   G(200)
     * #.G...#   G(131)
     * #.#.#G#   G(116)
     * #...#E#   E(113)
     * #....G#   G(200)
     * #######   
     * 
     * More combat ensues; eventually, the bottom Elf dies:
     * 
     * After 47 rounds:
     * #######   
     * #G....#   G(200)
     * #.G...#   G(131)
     * #.#.#G#   G(59)
     * #...#.#   
     * #....G#   G(200)
     * #######   
     * Before the 48th round can finish, the top-left Goblin finds that there are no targets remaining, and so combat ends. So, the number of full rounds that were completed is 47, and the sum of the hit points of all remaining units is 200+131+59+200 = 590. From these, the outcome of the battle is 47 * 590 = 27730.
     * 
     * Here are a few example summarized combats:
     * 
     * #######       #######
     * #G..#E#       #...#E#   E(200)
     * #E#E.E#       #E#...#   E(197)
     * #G.##.#  -->  #.E##.#   E(185)
     * #...#E#       #E..#E#   E(200), E(200)
     * #...E.#       #.....#
     * #######       #######
     * 
     * Combat ends after 37 full rounds
     * Elves win with 982 total hit points left
     * Outcome: 37 * 982 = 36334
     * #######       #######   
     * #E..EG#       #.E.E.#   E(164), E(197)
     * #.#G.E#       #.#E..#   E(200)
     * #E.##E#  -->  #E.##.#   E(98)
     * #G..#.#       #.E.#.#   E(200)
     * #..E#.#       #...#.#   
     * #######       #######   
     * 
     * Combat ends after 46 full rounds
     * Elves win with 859 total hit points left
     * Outcome: 46 * 859 = 39514
     * #######       #######   
     * #E.G#.#       #G.G#.#   G(200), G(98)
     * #.#G..#       #.#G..#   G(200)
     * #G.#.G#  -->  #..#..#   
     * #G..#.#       #...#G#   G(95)
     * #...E.#       #...G.#   G(200)
     * #######       #######   
     * 
     * Combat ends after 35 full rounds
     * Goblins win with 793 total hit points left
     * Outcome: 35 * 793 = 27755
     * #######       #######   
     * #.E...#       #.....#   
     * #.#..G#       #.#G..#   G(200)
     * #.###.#  -->  #.###.#   
     * #E#G#G#       #.#.#.#   
     * #...#G#       #G.G#G#   G(98), G(38), G(200)
     * #######       #######   
     * 
     * Combat ends after 54 full rounds
     * Goblins win with 536 total hit points left
     * Outcome: 54 * 536 = 28944
     * #########       #########   
     * #G......#       #.G.....#   G(137)
     * #.E.#...#       #G.G#...#   G(200), G(200)
     * #..##..G#       #.G##...#   G(200)
     * #...##..#  -->  #...##..#   
     * #...#...#       #.G.#...#   G(200)
     * #.G...G.#       #.......#   
     * #.....G.#       #.......#   
     * #########       #########   
     * 
     * Combat ends after 20 full rounds
     * Goblins win with 937 total hit points left
     * Outcome: 20 * 937 = 18740
     * What is the outcome of the combat described in your puzzle input?
     */
    public class Day15 : Day
    {
        public override int PuzzleDay => 15;

        private char[,] Cave;
        private const char Wall = '#';
        private const char Open = '.';
        private int CaveWidth;
        private int CaveHeight;
        private IEnumerable<Mob> Elves { get { return Mobs.Where(m => m.IsElf); } }
        private int ElfCount { get { return Mobs.Count(m => m.IsElf); } }
        private int GoblinCount { get { return Mobs.Count(m => !m.IsElf); } }
        private IEnumerable<Mob> Goblins { get { return Mobs.Where(m => !m.IsElf); } }
        private List<Mob> Mobs;
        private Vec OrigCursPos;

        protected override void ProcessInput()
        {
            string[] input = Input;
                //{ "#######",
                //  "#.G...#",
                //  "#...EG#",
                //  "#.#.#G#",
                //  "#..G#E#",
                //  "#.....#",
                //  "#######"};

            Input = input;
            CaveWidth = Input[0].Length;
            CaveHeight = Input.Length;

            Console.WindowHeight = Math.Max(Console.WindowHeight, CaveHeight + CaveHeight / 4);
            OrigCursPos = new Vec(Console.CursorLeft, Console.CursorTop);
            Console.CursorVisible = false;

            Cave = new char[CaveWidth, CaveHeight];
            Mobs = new List<Mob>();

            for (int y = 0; y < Input.Length; y++)
            {
                for (int x = 0; x < Input[y].Length; x++)
                {
                    char c = Input[y][x];
                    switch (c)
                    {
                        case 'E':
                            Elf newElf = new Elf(Elves.Count(), x, y, Cave);
                            Mobs.Add(newElf);
                            Cave[x, y] = newElf.Id;
                            break;
                        case 'G':
                            Goblin newGoblin = new Goblin(Goblins.Count(), x, y, Cave);
                            Mobs.Add(newGoblin);
                            Cave[x, y] = newGoblin.Id;
                            break;
                        default:
                            Cave[x, y] = c;
                            break;
                    }
                }
            }
        }

        protected override string Part1()
        {
            int turn = 0;
            Stopwatch t = new Stopwatch();
            t.Start();
            while(ElfCount > 0 && GoblinCount > 0)
            {
                t.Reset();
                PrintCave(turn);

                Console.ForegroundColor = ConsoleColor.White;
                StringBuilder sb = new StringBuilder();
                foreach (Mob mob in Mobs.OrderBy(m => m.Pos.Y * 10000 + m.Pos.X).ToList())
                {
                    if(mob.Alive && OpponentCount(mob) > 0)
                    {
                        sb.Append(mob.Id);
                        if(!mob.CanAttack())
                        {
                            mob.Move(Mobs.Where(m => m.IsElf != mob.IsElf));
                        }

                        Mob attacked = mob.Attack(Mobs.Where(m => m.IsElf != mob.IsElf));
                        if(attacked != null && !attacked.Alive)
                        {
                            Mobs.Remove(attacked);
                            Cave[attacked.Pos.X, attacked.Pos.Y] = Open;
                        }
                    }

                    //PrintCave(turn, mob);
                }
                Console.WriteLine("Turn Order: " + sb.ToString());
                turn++;
                t.Stop();
                if(t.ElapsedMilliseconds < 100)
                {
                    Thread.Sleep(100 - (int)t.ElapsedMilliseconds);
                }
            }

            PrintCave(turn);

            int health = 0;
            if(Mobs.Any(m => m.IsElf))
            {
                // elves win
                health = Mobs.Where(m => m.IsElf && m.Alive).Sum(m => m.HP);
            }
            else
            {
                health = Mobs.Where(m => !m.IsElf && m.Alive).Sum(m => m.HP);
            }

            int result = health * (turn );
            // wrong: 231182 (too low?)

            return $"{result}";
        }

        private int OpponentCount(Mob mob)
        {
            return mob.IsElf ? GoblinCount : ElfCount; 
        }

        protected override string Part2()
        {
            return "unsolved";
        }

        [DebuggerDisplay("[{X},{Y}]")]
        private class Vec 
        {
            public Vec(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X;
            public int Y;

            public static bool operator==(Vec a, Vec b)
            {
                return a?.X == b?.X && a?.Y == b?.Y;
            }

            public static bool operator!=(Vec a, Vec b)
            {
                return a?.X != b?.X || a?.Y != b?.Y;
            }

            public override bool Equals(object obj)
            {
                Vec o = (Vec)obj;
                return o != null && X == o.X && Y == o.Y;
            }

            public override int GetHashCode()
            {
                // "reading order" of the vector
                return X + Y * 10000;
            }
        }

        private class VecComparer : IEqualityComparer<Vec>
        {
            public bool Equals(Vec x, Vec y)
            {
                return x == y;
            }

            public int GetHashCode(Vec obj)
            {
                return obj.X + obj.Y * 10000;
            }
        }

        private static IList<Vec> GetNeighbors(char[,] cave, Vec pos)
        {
            IList<Vec> n = new List<Vec>(4);
            int xSize = cave.GetLength(0);
            int ySize = cave.GetLength(1);
            if (pos.Y > 0)
            {
                n.Add(new Vec(pos.X, pos.Y - 1));
            }
            if (pos.X > 0)
            {
                n.Add(new Vec(pos.X - 1, pos.Y));
            }
            if(pos.X < ySize - 1)
            {
                n.Add(new Vec(pos.X + 1, pos.Y));
            }
            if(pos.Y < ySize - 1)
            {
                n.Add(new Vec(pos.X, pos.Y + 1));
            }
            return n;
        }

        private class Mob
        {
            protected Mob(char id, int xPos, int yPos, char[,] cave, bool isElf)
            {
                Id = id;
                Pos = new Vec(xPos, yPos);
                HP = 200;
                Atk = 3;
                Moved = false;
                IsElf = isElf;
                Cave = cave;
            }

            public char Id;
            public Vec Pos;
            public int HP;
            public int Atk;
            public bool Moved;
            public bool IsElf;
            private char[,] Cave;
            public int X => Pos.X;
            public int Y => Pos.Y;

            public bool Alive { get { return HP > 0; } }
            public bool CanAttack() {
                
                //check in reading order
                return IsEnemy(Cave[Pos.X, Pos.Y - 1]) ||
                       IsEnemy(Cave[Pos.X - 1, Pos.Y]) ||
                       IsEnemy(Cave[Pos.X + 1, Pos.Y]) ||
                       IsEnemy(Cave[Pos.X, Pos.Y + 1]);
            }

            public bool IsEnemy(char c)
            {
                if (c == Wall || c == Open)
                {
                    return false;
                }
                if(IsElf)
                {
                    return c >= 65;
                }
                else
                {
                    return c < 65;
                }
            }

            public Mob Attack(IEnumerable<Mob> enemies)
            {
                // get adjacent enemies, order by HP
                IList<Mob> attackables = enemies.Where(m => Math.Abs(m.X - X) + Math.Abs(m.Y - Y) == 1).OrderBy(m => m.HP).ToList();
                
                if(attackables.Any())
                {
                    // first in list has lowest HP
                    Mob best = attackables.First();
                    
                    // if multiple are tied for lowest HP, chose first in reading order
                    if(attackables.Count(m => m.HP == best.HP) > 1)
                    {
                        best = attackables.Where(m => m.HP == best.HP).OrderBy(m => m.Y * 10000 + m.X).First();
                    }

                    best.HP -= Atk;
                    return best;
                }
                else
                {
                    return null;
                }
            }


            public void Move(IEnumerable<Mob> enemies)
            {
                // get a graph of the cave
                Dictionary<Vec, CNode> graph = InitializeDistGraph(enemies);
                // set distance origin to 0
                graph[Pos].Dist = 0;
                graph[Pos].Discovered = true;
                while (graph.Values.Any(v => !v.Visited && v.Discovered))
                {
                    CNode current = graph.Values.Where(c => !c.Visited).OrderBy(c => c.Dist).First();
                    current.Visited = true;

                    foreach (Vec v in current.Neighbors)
                    {
                        CNode n = graph[v];
                        n.Discovered = true;
                        int newDist = current.Dist + 1;
                        if (newDist < 0)
                        {
                            newDist = int.MaxValue;
                        }
                        if(newDist < n.Dist)
                        {
                            n.Dist = newDist;
                            n.Prev = current.Pos;
                        }
                    }
                }

                List<CNode> candidates = graph.Values.Where(v => v.IsTarget).OrderBy(v => v.Dist).ToList();
                
                if(candidates.Count > 0)
                {
                    int closest = candidates[0].Dist;
                    Dictionary<CNode,List<Vec>> shortestPaths = new Dictionary<CNode,List<Vec>>();
                    foreach (CNode cand in candidates.Where(c => c.Dist == closest))
                    {
                        List<Vec> path = GetShortestPath(graph, cand);
                        Vec n = path.FirstOrDefault();
                        if (n != null)
                        {
                            shortestPaths.Add(cand, path);
                        }
                    }

                    if (shortestPaths.Any())
                    {
                        Vec next = shortestPaths.Values.OrderBy(p => p[0].GetHashCode()).First()[0];

                        if (Math.Abs(next.X - X) + Math.Abs(next.Y - Y) != 1)
                        {
                            throw new Exception("Next step should not be more that 1 space away!");
                        }

                        Cave[X, Y] = Open;
                        Pos = next;
                        Cave[X, Y] = Id;
                    }
                }
            }

            private List<Vec> GetShortestPath(Dictionary<Vec, CNode> graph, CNode target)
            {
                CNode current = target;
                Vec next = target.Pos;
                List<Vec> vecStack = new List<Vec>();
                while(next != Pos)
                {
                    if(graph[next].Prev == null)
                    {
                        return new List<Vec>();
                    }

                    vecStack.Add(next);
                    next = graph[next].Prev;
                }
                vecStack.Reverse();
                return vecStack;
            }

            private Dictionary<Vec, CNode> InitializeDistGraph(IEnumerable<Mob> enemies)
            {
                IList<Vec> targets = new List<Vec>();
                // we want to find the open spots next to enemies, these are the target nodes
                foreach(Mob m in enemies)
                {
                    foreach(Vec v in GetNeighbors(Cave, m.Pos))
                    {
                        if(Cave[v.X,v.Y] == Open)
                        {
                            targets.Add(v);
                        }
                    }
                }

                Dictionary<Vec,CNode> vertices = new Dictionary<Vec, CNode>();
                for (int y = 0; y < Cave.GetLength(1); y++)
                {
                    for (int x = 0; x < Cave.GetLength(0); x++)
                    {
                        // not adding walls
                        char c = Cave[x, y];
                        if(c != Wall)
                        {
                            if(c == Id || c == Open)
                            {
                                CNode newNode = new CNode(x, y);

                                foreach(Vec v in GetNeighbors(Cave,newNode.Pos))
                                {
                                    char n = Cave[v.X, v.Y];
                                    if(n == Open || n == Id)
                                    {
                                        newNode.Neighbors.Add(v);
                                    }
                                }

                                if(targets.Any(t => t == newNode.Pos))
                                {
                                    newNode.IsTarget = true;
                                }

                                vertices.Add(newNode.Pos, newNode);
                            }
                        }
                    }
                }

                return vertices;
            }
        }

        private class Elf : Mob
        {
            public static string Ids = "1234567890";
            public Elf(int id, int xPos, int yPos, char[,] cave) : base(Ids[id], xPos, yPos, cave, true)
            {
            }
        }

        private class Goblin : Mob
        {
            public static string Ids = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public Goblin(int id, int xPos, int yPos, char[,] cave) : base(Ids[id], xPos, yPos, cave, false)
            {
            }
        }

        [DebuggerDisplay("[{X},{Y}]")]
        private class CNode
        {
            public CNode(int x, int y)
            {
                Pos = new Vec(x,y);
                Visited = false;
                Dist = int.MaxValue - 100000;
                Prev = null;
                IsTarget = false;
                Discovered = false;
                Neighbors = new List<Vec>();
            }

            public int X => Pos.X;
            public int Y => Pos.Y;
            public Vec Pos;
            public bool Visited;
            public bool Discovered;
            public int Dist;
            public Vec Prev;
            public bool IsTarget;
            public List<Vec> Neighbors { get; private set; }
        }

        private void PrintCave(int turn, Mob lastMoved = null)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = OrigCursPos.Y + 1;
            Console.WriteLine($"Turn {turn} start:");
            int xDigits = CaveWidth.ToString().Length;
            int yDigits = CaveHeight.ToString().Length;
            for(int d = xDigits; d > 0; d--)
            {
                for(int x = 0; x < yDigits; x++)
                {
                    Console.Write(' ');
                }
                for(int x = 0; x < CaveWidth; x++)
                {
                    int digit = (x / ((int)Math.Pow(10,d - 1)))%10;
                    Console.Write(digit);
                }
                Console.Write('\n');
            }
            for (int y = 0; y < CaveHeight; y++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                for (int x = yDigits; x > 0; x--)
                {
                    int digit = (y / ((int)Math.Pow(10, x - 1))) % 10;
                    Console.Write(digit);
                }
                for (int x = 0; x < CaveWidth; x++)
                {
                    char c = Cave[x, y];
                    Console.ForegroundColor = GetColor(c);
                    Console.Write(c);
                }
                Console.Write('\n');
            }
            if(lastMoved != null)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Last moved: {lastMoved.Id}");
            }
        }

        private ConsoleColor GetColor(char obj)
        {
            if (obj == Wall)
            {
                return ConsoleColor.DarkGray;
            }
            else if (obj == Open)
            {
                return ConsoleColor.White;
            }
            else if (obj < 65) //elf
            {
                return ConsoleColor.Green;
            }
            else // goblin
            {
                return ConsoleColor.DarkYellow;
            }
        }
    }
}
