using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ether.WeightedSelector
{
    public class SelectorOptions
    {
        /// <summary>
        /// This only impacts MultiSelect. Turning it on will allow result set to contain duplicates, otherwise
        /// we will never return the same item twice.
        /// </summary>
        public Boolean AllowDuplicates { get; set; }     

        /// <summary>
        /// If this is false and you add an item with a weight of zero or less, that will throw an exception. If it's true,
        /// the item will just be ignored (not added). This is often convenient. 
        /// </summary>
        public Boolean DropZeroWeightItems { get; set; } 

        public SelectorOptions()
        {
            AllowDuplicates = false;
            DropZeroWeightItems = true;
        }
    }
}
