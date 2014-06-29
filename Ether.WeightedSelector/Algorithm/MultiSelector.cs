using System;
using System.Collections.Generic;

namespace Ether.WeightedSelector.Algorithm
{
    internal class MultiSelector<T> : SelectorBase<T>
    {

        internal MultiSelector(WeightedSelector<T> weightedSelector) : base(weightedSelector)
        {
        }

        internal List<T> Select(int count)
        {
            Validate(count);

            //Create a shallow clone of the our items, because we're going to be removing 
            //items from the list in some cases, and we want to preserve the original.
            var Items = new List<WeightedItem<T>>(WeightedSelector.Items);
            var ResultList = new List<T>();

            do
            {
                WeightedItem<T> Item = null;

                if (WeightedSelector.Options.AllowDuplicates)
                    Item = ExecuteSelect(Items); //Use binary search if we can.
                else
                    //Force linear search, since AllowDuplicates currently breaks binary search.
                    Item = ExecuteSelectWithLinearSearch(Items); 


                ResultList.Add(Item.Value);

                if (!WeightedSelector.Options.AllowDuplicates)
                    Items.Remove(Item);

            } while (ResultList.Count < count);

            return ResultList;
        }

        private void Validate(int count)
        {

            if (count <= 0)
                throw new InvalidOperationException("Count must be > 0.");

            var Items = WeightedSelector.Items;

            if (Items.Count == 0)
                throw new InvalidOperationException("There were no items to select from.");

            if (!WeightedSelector.Options.AllowDuplicates && Items.Count < count)
                throw new InvalidOperationException("There aren't enough items in the collection to take " + count);
        }
    }
}
