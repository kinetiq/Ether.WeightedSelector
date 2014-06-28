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
    public class ProbabilityTests
    {
        //This test runs a million trials with randomized parameters, and tests to make sure the 
        //distribution of selections is somewhat close to the weight proportions. For instance,
        //if one item's weight makes up 33% of a test's total weight, after a million tests it
        //should be selected about 33% of the time. 
        
        private const int Trials = 1000000; //We do a million tests. 

        //Percent acceptable difference between proportion of total weight and actual density. For instance,
        //given the example above, maybe the item is selected 34% or even 35% of the time. That's an acceptable
        //abnormality.
        private const int AcceptableDeviation = 2; 

        //Range of weighted items going in.
        private const int MinInputs = 5;           
        private const int MaxInputs = 10;

        //Range of weights each item can have. 
        private const int MinWeight = 1;
        private const int MaxWeight = 30;
        
        [TestMethod]
        public void ProbabilityTest()
        {
            var Inputs = InputBuilder.CreateInputs(MinInputs, MaxInputs, MinWeight, MaxWeight);
            var Selector = new WeightedSelector<string>();
            Selector.Add(Inputs);

            Console.WriteLine("Running {0} trials with {1} items (total weight: {2})", 
                              Trials,
                              Selector.Items.Count,
                              Selector.TotalWeight());

            var ResultCounter = RunTrialsAndCountResults(Selector, Trials);

            foreach (var Key in ResultCounter.Keys)
                ExamineMetricsForKey(Key, ResultCounter, Selector);

            Assert.IsTrue(ResultCounter.Keys.Count == Inputs.Count,
                          string.Format("Expected {0} outputs, actual: {1}. Details: {2}", 
                                        Inputs.Count,                                                                                  
                                        ResultCounter.Keys.Count,                                                                                         
                                        GetErrorMessage(ResultCounter, Inputs)));
        } 


        private string GetErrorMessage(Dictionary<string, int> resultCounter, IEnumerable<WeightedItem<string>> inputs)
        {
            //If an item didn't generate any hits at all, it won't show up in ResultCounter. This grabs some details. 
            var Builder = new StringBuilder();

            foreach (var Key in inputs)
            {
                if (!resultCounter.ContainsKey(Key.Value))
                    Builder.AppendLine(string.Format("Missing {0}, Weight: {1}", Key.Value, Key.Weight));
            }

            return Builder.ToString();
        }

        private void ExamineMetricsForKey(string key, Dictionary<string, int> resultCounter, WeightedSelector<string> selector)
        {
            decimal WeightProportion = GetWeightProportion(key, resultCounter);
            decimal SelectionProportion = GetSelectionProportion(key, selector);

            Console.WriteLine("{0}", key);
            Console.WriteLine("     Hits: {0} ({3}% of total)     Weight {1} ({2}% of total)", 
                                resultCounter[key],
                                GetWeight(key, selector),
                                Math.Round(WeightProportion, 3), 
                                Math.Round(SelectionProportion, 3));

            Assert.IsTrue(WeightProportion >= (SelectionProportion - AcceptableDeviation) && 
                          WeightProportion <= (SelectionProportion + AcceptableDeviation),
                          string.Format("Expected between {0}% and {1}%. Actual: {2}%", SelectionProportion - AcceptableDeviation, 
                                                                                        SelectionProportion + AcceptableDeviation, 
                                                                                        WeightProportion));             
        }

        private Dictionary<string, int> RunTrialsAndCountResults(WeightedSelector<string> selector, int cycles)
        {
            //Do [trials] selections, and dump the number of hits for each item into a dictionary.
            var ResultCounter = new Dictionary<string, int>();
            
            for (var i = 0; i < cycles; i++)
            {
                string Decision = selector.Select();

                if (!ResultCounter.ContainsKey(Decision))
                    ResultCounter[Decision] = 1;
                else
                    ResultCounter[Decision] += 1;
            }

            return ResultCounter;
        }

        private decimal GetWeightProportion(string key, Dictionary<string, int> resultCounter)
        {
            //What % of the total weight does this key's weight represent? 
            return ((decimal)resultCounter[key] / Trials) * 100;            
        }

        private decimal GetSelectionProportion(string key, WeightedSelector<string> selector)
        {
            //Over all of our tests, how many times did we select this key? 
            var Total = selector.TotalWeight();

            var Item = (from WeightedItem<string> W in selector.Items 
                        where W.Value == key 
                        select W).First();
        
            return ((decimal) Item.Weight / Total) * 100;
        }


        private decimal GetWeight(string key, WeightedSelector<string> selector)
        {
            var Item = (from WeightedItem<string> W in selector.Items
                        where W.Value == key
                        select W).First();

            return Item.Weight;
        }
    }
}
