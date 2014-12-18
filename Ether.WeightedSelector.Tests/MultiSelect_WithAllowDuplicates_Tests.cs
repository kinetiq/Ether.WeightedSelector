using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ether.WeightedSelector.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ether.WeightedSelector.Tests
{
    [TestClass]
    public class MultiSelectWithAllowDuplicatesTests
    {
        //Tests multiple selection using randomized parameters. 

        private const int Cycles = 100;
        private const int MinInputs = 10;
        private const int MaxInputs = 50;
        private const int MinWeight = 1;
        private const int MaxWeight = 3;
        
        [TestMethod]
        public void MultiSelect_WithAllowDuplicates_Test()
        {
           
            for (var i = 0; i < Cycles; i++)
            {
                var Selector = BuildSelector();

                var Rnd = new Random();
                var SelectionsRequested = Rnd.Next(MaxInputs, MaxInputs * 2); //Ask for way more results than we have items,
                                                                              //to make the point that items aren't being removed as we select them.

                var SelectionList = Selector.SelectMultiple(SelectionsRequested);

                Assert.IsTrue(SelectionList.Count == SelectionsRequested); //We have as many selections as we requested.
                Assert.IsTrue(Selector.ReadOnlyItems.Count <  SelectionsRequested); //We have more selections than we have source items (due to duplicates).
            }
        }

        private WeightedSelector<string> BuildSelector()
        {
            var Selector = new WeightedSelector<string>(new SelectorOptions() { AllowDuplicates = true });
            var Inputs = InputBuilder.CreateInputs(MinInputs, MaxInputs, MinWeight, MaxWeight);
            Selector.Add(Inputs);

            return Selector; 
        } 
    }
}
