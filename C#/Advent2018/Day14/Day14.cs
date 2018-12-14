using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    /*
     * --- Day 14: Chocolate Charts ---
     * You finally have a chance to look at all of the produce moving around. Chocolate, cinnamon, mint, chili peppers, nutmeg, vanilla... the Elves must be growing these plants to make hot chocolate! As you realize this, you hear a conversation in the distance. When you go to investigate, you discover two Elves in what appears to be a makeshift underground kitchen/laboratory.
     * 
     * The Elves are trying to come up with the ultimate hot chocolate recipe; they're even maintaining a scoreboard which tracks the quality score (0-9) of each recipe.
     * 
     * Only two recipes are on the board: the first recipe got a score of 3, the second, 7. Each of the two Elves has a current recipe: the first Elf starts with the first recipe, and the second Elf starts with the second recipe.
     * 
     * To create new recipes, the two Elves combine their current recipes. This creates new recipes from the digits of the sum of the current recipes' scores. With the current recipes' scores of 3 and 7, their sum is 10, and so two new recipes would be created: the first with score 1 and the second with score 0. If the current recipes' scores were 2 and 3, the sum, 5, would only create one recipe (with a score of 5) with its single digit.
     * 
     * The new recipes are added to the end of the scoreboard in the order they are created. So, after the first round, the scoreboard is 3, 7, 1, 0.
     * 
     * After all new recipes are added to the scoreboard, each Elf picks a new current recipe. To do this, the Elf steps forward through the scoreboard a number of recipes equal to 1 plus the score of their current recipe. So, after the first round, the first Elf moves forward 1 + 3 = 4 times, while the second Elf moves forward 1 + 7 = 8 times. If they run out of recipes, they loop back around to the beginning. After the first round, both Elves happen to loop around until they land on the same recipe that they had in the beginning; in general, they will move to different recipes.
     * 
     * Drawing the first Elf as parentheses and the second Elf as square brackets, they continue this process:
     * 
     * (3)[7]
     * (3)[7] 1  0 
     *  3  7  1 [0](1) 0 
     *  3  7  1  0 [1] 0 (1)
     * (3) 7  1  0  1  0 [1] 2 
     *  3  7  1  0 (1) 0  1  2 [4]
     *  3  7  1 [0] 1  0 (1) 2  4  5 
     *  3  7  1  0 [1] 0  1  2 (4) 5  1 
     *  3 (7) 1  0  1  0 [1] 2  4  5  1  5 
     *  3  7  1  0  1  0  1  2 [4](5) 1  5  8 
     *  3 (7) 1  0  1  0  1  2  4  5  1  5  8 [9]
     *  3  7  1  0  1  0  1 [2] 4 (5) 1  5  8  9  1  6 
     *  3  7  1  0  1  0  1  2  4  5 [1] 5  8  9  1 (6) 7 
     *  3  7  1  0 (1) 0  1  2  4  5  1  5 [8] 9  1  6  7  7 
     *  3  7 [1] 0  1  0 (1) 2  4  5  1  5  8  9  1  6  7  7  9 
     *  3  7  1  0 [1] 0  1  2 (4) 5  1  5  8  9  1  6  7  7  9  2 
     * The Elves think their skill will improve after making a few recipes (your puzzle input). However, that could take ages; you can speed this up considerably by identifying the scores of the ten recipes after that. For example:
     * 
     * If the Elves think their skill will improve after making 9 recipes, the scores of the ten recipes after the first nine on the scoreboard would be 5158916779 (highlighted in the last line of the diagram).
     * After 5 recipes, the scores of the next ten would be 0124515891.
     * After 18 recipes, the scores of the next ten would be 9251071085.
     * After 2018 recipes, the scores of the next ten would be 5941429882.
     * What are the scores of the ten recipes immediately after the number of recipes in your puzzle input?
     * 
     * Your puzzle input is 147061.
     */
    public class Day14 : Day
    {
        public override int PuzzleDay =>14;

        private int TotalRecipes => 147061;

        private int FirstScore => 3;
        private int SecondScore => 7;

        protected override string Part1()
        {
            LinkedList<int> scores = new LinkedList<int>();
            scores.AddLast(FirstScore);
            scores.AddLast(SecondScore);
            LinkedListNode<int> elf1 = scores.First;
            LinkedListNode<int> elf2 = elf1.Next;

            for(int recipeId = 3; recipeId < TotalRecipes; recipeId++)
            {
                int newScore = elf1.Value + elf2.Value;

                // score will never be more than 18, so first score is always 1
                // iff the new score is over 10
                if(newScore >= 10)
                {
                    scores.AddLast(1);
                }
                // add score for 1s digit
                scores.AddLast(newScore % 10);

                int elfAdvance = elf1.Value + 1;
                for (int i = 0; i < elfAdvance; i++)
                {
                    if (elf1.Next == null)
                    {
                        elf1 = scores.First;
                    }
                    else
                    {
                        elf1 = elf1.Next;
                    }
                }

                elfAdvance = elf2.Value + 1;
                for (int i = 0; i < elfAdvance; i++)
                {
                    if (elf1.Next == null)
                    {
                        elf2 = scores.First;
                    }
                    else
                    {
                        elf2 = elf2.Next;
                    }
                }
            }

            LinkedListNode<int> current = scores.First;
            
            for(int i = 0; i < TotalRecipes; i++)
            {
                current = current.Next;
            }


            return "unsolved";
        }

        protected override string Part2()
        {
            return "unsolved";
        }

        [DebuggerDisplay("Val: {Val} Next:{Next?.Val} Prev:{Prev?.Val}")]
        private class LLN
        {
            public LLN(int val)
            {
                Val = val;
            }

            public int Val { get; private set; }
            public LLN Next { get; set; }
            public LLN Prev { get; set; }
        }
    }
}
