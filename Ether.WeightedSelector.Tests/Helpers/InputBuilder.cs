using System;
using System.Collections.Generic;

namespace Ether.WeightedSelector.Tests.Helpers
{
    internal class InputBuilder
    {
       public static List<WeightedItem<string>> CreateInputs(int minInputs, int maxInputs, int minWeight, int maxWeight)
        {
            var Gen = new Random();
            var InputCount = Gen.Next(minInputs, maxInputs);
            var Result = new List<WeightedItem<String>>();

            for (var i = 1; i <= InputCount; i++)
            {
                var Item = new WeightedItem<string>(GetInputName(), 
                                                    Gen.Next(minWeight, maxWeight));
                Result.Add(Item);
            }

            return Result;
        }

        private static string GetInputName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
