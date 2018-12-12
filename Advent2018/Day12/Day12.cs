using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode2018
{
    /*
     * --- Day 12: Subterranean Sustainability ---
     * The year 518 is significantly more underground than your history books implied. Either that, or you've arrived in a vast cavern network under the North Pole.
     * 
     * After exploring a little, you discover a long tunnel that contains a row of small pots as far as you can see to your left and right. A few of them contain plants - someone is trying to grow things in these geothermally-heated caves.
     * 
     * The pots are numbered, with 0 in front of you. To the left, the pots are numbered -1, -2, -3, and so on; to the right, 1, 2, 3.... Your puzzle input contains a list of pots from 0 to the right and whether they do (#) or do not (.) currently contain a plant, the initial state. (No other pots currently contain plants.) For example, an initial state of #..##.... indicates that pots 0, 3, and 4 currently contain plants.
     * 
     * Your puzzle input also contains some notes you find on a nearby table: someone has been trying to figure out how these plants spread to nearby pots. Based on the notes, for each generation of plants, a given pot has or does not have a plant based on whether that pot (and the two pots on either side of it) had a plant in the last generation. These are written as LLCRR => N, where L are pots to the left, C is the current pot being considered, R are the pots to the right, and N is whether the current pot will have a plant in the next generation. For example:
     * 
     * A note like ..#.. => . means that a pot that contains a plant but with no plants within two pots of it will not have a plant in it during the next generation.
     * A note like ##.## => . means that an empty pot with two plants on each side of it will remain empty in the next generation.
     * A note like .##.# => # means that a pot has a plant in a given generation if, in the previous generation, there were plants in that pot, the one immediately to the left, and the one two pots to the right, but not in the ones immediately to the right and two to the left.
     * It's not clear what these plants are for, but you're sure it's important, so you'd like to make sure the current configuration of plants is sustainable by determining what will happen after 20 generations.
     * 
     * For example, given the following input:
     * 
     * initial state: #..#.#..##......###...###
     * 
     * ...## => #
     * ..#.. => #
     * .#... => #
     * .#.#. => #
     * .#.## => #
     * .##.. => #
     * .#### => #
     * #.#.# => #
     * #.### => #
     * ##.#. => #
     * ##.## => #
     * ###.. => #
     * ###.# => #
     * ####. => #
     * For brevity, in this example, only the combinations which do produce a plant are listed. (Your input includes all possible combinations.) Then, the next 20 generations will look like this:
     * 
     *                  1         2         3     
     *        0         0         0         0     
     *  0: ...#..#.#..##......###...###...........
     *  1: ...#...#....#.....#..#..#..#...........
     *  2: ...##..##...##....#..#..#..##..........
     *  3: ..#.#...#..#.#....#..#..#...#..........
     *  4: ...#.#..#...#.#...#..#..##..##.........
     *  5: ....#...##...#.#..#..#...#...#.........
     *  6: ....##.#.#....#...#..##..##..##........
     *  7: ...#..###.#...##..#...#...#...#........
     *  8: ...#....##.#.#.#..##..##..##..##.......
     *  9: ...##..#..#####....#...#...#...#.......
     * 10: ..#.#..#...#.##....##..##..##..##......
     * 11: ...#...##...#.#...#.#...#...#...#......
     * 12: ...##.#.#....#.#...#.#..##..##..##.....
     * 13: ..#..###.#....#.#...#....#...#...#.....
     * 14: ..#....##.#....#.#..##...##..##..##....
     * 15: ..##..#..#.#....#....#..#.#...#...#....
     * 16: .#.#..#...#.#...##...#...#.#..##..##...
     * 17: ..#...##...#.#.#.#...##...#....#...#...
     * 18: ..##.#.#....#####.#.#.#...##...##..##..
     * 19: .#..###.#..#.#.#######.#.#.#..#.#...#..
     * 20: .#....##....#####...#######....#.#..##.
     * The generation is shown along the left, where 0 is the initial state. The pot numbers are shown along the top, where 0 labels the center pot, negative-numbered pots extend to the left, and positive pots extend toward the right. Remember, the initial state begins at pot 0, which is not the leftmost pot used in this example.
     * 
     * After one generation, only seven plants remain. The one in pot 0 matched the rule looking for ..#.., the one in pot 4 matched the rule looking for .#.#., pot 9 matched .##.., and so on.
     * 
     * In this example, after 20 generations, the pots shown as # contain plants, the furthest left of which is pot -2, and the furthest right of which is pot 34. Adding up all the numbers of plant-containing pots after the 20th generation produces 325.
     * 
     * After 20 generations, what is the sum of the numbers of all pots which contain a plant?
     * --- Part Two ---
     * You realize that 20 generations aren't enough. After all, these plants will need to last another 1500 years to even reach your timeline, not to mention your future.
     * 
     * After fifty billion (50000000000) generations, what is the sum of the numbers of all pots which contain a plant?
     * 
     */
    public class Day12 : Day
    {
        public override int PuzzleDay => 12;

        public List<Plat> Plats { get; private set; }

        private int Plant0Idx { get; set; }

        public bool[] Current { get; set; }
        public bool[] Next { get; set; }

        protected override void ProcessInput()
        {
            ProcessInitialState(Input[0]);

            Plats = new List<Plat>(32);
            
            foreach(string s in Input)
            {
                if(s.Length == 10)
                {
                    Plats.Add(new Plat(s));
                }
            }
        }

        private void ProcessInitialState(string input)
        {
            // initial state: ##.##.##..#..#.#.#.#...#...#####.###...#####.##..#####.#..#.##..#..#.#...#...##.##...#.##......####.

            string state = input.Split(':')[1].Trim();
            int plantCount = state.Length + 5;
            Plant0Idx = 3;
            Current = new bool[plantCount];
            Next = new bool[plantCount];

            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] == '#')
                    Current[i + Plant0Idx] = true;
            }
        }

        private void SwapCurrentPlants()
        {
            bool[] tmp = Current;
            Current = Next;
            Next = tmp;
        }

        protected override string Part1()
        {
            int Generations = 20;
            for (int gen = 0; gen < Generations; gen++)
            {
                RunGeneration();
            }

            int sum = 0;
            for (int i = 0; i < Current.Length; i++)
            {
                if (Current[i])
                    sum += i - Plant0Idx;
            }

            // 58 is wrong (was only counting # of plants, not summing indicies)
            // 2904 is wrong (too low, set up initial state wrong)
            // 2930 correct

            return $"Pot ID sum: {sum}";
        }

        protected override string Part2()
        {
            ProcessInitialState(Input[0]);
            // obviously were not going to run 50 billion generations, that would be impossible
            // so we have to hack this
            int Generations = 200;
            List<int> repeatScores = new List<int>();
            string plantState = GetTrimmedPlantStr(Current);
            int gen = 1;
            for (; gen < Generations; gen++)
            {
                RunGeneration();
                string newPlantState = GetTrimmedPlantStr(Current);
                // state of the plants hasn't changed, only shifted left or right (probably right)
                if(newPlantState.Equals(plantState))
                {
                    int sum = 0;
                    for (int i = 0; i < Current.Length; i++)
                    {
                        if (Current[i])
                            sum += i - Plant0Idx;
                    }

                    repeatScores.Add(sum);

                    if(repeatScores.Count > 1)
                    {
                        break;
                    }
                }
                plantState = newPlantState;
            }

            // get how much the score increases each iteration
            int diff = repeatScores[1] - repeatScores[0];
            // get remaining number of iterations
            long remainingGenerations = 50000000000 - gen;
            // add it up
            long score = remainingGenerations * diff + repeatScores[1];

            // correct: 3099999999491 - got it on first try (whaat?)
            return $"Pot Id sum: {score}";
        }

        private void RunGeneration()
        {
            for (int plant = 2; plant < Current.Length - 2; plant++)
            {
                foreach (Plat p in Plats)
                {
                    bool match = true;
                    for (int i = 0; i < 5; i++)
                    {
                        bool p1 = p.Pattern[i];
                        bool p2 = Current[plant + i - 2];
                        if (p.Pattern[i] != Current[plant + i - 2])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        Next[plant] = p.Result;
                        break;
                    }
                }
            }

            ExpandArrays();

            SwapCurrentPlants();
        }

        private void ExpandArrays()
        {
            int lastPlantIdx = int.MaxValue;
            for (int i = Current.Length - 1; i > 0; i--)
            {
                if (Current[i])
                {
                    lastPlantIdx = i;
                    break;
                }
            }

            if(lastPlantIdx + 5 >= Current.Length)
            {
                bool[] toResize = Current;
                Array.Resize(ref toResize, Current.Length + 10);
                Current = toResize;
                toResize = Next;
                Array.Resize(ref toResize, Next.Length + 10);
                Next = toResize;
            }
        }

        // gets full list of pots
        private string GetPlantStr(bool[] arr)
        {
            StringBuilder sb = new StringBuilder(arr.Length);
            foreach(bool b in arr)
            {
                if (b) sb.Append('#');
                else sb.Append('.');
            }
            return sb.ToString();
        }

        // Returns the 'trimmed' string of pots containing plants
        // e.g. if input is "...#.#.....", "#.#" is returned
        private string GetTrimmedPlantStr(bool[] arr)
        {
            StringBuilder sb = new StringBuilder();
            int firstHash = 0;
            int lastHash = 0;
            for(int i = 0; i < arr.Length; i++)
            {
                if (arr[i])
                {
                    firstHash = i;
                    break;
                }
            }

            for (int i = arr.Length - 1; i > 0; i--)
            {
                if (arr[i])
                {
                    lastHash = i;
                    break;
                }
            }

            for(int i = firstHash; i <= lastHash; i++)
            {
                if (arr[i]) sb.Append('#');
                else sb.Append('.');
            }

            return sb.ToString();
        }

        [DebuggerDisplay("{PatternStr}")]
        public class Plat
        {
            public Plat(string plat)
            {
                Pattern = new bool[5];
                for (int i = 0; i < 5; i++)
                {
                    if (plat[i] == '#') Pattern[i] = true;
                }

                if (plat[9] == '#') Result = true;

                PatternStr = plat;
            }

            public bool[] Pattern { get; private set; }
            public bool Result { get; private set; }

            public string PatternStr { get; private set; }
        }

    }
}
