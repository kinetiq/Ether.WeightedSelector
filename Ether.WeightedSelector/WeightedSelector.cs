using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Ether.WeightedSelector.Algorithm;

namespace Ether.WeightedSelector
{
    public class WeightedSelector<T> 
    {
        internal readonly List<WeightedItem<T>> Items = new List<WeightedItem<T>>();
        public readonly SelectorOptions Options;  

        internal int[] CumulativeWeights = null;   //used for binary search. 
        private Boolean IsCumulativeWeightsStale;  //forces recalc of CumulativeWeights any time our list of WeightedItems changes.
                                       
        public WeightedSelector(SelectorOptions options = null)
        {
            if (options == null)
                options = new SelectorOptions();

            this.Options = options; 
            IsCumulativeWeightsStale = false;
        }

        #region "Add/Remove"
        public void Add(WeightedItem<T> item)
        {
            if (item.Weight <= 0)
            {               
                if (Options.DropZeroWeightItems)
                    return; //"drop" the item, that is don't add it.
                else
                    throw new InvalidOperationException("Scores must be => 0.");
            }

            IsCumulativeWeightsStale = true;
            Items.Add(item);    
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

        public void Remove(WeightedItem<T> item)
        {
            IsCumulativeWeightsStale = true;
            Items.Remove(item);
        }
        #endregion

        #region "Selection API"

        /// <summary>
        /// Execute the selection algorithm, returning one result.
        /// </summary>
        public T Select()
        {
            CalculateCumulativeWeights();

            var Selector = new SingleSelector<T>(this);
            return Selector.Select();
        }

        /// <summary>
        /// Execute the selection algorithm, returning multiple results.
        /// </summary>
        public List<T> SelectMultiple(int count)
        {
            CalculateCumulativeWeights();

            var Selector = new MultiSelector<T>(this);
            return Selector.Select(count);
        }

        private void CalculateCumulativeWeights()
        {
            if (!IsCumulativeWeightsStale) //If it's not stale, we can skip this! 
                return;

            IsCumulativeWeightsStale = false; 
            CumulativeWeights = BinarySearchOptimizer.GetCumulativeWeights(Items);  
        }
        #endregion

        #region "Public Properties"

        /// <summary>
        /// Read-only collection of WeightedItems.
        /// </summary>
        public ReadOnlyCollection<WeightedItem<T>> ReadOnlyItems
        {
            get { return new ReadOnlyCollection<WeightedItem<T>>(this.Items); }
        }

        #endregion
    }
}
