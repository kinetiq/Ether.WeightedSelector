using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ether.WeightedSelector.Extensions;
using Ether.WeightedSelector.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ether.WeightedSelector.Tests
{
    [TestClass]
    public class ProbabilityExtremeInputsTests
    {
        //This test runs a million trials with randomized parameters, and tests to make sure the 
        //distribution of selections is somewhat close to the weight proportions. For instance,
        //if one item's weight makes up 33% of a test's total weight, after a million tests it
        //should be selected about 33% of the time. 
        
        private const int Trials = 50000; //Fewer tests on this one; extreme inputs is time consuming.

        //AcceptableDeviation gives us some margin for small statistical anomolies. For instance,
        //given the example above, maybe the item is selected 34% or even 35% of the time. That's an acceptable
        //abnormality. We could close the gap by running even more tests.
        private const int AcceptableDeviation = 4; 

        //Range of weighted items going in.
        private const int MinInputs = 2000;    
        private const int MaxInputs = 3000;

        //Range of weights each item can have. 
        private const int MinWeight = 3;
        private const int MaxWeight = 6;
        
        [TestMethod]
        public void Probability_ExtremeInputs_Test()
        {
            var Inputs = InputBuilder.CreateInputs(MinInputs, MaxInputs, MinWeight, MaxWeight);
            var Selector = new WeightedSelector<string>();
            Selector.Add(Inputs);

            var Helper = new ProbabilityHelpers(Selector, Inputs, Trials, AcceptableDeviation);

            Console.WriteLine("Running {0} trials with {1} items (total weight: {2})", 
                              Trials,
                              Selector.ReadOnlyItems.Count,
                              Selector.TotalWeight());

            var ResultCounter = Helper.RunTrialsAndCountResults();

            foreach (var Key in ResultCounter.Keys)
                Helper.ExamineMetricsForKey(Key);

            //Note that in this test, a ton of items will never be selected because there are so many.
            //If we did tens of millions of trials that would counter it, but it might take 30s+, which is 
            //unecessary.
            Console.WriteLine("Expected {0} outputs, actual: {1}. Details: {2}",
                              Inputs.Count,                                                                                  
                              ResultCounter.Keys.Count,                                                                                         
                              Helper.GetErrorMessage());
        } 
    }
}
