using System;

namespace Ether.WeightedSelector.Algorithm
{
    internal class SingleSelector<T> : SelectorBase<T>
    {
        internal SingleSelector(WeightedSelector<T> weightedSelector) : base(weightedSelector)
        {
        }

        internal T Select()
        {
            var Items = WeightedSelector.WeightedItems;

            if (Items.Count == 0)
                throw new InvalidOperationException("There were no items to select from.");

            return ExecuteSelect(Items).Value;
        }
    }
}
