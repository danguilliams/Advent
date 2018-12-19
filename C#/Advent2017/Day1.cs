using System;

/* 
--- Day 1: Inverse Captcha ---

The night before Christmas, one of Santa's Elves calls you in a panic. "The 
printer's broken! We can't print the Naughty or Nice List!" By the time you make
it to sub-basement 17, there are only a few minutes until midnight. "We have a 
big problem," she says; "there must be almost fifty bugs in this system, but 
nothing else can print The List. Stand in this square, quick! There's no time to
explain; if you can convince them to pay you in stars, you'll be able to--" She 
pulls a lever and the world goes blurry.

When your eyes can focus again, everything seems a lot more pixelated than 
before. She must have sent you inside the computer! You check the system clock: 
25 milliseconds until midnight. With that much time, you should be able to 
collect all fifty stars by December 25th.

Collect stars by solving puzzles. Two puzzles will be made available on each day
millisecond in the advent calendar; the second puzzle is unlocked when you 
complete the first. Each puzzle grants one star. Good luck!


You're standing in a room with "digitization quarantine" written in LEDs along 
one wall. The only door is locked, but it includes a small interface. 
"Restricted Area - Strictly No Digitized Users Allowed."

It goes on to explain that you may only leave by solving a captcha to prove 
you're not a human. Apparently, you only get one millisecond to solve the 
captcha: too fast for a normal human, but it feels like hours to you.

The captcha requires you to review a sequence of digits (your puzzle input) and 
find the sum of all digits that match the next digit in the list. The list is 
circular, so the digit after the last digit is the first digit in the list.

For example:

1122 produces a sum of 3 (1 + 2) because the first digit (1) matches the second 
digit and the third digit (2) matches the fourth digit.

1111 produces 4 because each digit (all 1) matches the next.

1234 produces 0 because no digit matches the next.

91212129 produces 9 because the only digit that matches the next one is the last
digit, 9

What is the solution to your captcha?

Your puzzle answer was 1253.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

You notice a progress bar that jumps to 50% completion. Apparently, the door 
isn't yet satisfied, but it did emit a star as encouragement. The instructions 
change:

Now, instead of considering the next digit, it wants you to consider the digit 
halfway around the circular list. That is, if your list contains 10 items, only 
include a digit in your sum if the digit 10/2 = 5 steps forward matches it. 
Fortunately, your list has an even number of elements.

For example:

1212 produces 6: the list contains 4 items, and all four digits match the digit 2 items ahead.
1221 produces 0, because every comparison is between a 1 and a 2.
123425 produces 4, because both 2s match each other, but no other digit has a match.
123123 produces 12.
12131415 produces 4.
What is the solution to your new captcha?

*/
namespace AdventOfCode2017.Puzzles
{
    public class Day1 : Puzzle
    {
        public Day1() : base(1)
        {
            Input = day1_input;
        }

        private int[] Vals;


        protected override void Part1()
        {

            int sum = 0;
            // if two consecutive digits are the same, add the first to the sum
            for (int i = 0; i < Vals.Length - 1; i++)
            {
                if (Vals[i] == Vals[i + 1])
                {
                    sum += Vals[i];
                }
            }

            // special case-> if last and first digit of the array are the same, 
            // add the last digit to the sum
            if (Vals[0] == Vals[Vals.Length - 1])
            {
                sum += Vals[Vals.Length - 1];
            }

            Part1Solution = $"Anti-CAPTCHA: {sum}";
        }

        protected override void Part2()
        {
            int sum = 0;
            int l = Vals.Length;
            int h = l / 2;
            // if two consecutive digits are the same, add the first to the sum
            for (int i = 0; i < Vals.Length; i++)
            {
                int halfIdx = (i + h) % l;
                if (Vals[i] == Vals[(i + h) % l])
                {
                    sum += Vals[i];
                }
            }

            Part2Solution = $"Anti-CAPTCHA: {sum}";
        }

        protected override void ProcessInput()
        {
            Vals = new int[day1_input.Length];

            for (int i = 0; i < day1_input.Length; i++)
            {
                Vals[i] = CharToInt(day1_input[i]);
            }
        }

        private int CharToInt(char c)
        {
            switch (c)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                default: throw new Exception("BAD DIGIT");
            }

        }

        private static string day1_input = "181445682966897848665963472661939865313976877194312684993521259486517527961396717561854825453963181134379574918373213732184697746668399631642622373684425326112585283946462323363991753895647177797691214784149215198715986947573668987188746878678399624533792551651335979847131975965677957755571358934665327487287312467771187981424785514785421781781976477326712674311994735947987383516699897916595433228294198759715959469578766739518475118771755787196238772345762941477359483456641194685333528329581113788599843621326313592354167846466415943566183192946217689936174884493199368681514958669615226362538622898367728662941275658917124167353496334664239539753835439929664552886538885727235662548783529353611441231681613535447417941911479391558481443933134283852879511395429489152435996669232681215627723723565872291296878528334773391626672491878762288953597499218397146685679387438634857358552943964839321464529237533868734473777756775687759355878519113426969197211824325893376812556798483325994128743242544899625215765851923959798197562831313891371735973761384464685316273343541852758525318144681364492173465174512856618292785483181956548813344752352933634979165667651165776587656468598791994573513652324764687515345959621493346623821965554755615219855842969932269414839446887613738174567989512857785566352285988991946436148652839391593178736624957214917527759574235133666461988355855613377789115472297915429318142824465141688559333787512328799783539285826471818279818457674417354335454395644435889386297695625378256613558911695145397779576526397241795181294322797687168326696497256684943829666672341162656479563522892141714998477865114944671225898297338685958644728534192317628618817551492975251364233974374724968483637518876583946828819994321129556511537619253381981544394112184655586964655164192552352534626295996968762388827294873362719636616182786976922445125551927969267591395292198155775434997827738862786341543524544822321112131815475829945625787561369956264826651461575948462782869972654343749617939132353399334744265286151177931594514857563664329299713436914721119746932159456287267887878779218815883191236858656959258484139254446341";
    }
}
