namespace AdventOfCode2018
{
    /*
     * 
     */
    public class Day11 : Day
    {
        public Day11()
        {

        }

        public override int PuzzleDay => 11;

        public int GridSerialNumber => 1723;

        public int GridSize = 300;

        protected override void ProcessInput()
        {

        }

        protected override string Part1()
        {
            int arrSize = GridSize + 1;
            int[,] grid = new int[arrSize, arrSize];

            for(int y = 1; y < arrSize; y++)
            {
                for (int x = 1; x < arrSize ; x++)
                {
                    grid[x, y] = GetPowerLevel(x, y);
                }
            }

            int maxPower = int.MinValue;
            int maxX = 0;
            int maxY = 0;
            for(int y = 1; y <= GridSize - 2; y++)
            {
                for(int x = 1; x <= GridSize - 2; x++)
                {
                     int power =  grid[x, y] +     grid[x + 1, y] +     grid[x + 2, y] +
                                  grid[x, y + 1] + grid[x + 1, y + 1] + grid[x + 2, y + 1] +
                                  grid[x, y + 2] + grid[x + 1, y + 2] + grid[x + 2, y + 2];

                    if(power > maxPower)
                    {
                        maxPower = power;
                        maxX = x;
                        maxY = y;
                    }
                }
            }

            // incorrect: [28,1] - was calculating cell rack id  wrong (multiplying instead of adding)

            return $"Max Power: {maxPower} at [{maxX},{maxY}]";
        }

        private int GetPowerLevel(int x, int y)
        {
            /*
             * The power level in a given fuel cell can be found through the following process:
             *
             * Find the fuel cell's rack ID, which is its X coordinate plus 10.
             * Begin with a power level of the rack ID times the Y coordinate.
             * Increase the power level by the value of the grid serial number (your puzzle input).
             * Set the power level to itself multiplied by the rack ID.
             * Keep only the hundreds digit of the power level (so 12345 becomes 3; numbers with no hundreds digit become 0).
             * Subtract 5 from the power level.
             */

            int rackId = x + 10;
            int power = rackId * y;
            power = power + GridSerialNumber;
            power = power * rackId;
            string pStr = power.ToString();
            // [1]23
            
            if(pStr.Length > 2)
            {
                power = int.Parse(pStr.Substring(pStr.Length - 3, 1));
            }
            else
            {
                power = 0;
            }

            power = power - 5;
            return power;
        }

        protected override string Part2()
        {
            return "Unfinished";
        }
    }
}
