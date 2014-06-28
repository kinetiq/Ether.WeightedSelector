using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ether.WeightedSelector.Algorithm;

namespace Ether.WeightedSelector
{
    public class WeightedSelector<T> 
    {
        internal readonly List<WeightedItem<T>> WeightedItems = new List<WeightedItem<T>>();
        public Boolean AllowDuplicates { get; set; }
        public Boolean DropZeroWeightItems { get; set; }

        public WeightedSelector()
        {
            AllowDuplicates = false;
            DropZeroWeightItems = true;
        }

        #region "Add"
        public void Add(WeightedItem<T> item)
        {
            if (item.Weight <= 0)
            {               
                if (DropZeroWeightItems)
                    return; //"drop" the item, that is don't add it.
                else
                    throw new InvalidOperationException("Scores must be => 0.");
            }

            WeightedItems.Add(item);    
        }
       
        public void Add(IEnumerable<WeightedItem<T>> items)
        {
            foreach (var Item in items)
            {                
                this.Add(Item);
            }
        }

        public void Add(T item, int weight)
        {
            this.Add(new WeightedItem<T>(item, weight));
        }
        #endregion

        #region "Selection"

        /// <summary>
        /// Execute the selection algorithm.
        /// </summary>
        public T Select()
        {
            var Selector = new SingleSelector<T>(this);
            return Selector.Select();
        }

        /// <summary>
        /// Execute the selection algorithm, returning multiple results.
        /// </summary>
        public List<T> SelectMultiple(int count)
        {
            var Selector = new MultiSelector<T>(this);
            return Selector.Select(count);
        }
        #endregion

        #region "Public Properties"

        /// <summary>
        /// Read-only collection of WeightedItems.
        /// </summary>
        public ReadOnlyCollection<WeightedItem<T>> Items
        {
            get { return new ReadOnlyCollection<WeightedItem<T>>(this.WeightedItems); }
        }

        #endregion
    }
}
