using System;
using System.Linq;

namespace RoomGen
{
    public static class Util
    {
        public static readonly Random RandomGenerator = new Random();
        
        /// <summary>
        /// GenerateLevel <see cref="size"/> distinct random numbers
        /// </summary>
        public static int[] GetRandomDistinctNumbers(int size)
        {
            var nums = Enumerable.Range(0, size).ToArray();
            

            // Shuffle the array
            for (var i = 0; i < nums.Length; ++i)
            {
                var randomIndex = RandomGenerator.Next(nums.Length);
                var temp = nums[randomIndex];
                nums[randomIndex] = nums[i];
                nums[i] = temp;
            }
            return nums;
        }
    }
}
