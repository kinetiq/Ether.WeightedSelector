using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ether.WeightedSelector.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ether.WeightedSelector.Tests.Helpers
{
    class ProbabilityHelpers
    {
        private Dictionary<string, int> ResultCounter;
        private readonly List<WeightedItem<string>> Inputs;
        private readonly WeightedSelector<string> Selector;
        private readonly int Trials;
        private readonly int AcceptableDeviation;

        public ProbabilityHelpers(WeightedSelector<string> selector, 
                                  List<WeightedItem<string>> inputs, 
                                  int trials, 
                                  int acceptableDeviation)
        {
            this.Selector = selector;
            this.Inputs = inputs;
            this.Trials = trials;
            this.AcceptableDeviation = acceptableDeviation;
        }

        public Dictionary<string, int> RunTrialsAndCountResults()
        {
            //Do [trials] selections, and dump the number of hits for each item into a dictionary.
            var Results = new Dictionary<string, int>();

            for (var i = 0; i < Trials; i++)
            {
                string Decision = Selector.Select();

                if (!Results.ContainsKey(Decision))
                    Results[Decision] = 1;
                else
                    Results[Decision] += 1;
            }

            this.ResultCounter = Results;

            return Results;
        }

        public void ExamineMetricsForKey(string key)
        {
            decimal WeightProportion = GetWeightProportion(key);
            decimal SelectionProportion = GetSelectionProportion(key);

            Console.WriteLine("{0}", key);
            Console.WriteLine("     Hits: {0} ({3}% of total)     Weight {1} ({2}% of total)",
                                ResultCounter[key],
                                GetWeight(key),
                                Math.Round(WeightProportion, 3),
                                Math.Round(SelectionProportion, 3));

            Assert.IsTrue(WeightProportion >= (SelectionProportion - AcceptableDeviation) &&
                          WeightProportion <= (SelectionProportion + AcceptableDeviation),
                          string.Format("Expected between {0}% and {1}%. Actual: {2}%", SelectionProportion - AcceptableDeviation,
                                                                                        SelectionProportion + AcceptableDeviation,
                                                                                        WeightProportion));
        }

        private decimal GetWeightProportion(string key)
        {
            //What % of the total weight does this key's weight represent? 
            return ((decimal)ResultCounter[key] / Trials) * 100;
        }

        private decimal GetSelectionProportion(string key)
        {
            //Over all of our tests, how many times did we select this key? 
            var Total = Selector.TotalWeight();

            var Item = (from WeightedItem<string> W in Selector.ReadOnlyItems
                        where W.Value == key
                        select W).First();

            return ((decimal)Item.Weight / Total) * 100;
        }

        private decimal GetWeight(string key)
        {
            var Item = (from WeightedItem<string> W in Selector.ReadOnlyItems
                        where W.Value == key
                        select W).First();

            return Item.Weight;
        }


        public string GetErrorMessage()
        {
            //If an item didn't generate any hits at all, it won't show up in ResultCounter. This grabs some details. 
            var Builder = new StringBuilder();

            foreach (var Key in Inputs)
            {
                if (!ResultCounter.ContainsKey(Key.Value))
                    Builder.AppendLine(string.Format("Missing {0}, Weight: {1}", Key.Value, Key.Weight));
            }

            return Builder.ToString();
        }


    }
}
