using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ether.WeightedSelector.Algorithm
{
    public class BinarySearchOptimizer
    {
        public static int[] GetCumulativeWeights<T>(List<WeightedItem<T>> items)
        {
            //For binary search, we do some setup ahead of time here... We need to build an array of 
            //cumulative weights. So if our items had weights of: 3, 5, 3, 2 the array would be: 3, 8, 11, 13

            int RunningWeight = 0;
            int Index = 0;
            var ResultArray = new int[items.Count() + 1];

            foreach (var Item in items)
            {
                RunningWeight += Item.Weight;
                ResultArray[Index] = RunningWeight;

                Index ++;
            }

            return ResultArray;
        }
    }
}
