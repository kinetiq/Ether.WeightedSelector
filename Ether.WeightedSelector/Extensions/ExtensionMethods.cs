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
            return selector.Items.Count == 0 ? 0 : (int) selector.Items.Average(t => t.Weight);
        }

        public static int Count<T>(this WeightedSelector<T> selector)
        {
            return selector.Items.Count();
        }

        public static int MaxWeight<T>(this WeightedSelector<T> selector)
        {
            return selector.Items.Count == 0 ? 0 : selector.Items.Max(t => t.Weight);
        }

        public static int MinWeight<T>(this WeightedSelector<T> selector)
        {
            return selector.Items.Count == 0 ? 0 : selector.Items.Min(t => t.Weight);
        }

        public static int TotalWeight<T>(this WeightedSelector<T> selector)
        {
            return selector.Items.Count == 0 ? 0 : selector.Items.Sum(t => t.Weight);
        }

        #region "Sorting"
        public static List<WeightedItem<T>> ListByWeightDescending<T>(this WeightedSelector<T> selector)
        {
            var Result = (from Item in selector.Items
                          orderby Item.Weight descending
                          select Item).ToList();

            return Result;
        }

        public static List<WeightedItem<T>> ListByWeightAscending<T>(this WeightedSelector<T> selector)
        {
            var Result = (from Item in selector.Items
                          orderby Item.Weight ascending
                          select Item).ToList();

            return Result;
        }
        #endregion
    }
}
