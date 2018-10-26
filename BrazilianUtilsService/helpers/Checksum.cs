using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BrazilianUtilsService.helpers
{   public static class Checksum
    {
        private static int CreateCheckSum(string[] start, List<int> weights) 
        {
            int sum = 0;
			for(int index = 0; index < start.Length; index ++)        
               sum += int.Parse(start[index]) * weights[index];
            return sum;
        }

        public static int GenerateChecksum(string[] value, List<int> weights) 
        {
            return CreateCheckSum(value, weights);
        }

        public static int GenerateChecksum(string[] value, int weights) 
        {
            int[] weightsArray = new int[value.Length];
            for (int i = 0; i < value.Length; i++)
                weightsArray[i] = weights - i;

            return CreateCheckSum(value, weightsArray.ToList());
        }
    }
}