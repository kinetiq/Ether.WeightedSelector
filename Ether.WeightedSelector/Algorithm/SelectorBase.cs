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

        protected WeightedItem<T> ExecuteSelect(List<WeightedItem<T>> items)
        {
            //Choose an item based on each item's proportion of the total weight.
            var Seed = GetSeed(items);
            var RunningCount = 0;

            foreach (var Item in items)
            {
                RunningCount += Item.Weight;

                if (Seed <= RunningCount)
                    return Item;
            }

            throw new InvalidOperationException("Nothing was returned. This should never happen.");
        }
    }
}
