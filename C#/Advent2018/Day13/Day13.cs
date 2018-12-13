using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode2018
{
    /*
     * --- Day 13: Mine Cart Madness ---
     * A crop of this size requires significant logistics to transport produce, soil, fertilizer, and so on. The Elves are very busy pushing things around in carts on some kind of rudimentary system of tracks they've come up with.
     * 
     * Seeing as how cart-and-track systems don't appear in recorded history for another 1000 years, the Elves seem to be making this up as they go along. They haven't even figured out how to avoid collisions yet.
     * 
     * You map out the tracks (your puzzle input) and see where you can help.
     * 
     * Tracks consist of straight paths (| and -), curves (/ and \), and intersections (+). Curves connect exactly two perpendicular pieces of track; for example, this is a closed loop:
     * 
     * /----\
     * |    |
     * |    |
     * \----/
     * Intersections occur when two perpendicular paths cross. At an intersection, a cart is capable of turning left, turning right, or continuing straight. Here are two loops connected by two intersections:
     * 
     * /-----\
     * |     |
     * |  /--+--\
     * |  |  |  |
     * \--+--/  |
     *    |     |
     *    \-----/
     * Several carts are also on the tracks. Carts always face either up (^), down (v), left (<), or right (>). (On your initial map, the track under each cart is a straight path matching the direction the cart is facing.)
     * 
     * Each time a cart has the option to turn (by arriving at any intersection), it turns left the first time, goes straight the second time, turns right the third time, and then repeats those directions starting again with left the fourth time, straight the fifth time, and so on. This process is independent of the particular intersection at which the cart has arrived - that is, the cart has no per-intersection memory.
     * 
     * Carts all move at the same speed; they take turns moving a single step at a time. They do this based on their current location: carts on the top row move first (acting from left to right), then carts on the second row move (again from left to right), then carts on the third row, and so on. Once each cart has moved one step, the process repeats; each of these loops is called a tick.
     * 
     * For example, suppose there are two carts on a straight track:
     * 
     * |  |  |  |  |
     * v  |  |  |  |
     * |  v  v  |  |
     * |  |  |  v  X
     * |  |  ^  ^  |
     * ^  ^  |  |  |
     * |  |  |  |  |
     * First, the top cart moves. It is facing down (v), so it moves down one square. Second, the bottom cart moves. It is facing up (^), so it moves up one square. Because all carts have moved, the first tick ends. Then, the process repeats, starting with the first cart. The first cart moves down, then the second cart moves up - right into the first cart, colliding with it! (The location of the crash is marked with an X.) This ends the second and last tick.
     * 
     * Here is a longer example:
     * 
     * /->-\        
     * |   |  /----\
     * | /-+--+-\  |
     * | | |  | v  |
     * \-+-/  \-+--/
     *   \------/   
     * 
     * /-->\        
     * |   |  /----\
     * | /-+--+-\  |
     * | | |  | |  |
     * \-+-/  \->--/
     *   \------/   
     * 
     * /---v        
     * |   |  /----\
     * | /-+--+-\  |
     * | | |  | |  |
     * \-+-/  \-+>-/
     *   \------/   
     * 
     * /---\        
     * |   v  /----\
     * | /-+--+-\  |
     * | | |  | |  |
     * \-+-/  \-+->/
     *   \------/   
     * 
     * /---\        
     * |   |  /----\
     * | /->--+-\  |
     * | | |  | |  |
     * \-+-/  \-+--^
     *   \------/   
     * 
     * /---\        
     * |   |  /----\
     * | /-+>-+-\  |
     * | | |  | |  ^
     * \-+-/  \-+--/
     *   \------/   
     * 
     * /---\        
     * |   |  /----\
     * | /-+->+-\  ^
     * | | |  | |  |
     * \-+-/  \-+--/
     *   \------/   
     * 
     * /---\        
     * |   |  /----<
     * | /-+-->-\  |
     * | | |  | |  |
     * \-+-/  \-+--/
     *   \------/   
     * 
     * /---\        
     * |   |  /---<\
     * | /-+--+>\  |
     * | | |  | |  |
     * \-+-/  \-+--/
     *   \------/   
     * 
     * /---\        
     * |   |  /--<-\
     * | /-+--+-v  |
     * | | |  | |  |
     * \-+-/  \-+--/
     *   \------/   
     * 
     * /---\        
     * |   |  /-<--\
     * | /-+--+-\  |
     * | | |  | v  |
     * \-+-/  \-+--/
     *   \------/   
     * 
     * /---\        
     * |   |  /<---\
     * | /-+--+-\  |
     * | | |  | |  |
     * \-+-/  \-<--/
     *   \------/   
     * 
     * /---\        
     * |   |  v----\
     * | /-+--+-\  |
     * | | |  | |  |
     * \-+-/  \<+--/
     *   \------/   
     * 
     * /---\        
     * |   |  /----\
     * | /-+--v-\  |
     * | | |  | |  |
     * \-+-/  ^-+--/
     *   \------/   
     * 
     * /---\        
     * |   |  /----\
     * | /-+--+-\  |
     * | | |  X |  |
     * \-+-/  \-+--/
     *   \------/   
     * After following their respective paths for a while, the carts eventually crash. To help prevent crashes, you'd like to know the location of the first crash. Locations are given in X,Y coordinates, where the furthest left column is X=0 and the furthest top row is Y=0:
     * 
     *            111
     *  0123456789012
     * 0/---\        
     * 1|   |  /----\
     * 2| /-+--+-\  |
     * 3| | |  X |  |
     * 4\-+-/  \-+--/
     * 5  \------/   
     * In this example, the location of the first crash is 7,3.
     */
    public class Day13 : Day
    {
        public override int PuzzleDay => 13;

        private char[,] Track { get; set; }

        private List<Cart> Carts { get; set; }

        protected override void ProcessInput()
        {
            Track = new char[150, 150];
            Carts = new List<Cart>();
            for (int y = 0; y < 150; y++)
            {
                for (int x = 0; x < 150; x++)
                {
                    char next = Input[y][x];
                    if (next == '>' || next == '<')
                    {
                        Carts.Add(new Cart(x, y, next));
                        Track[x, y] = '-';
                    }
                    else if( next == '^' || next == 'v')
                    {
                        Carts.Add(new Cart(x, y, next));
                        Track[x, y] = '|';
                    }
                    else
                    {
                        Track[x, y] = next;
                    }
                }
            }
        }

        protected override string Part1()
        {
            return "unfinished";
        }

        protected override string Part2()
        {
            return "unfinished";
        }

        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        private enum Turn
        {
            Left = 0,
            Straight = 1,
            Right = 2,
        }

        private class Cart
        {
            public Cart(int x, int y, char d)
            {
                X = x;
                Y = y;
                switch (d)
                {
                    case '>':
                        Dir = Direction.Right;
                        break;
                    case '<':
                        Dir = Direction.Left;
                        break;
                    case '^':
                        Dir = Direction.Up;
                        break;
                    case 'v':
                        Dir = Direction.Down;
                        break;
                }

                NextTurn = Turn.Right;

            }

            public int X;
            public int Y;
            public Direction Dir;
            public Turn NextTurn;

            public void Move(char[,] Track)
            {
                switch (Dir)
                {
                    case Direction.Left:
                        X--;
                        break;
                    case Direction.Right:
                        X++;
                        break;
                    case Direction.Up:
                        Y--;
                        break;
                    case Direction.Down:
                        Y++;
                        break;
                }
                char next = Track[X, Y];
                if (next == '+')
                {
                    Intersection();
                }
                else if (next == '\\')
                {
                    if (Dir == Direction.Left || Dir == Direction.Right)
                    {
                        TurnRight();
                    }
                    else
                    {
                        TurnLeft();
                    }
                }
                else if (next == '/')
                {
                    if (Dir == Direction.Left || Dir == Direction.Right)
                    {
                        TurnLeft();
                    }
                    else
                    {
                        TurnRight();
                    }
                }



            }

            private void Intersection()
            {
                switch (NextTurn)
                {
                    case Turn.Left:
                        TurnLeft();
                        NextTurn = Turn.Straight;
                        break;
                    case Turn.Straight:
                        NextTurn = Turn.Right;
                        break;
                    case Turn.Right:
                        TurnRight();
                        NextTurn = Turn.Left;
                        break;
                }
            }

            private void TurnLeft()
            {
                switch (Dir)
                {
                    case Direction.Left:
                        Dir = Direction.Down;
                        break;
                    case Direction.Right:
                        Dir = Direction.Up;
                        break;
                    case Direction.Up:
                        Dir = Direction.Left;
                        break;
                    case Direction.Down:
                        Dir = Direction.Right;
                        break;
                }
            }

            private void TurnRight()
            {
                switch (Dir)
                {
                    case Direction.Left:
                        Dir = Direction.Up;
                        break;
                    case Direction.Right:
                        Dir = Direction.Down;
                        break;
                    case Direction.Up:
                        Dir = Direction.Right;
                        break;
                    case Direction.Down:
                        Dir = Direction.Left;
                        break;
                }
            }
        }
    }
}
