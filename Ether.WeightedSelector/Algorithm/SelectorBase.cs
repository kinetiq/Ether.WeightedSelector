using System;
using System.Collections.Generic;
using System.Linq;
using Ether.WeightedSelector.Extensions;

namespace Ether.WeightedSelector.Algorithm
{
    internal abstract class SelectorBase<T>        
    {
        protected readonly WeightedSelector<T> WeightedSelector;
        private readonly Random Rng;

        internal SelectorBase(WeightedSelector<T> weightedSelector)
        {
            WeightedSelector = weightedSelector;
            Rng = new Random();
        }

        protected int GetSeed(List<WeightedItem<T>> items)
        {
            var TopRange = items.Sum(i => i.Weight) + 1;
            return Rng.Next(1, TopRange);
        }

        /// <summary>
        /// Execute selection using the binary search algorithm, which is the fastest way.
        /// </summary>
        protected WeightedItem<T> ExecuteSelect(List<WeightedItem<T>> items)
        {      
            if (items.Count == 0)
                throw new InvalidOperationException("Tried to do a select, but WeightedItems was emtpy.");

            //Choose an item based on each item's proportion of the total weight.
            var Seed = GetSeed(items);

            return BinarySearch(items, Seed);
        }

        /// <summary>
        /// Select, and force the slower Linear Search algorithm.
        /// </summary>
        protected WeightedItem<T> ExecuteSelectWithLinearSearch(List<WeightedItem<T>> items)
        {
            //Note that this is only really useful for multiselect with !allowduplicates, which removes
            //items from the list as it goes. Just haven't put the effort into getting binary search to work
            //in those conditions.

            if (items.Count == 0)
                throw new InvalidOperationException("Tried to do a select, but WeightedItems was emtpy.");

            //Choose an item based on each item's proportion of the total weight.
            var Seed = GetSeed(items);

            return LinearSearch(items, Seed);
        }


        private WeightedItem<T> LinearSearch(IEnumerable<WeightedItem<T>> items, int seed)
        {
            var RunningCount = 0;
            
            foreach (var Item in items)
            {
                RunningCount += Item.Weight;

                if (seed <= RunningCount)
                    return Item;
            }

            throw new InvalidOperationException("There was no result during SimpleSearch. This should never happen.");
        }

        private WeightedItem<T> BinarySearch(List<WeightedItem<T>> items, int seed)
        {
            int Index = Array.BinarySearch(WeightedSelector.CumulativeWeights, seed);

            //If there's a near match, IE our array is (1, 5, 9) and we search for 3, BinarySearch
            //returns a negative number that is one less than the first index great than our search.
            if (Index < 0)
                Index = (Index*-1) - 1; 

            return items[Index];
        }
    }
}
