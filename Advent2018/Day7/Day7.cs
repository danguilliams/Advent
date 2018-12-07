using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    /*
     * --- Day 7: The Sum of Its Parts ---
     * You find yourself standing on a snow-covered coastline; apparently, you landed a little off course. The region is too hilly to see the North Pole from here, but you do spot some Elves that seem to be trying to unpack something that washed ashore. It's quite cold out, so you decide to risk creating a paradox by asking them for directions.
     * 
     * "Oh, are you the search party?" Somehow, you can understand whatever Elves from the year 1018 speak; you assume it's Ancient Nordic Elvish. Could the device on your wrist also be a translator? "Those clothes don't look very warm; take this." They hand you a heavy coat.
     * 
     * "We do need to find our way back to the North Pole, but we have higher priorities at the moment. You see, believe it or not, this box contains something that will solve all of Santa's transportation problems - at least, that's what it looks like from the pictures in the instructions." It doesn't seem like they can read whatever language it's in, but you can: "Sleigh kit. Some assembly required."
     * 
     * "'Sleigh'? What a wonderful name! You must help us assemble this 'sleigh' at once!" They start excitedly pulling more parts out of the box.
     * 
     * The instructions specify a series of steps and requirements about which steps must be finished before others can begin (your puzzle input). Each step is designated by a single letter. For example, suppose you have the following instructions:
     * 
     * Step C must be finished before step A can begin.
     * Step C must be finished before step F can begin.
     * Step A must be finished before step B can begin.
     * Step A must be finished before step D can begin.
     * Step B must be finished before step E can begin.
     * Step D must be finished before step E can begin.
     * Step F must be finished before step E can begin.
     * Visually, these requirements look like this:
     * 
     * 
     *   -->A--->B--
     *  /    \      \
     * C      -->D----->E
     *  \           /
     *   ---->F-----
     * Your first goal is to determine the order in which the steps should be completed. If more than one step is ready, choose the step which is first alphabetically. In this example, the steps would be completed as follows:
     * 
     * Only C is available, and so it is done first.
     * Next, both A and F are available. A is first alphabetically, so it is done next.
     * Then, even though F was available earlier, steps B and D are now also available, and B is the first alphabetically of the three.
     * After that, only D and F are available. E is not available because only some of its prerequisites are complete. Therefore, D is completed next.
     * F is the only choice, so it is done next.
     * Finally, E is completed.
     * So, in this example, the correct order is CABDFE.
     * 
     * In what order should the steps in your instructions be completed?
     */
    public class Day7 : Day
    {
        public Day7()
        {
            input = ReadInput("Day7/Day7Input.txt");
            Steps = new List<Step>();
        }

        private Step GetOrAdd(string id)
        {
            Step s = Steps.FirstOrDefault(step => step.Id.Equals(id));
            if (s == null)
            {
                s = new Step(id);
                Steps.Add(s);
            }

            return s;
        }
        
        public override int PuzzleDay => 7;

        private string[] input;

        public List<Step> Steps { get; private set; }

        protected override void ProcessInput()
        {
            foreach (string str in input)
            {
                string[] tokens = str.Split(' ');
                string previousId = tokens[1];
                string stepId = tokens[7];

                Step current = GetOrAdd(stepId);
                Step previous = GetOrAdd(previousId);

                current.PreviousSteps.Add(previous);
                previous.NextSteps.Add(current);
            }
        }

        protected override string Part1()
        {
            // get the first step
            int candidates = Steps.Count(s => s.PreviousSteps.Count == 0);

            SortedSet<Step> candidateSteps = new SortedSet<Step>(new StepComparer());
            Queue<Step> stepOrder = new Queue<Step>(Steps.Count);

            foreach(Step s in Steps.Where(s => s.PreviousSteps.Count == 0))
            {
                candidateSteps.Add(s);
            }

            Step current;
            // next step will be the first one alphabetically that hasn't already been done but has all previous steps finished
            while ((current = candidateSteps.FirstOrDefault(s => !s.Finished && s.PreviousSteps.All(st => st.Finished))) != null)
            {
                stepOrder.Enqueue(current);
                current.Finished = true;
                foreach (Step s in current.NextSteps)
                {
                    candidateSteps.Add(s);
                }
            }

            string order = string.Empty;
            foreach(Step s in stepOrder)
            {
                order += s.Id;
            }

            // incorrect: CBDEHILAFJKGMNOPQRSTUVWXYZ (was not ensuring all PreviousSteps are completed before finishing a step)

            return $"Step order: {order}";
        }

        protected override string Part2()
        {
            return "unsolved";
        }

        [DebuggerDisplay("{Id}")]
        public class Step
        {
            public Step(string id)
            {
                Id = id;
                NextSteps = new List<Step>();
                PreviousSteps = new List<Step>();
                Finished = false;
            }

            public string Id { get; private set; }
            public bool Finished { get; set; }
            public List<Step> PreviousSteps { get; private set; }
            public List<Step> NextSteps { get; private set; }
        }

        public class StepComparer : IComparer<Step>
        {
            public int Compare(Step x, Step y)
            {
                return x.Id.CompareTo(y.Id);
            }
        }
    }
}
