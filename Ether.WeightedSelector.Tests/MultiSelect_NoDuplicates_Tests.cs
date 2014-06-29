using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ether.WeightedSelector.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ether.WeightedSelector.Tests
{
    [TestClass]
    public class MultiSelect_NoDuplicates_Tests
    {
        private const int Cycles = 100;
        private const int MinInputs = 10;
        private const int MaxInputs = 50;
        private const int MinWeight = 1;
        private const int MaxWeight = 3;
        
        [TestMethod]
        public void MultiSelect_NoDuplicates_Test()
        {
           
            for (var i = 0; i < Cycles; i++)
            {
                var Selector = BuildSelector();

                var Rng = new Random();
                var SelectionsRequested = Rng.Next(1, MinInputs); //Have to make sure we don't ask for more than we create.

                var SelectionList = Selector.SelectMultiple(SelectionsRequested);

                Assert.IsTrue(SelectionList.Count == SelectionsRequested);
                Assert.IsTrue(SelectionList.Distinct().Count() == SelectionsRequested);
            }
        }

        private WeightedSelector<string> BuildSelector()
        {
            var Selector = new WeightedSelector<string>(new SelectorOptions() { AllowDuplicates = false });
            var Inputs = InputBuilder.CreateInputs(MinInputs, MaxInputs, MinWeight, MaxWeight);
            Selector.Add(Inputs);

            return Selector; 
        } 
    }
}
