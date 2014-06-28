using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.WeightedSelector.Extensions
{
    public static class ExtensionMethods
    {
        public static int AverageWeight<T>(this WeightedSelector<T> selector)
        {
            return selector.WeightedItems.Count == 0 ? 0 : (int) selector.WeightedItems.Average(t => t.Weight);
        }

        public static int Count<T>(this WeightedSelector<T> selector)
        {
            return selector.WeightedItems.Count();
        }

        public static int MaxWeight<T>(this WeightedSelector<T> selector)
        {
            return selector.WeightedItems.Count == 0 ? 0 : selector.WeightedItems.Max(t => t.Weight);
        }

        public static int MinWeight<T>(this WeightedSelector<T> selector)
        {
            return selector.WeightedItems.Count == 0 ? 0 : selector.WeightedItems.Min(t => t.Weight);
        }

        public static int TotalWeight<T>(this WeightedSelector<T> selector)
        {
            return selector.WeightedItems.Count == 0 ? 0 : selector.WeightedItems.Sum(t => t.Weight);
        }

        #region "Sorting"
        public static List<WeightedItem<T>> ListByWeightDescending<T>(this WeightedSelector<T> selector)
        {
            var Result = (from Item in selector.WeightedItems
                          orderby Item.Weight descending
                          select Item).ToList();

            return Result;
        }

        public static List<WeightedItem<T>> ListByWeightAscending<T>(this WeightedSelector<T> selector)
        {
            var Result = (from Item in selector.WeightedItems
                          orderby Item.Weight ascending
                          select Item).ToList();

            return Result;
        }
        #endregion
    }
}
