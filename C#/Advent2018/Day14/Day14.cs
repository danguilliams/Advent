using System.Collections.Generic;
using System.Linq;

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
     * 
     * Your puzzle answer was 2145581131.
     * 
     * The first half of this puzzle is complete! It provides one gold star: *
     * 
     * --- Part Two ---
     * As it turns out, you got the Elves' plan backwards. They actually want to know how many recipes appear on the scoreboard to the left of the first recipes whose scores are the digits from your puzzle input.
     * 
     * 51589 first appears after 9 recipes.
     * 01245 first appears after 5 recipes.
     * 92510 first appears after 18 recipes.
     * 59414 first appears after 2018 recipes.
     * How many recipes appear on the scoreboard to the left of the score sequence in your puzzle input?
     */
    public class Day14 : Day
    {
        public override int PuzzleDay => 14;

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

            for(int recipeId = 3; recipeId <= TotalRecipes + 10; recipeId++)
            {
                int newScore = elf1.Value + elf2.Value;

                // score will never be more than 18, so first score is always 1
                // iff the new score is over 10
                if(newScore >= 10)
                {
                    scores.AddLast(1);
                    recipeId++;
                }
                // add score for 1s digit
                scores.AddLast(newScore % 10);

                Advance(ref elf1);
                Advance(ref elf2);
            }

            LinkedListNode<int> current = scores.First;
            
            for(int i = 0; i < TotalRecipes; i++)
            {
                current = current.Next;
            }

            string next10 = "";
            for(int i = 0; i < 10; i++)
            {
                next10 += current.Value;
                current = current.Next;
            }

            return next10;
        }

        private void Advance(ref LinkedListNode<int> node)
        {
            int advances = node.Value + 1;
            for (int i = 0; i < advances; i++)
            {
                if (node.Next == null)
                {
                    node = node.List.First;
                }
                else
                {
                    node = node.Next;
                }
            }
        }

        protected override string Part2()
        {
            int[] targetScores = TotalRecipes.ToString().ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();

            LinkedList<int> scores = new LinkedList<int>();
            scores.AddLast(FirstScore);
            scores.AddLast(SecondScore);

            LinkedListNode<int> elf1 = scores.First;
            LinkedListNode<int> elf2 = elf1.Next;
            int tIdx = targetScores.Length - 1;
            while (true)
            {
                int newScore = elf1.Value + elf2.Value;

                // score will never be more than 18, so first score is always 1
                // iff the new score is over 10
                if (newScore >= 10)
                {
                    scores.AddLast(1);

                    if (targetScores[tIdx] == 1 && ScoresMatch(scores, targetScores))
                    {
                        break;
                    }
                }
                // add score for 1s digit
                newScore = newScore % 10;
                scores.AddLast(newScore);

                if (targetScores[tIdx] == newScore && ScoresMatch(scores, targetScores))
                {
                    break;
                }
                
                Advance(ref elf1);
                Advance(ref elf2);
            }

            return $"{scores.Count - targetScores.Length} recipes";
        }

        private bool ScoresMatch(LinkedList<int> scores, int[] targets)
        {
            LinkedListNode<int> current = scores.Last;

            for(int i = targets.Length - 1; i >= 0 && current != null; i--)
            {
                int v1 = current.Value;
                int v2 = targets[i];
                if(current.Value != targets[i])
                {
                    return false;
                }

                current = current.Previous;
            }

            return true;
        }
    }
}
