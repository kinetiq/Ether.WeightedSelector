using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ether.WeightedSelector.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ether.WeightedSelector.Tests
{
    [TestClass]
    public class ReUseTests
    {

        [TestMethod]
        public void ReUseTest()
        {
            var Selector = new WeightedSelector<int>();
            Selector.Add(1, 1);
            int Result1 = Selector.Select();

            //There's only one choice - 1. It will always return.
            Assert.IsTrue(Result1 == 1);

            //Now re-use the same selector, but put an item in with so much weight that it will 
            //always "win". 
            Selector.Add(2, 5000000); //That's a heavy item.
            int Result2 = Selector.Select();

            Assert.IsTrue(Selector.ReadOnlyItems.Count == 2);
            Assert.IsTrue(Result2 == 2);
        }
    }
}
