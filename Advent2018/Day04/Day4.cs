using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    /*
     * --- Day 4: Repose Record ---
     * You've sneaked into another supply closet - this time, it's across from the prototype suit manufacturing lab. You need to sneak inside and fix the issues with the suit, but there's a guard stationed outside the lab, so this is as close as you can safely get.
     * 
     * As you search the closet for anything that might help, you discover that you're not the first person to want to sneak in. Covering the walls, someone has spent an hour starting every midnight for the past few months secretly observing this guard post! They've been writing down the ID of the one guard on duty that night - the Elves seem to have decided that one guard was enough for the overnight shift - as well as when they fall asleep or wake up while at their post (your puzzle input).
     * 
     * For example, consider the following records, which have already been organized into chronological order:
     * 
     * [1518-11-01 00:00] Guard #10 begins shift
     * [1518-11-01 00:05] falls asleep
     * [1518-11-01 00:25] wakes up
     * [1518-11-01 00:30] falls asleep
     * [1518-11-01 00:55] wakes up
     * [1518-11-01 23:58] Guard #99 begins shift
     * [1518-11-02 00:40] falls asleep
     * [1518-11-02 00:50] wakes up
     * [1518-11-03 00:05] Guard #10 begins shift
     * [1518-11-03 00:24] falls asleep
     * [1518-11-03 00:29] wakes up
     * [1518-11-04 00:02] Guard #99 begins shift
     * [1518-11-04 00:36] falls asleep
     * [1518-11-04 00:46] wakes up
     * [1518-11-05 00:03] Guard #99 begins shift
     * [1518-11-05 00:45] falls asleep
     * [1518-11-05 00:55] wakes up
     * Timestamps are written using year-month-day hour:minute format. The guard falling asleep or waking up is always the one whose shift most recently started. Because all asleep/awake times are during the midnight hour (00:00 - 00:59), only the minute portion (00 - 59) is relevant for those events.
     * 
     * Visually, these records show that the guards are asleep at these times:
     * 
     * Date   ID   Minute
     *             000000000011111111112222222222333333333344444444445555555555
     *             012345678901234567890123456789012345678901234567890123456789
     * 11-01  #10  .....####################.....#########################.....
     * 11-02  #99  ........................................##########..........
     * 11-03  #10  ........................#####...............................
     * 11-04  #99  ....................................##########..............
     * 11-05  #99  .............................................##########.....
     * The columns are Date, which shows the month-day portion of the relevant day; ID, which shows the guard on duty that day; and Minute, which shows the minutes during which the guard was asleep within the midnight hour. (The Minute column's header shows the minute's ten's digit in the first row and the one's digit in the second row.) Awake is shown as ., and asleep is shown as #.
     * 
     * Note that guards count as asleep on the minute they fall asleep, and they count as awake on the minute they wake up. For example, because Guard #10 wakes up at 00:25 on 1518-11-01, minute 25 is marked as awake.
     * 
     * If you can figure out the guard most likely to be asleep at a specific time, you might be able to trick that guard into working tonight so you can have the best chance of sneaking in. You have two strategies for choosing the best guard/minute combination.
     * 
     * Strategy 1: Find the guard that has the most minutes asleep. What minute does that guard spend asleep the most?
     * 
     * In the example above, Guard #10 spent the most minutes asleep, a total of 50 minutes (20+25+5), while Guard #99 only slept for a total of 30 minutes (10+10+10). Guard #10 was asleep most during minute 24 (on two days, whereas any other minute the guard was asleep was only seen on one day).
     * 
     * While this example listed the entries in chronological order, your entries are in the order you found them. You'll need to organize them before they can be analyzed.
     * 
     * What is the ID of the guard you chose multiplied by the minute you chose? (In the above example, the answer would be 10 * 24 = 240.)
     *      Strategy 2: Of all guards, which guard is most frequently asleep on the same minute?
     * 
     * In the example above, Guard #99 spent minute 45 asleep more than any other guard or minute - three times in total. (In all other cases, any guard spent any minute asleep at most twice.)
     * 
     * What is the ID of the guard you chose multiplied by the minute you chose? (In the above example, the answer would be 99 * 45 = 4455.)
     */
    public class Day4 : Day
    {
        public Day4()
        {
            Guards = new Dictionary<int, GuardHistory>();
        }

        public override int PuzzleDay => 4;

        private Dictionary<int, GuardHistory> Guards {get; set;}

        protected override string Part1()
        {
            // get the guard with the most sleep
            GuardHistory sleepy = Guards.Values.OrderByDescending(g => g.NapData.Sum()).First();
            int sleepiestMinute = GetSleepiestMinute(sleepy);
            int solution = sleepy.Id * sleepiestMinute;
            return $"Guard #{sleepy.Id} slept most, sleepiest minute: {sleepiestMinute} = {solution}";
        }

        protected override string Part2()
        {
            // get the guard who slept the most during any one minute
            GuardHistory sleepy = Guards.Values.OrderByDescending(g => g.NapData.Max()).First();
            int sleepiestMinute = GetSleepiestMinute(sleepy);
            int solution = sleepy.Id * sleepiestMinute;
            return $"Guard #{sleepy.Id} slept most during minute: {sleepiestMinute} = {solution}";
        }

        private int GetSleepiestMinute(GuardHistory g)
        {
            int sleepiestMinute = 0;
            int sleepiestMinuteIdx = -1;
            for (int i = 0; i < g.NapData.Length; i++)
            {
                if (g.NapData[i] > sleepiestMinute)
                {
                    sleepiestMinute = g.NapData[i];
                    sleepiestMinuteIdx = i;
                }
            }

            return sleepiestMinuteIdx;
        }

        protected override void ProcessInput()
        {
            IList<LogEntry> logs = new List<LogEntry>(Input.Length);
            foreach (string str in Input)
            {
                logs.Add(new LogEntry(str));
            }

            logs = logs.OrderBy(l => l.When).ToList();

            // if you want to see sorted input...
            //File.WriteAllLines("Day4/Day4Sorted.txt", logs.Select(l => l.Original).ToArray());
            
            int currentGuard = 0;
            int sleepIdx = 0;
            // process log entries to form schedules
            for (int i = 0; i < logs.Count; i++)
            {
                // Guard starting a new shift
                if (logs[i].What.Length > 18)
                {
                    currentGuard = int.Parse(logs[i].What.Split(' ')[1].Replace('#', ' '));
                    if (!Guards.ContainsKey(currentGuard))
                    {
                        Guards[currentGuard] = new GuardHistory(currentGuard);
                    }
                }
                else if (logs[i].What.Length > 10)// guard falling asleep
                {
                    sleepIdx = i;
                }
                else // guard waking up, process the nap
                {
                    int napStart = logs[sleepIdx].When.Minute;
                    int napEnd = logs[i].When.Minute;
                    for (int j = napStart; j < napEnd; j++)
                    {
                        Guards[currentGuard].NapData[j]++;
                    }
                }
            }
        }

        public class LogEntry
        {
            public LogEntry(string input)
            {
                // "[1518-09-09 00:04] Guard #1543 begins shift"
                When = DateTime.Parse(input.Substring(1, 16).Replace("1518", "2018"));
                What = input.Substring(18).Trim();
                Original = input;
            }

            public DateTime When { get; private set; }

            public string What { get; private set; }

            public string Original { get; private set; }
        }

        public class GuardHistory
        {
            public GuardHistory(int id)
            {
                this.Id = id;
                NapData = new int[60];
            }

            public int Id { get; set; }

            public int[] NapData { get; private set; }

        }
       
    }
}
