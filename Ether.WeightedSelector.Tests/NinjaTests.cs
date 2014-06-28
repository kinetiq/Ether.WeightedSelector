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
    public class NinjaTests
    {
        
        [TestMethod]
        public void NinjaFearLevelTests()
        {
            var Selector = new WeightedSelector<MonsterAction>();

            var NinjaFearLevel = 5; //A value from 0 to 10, where 10 is the most afraid. 
                                    //As fear approaches 10, the monster is more likely to run.
           
            var ActionCandidates = new List<WeightedItem<MonsterAction>>()
            {
                new WeightedItem<MonsterAction>(new MonsterAction("Cast Heal"), NinjaFearLevel), 
                new WeightedItem<MonsterAction>(new MonsterAction("Flee"), NinjaFearLevel - 7),  //Ninjas fight to the death... Usually.
                new WeightedItem<MonsterAction>(new MonsterAction("Attack"), 10 - NinjaFearLevel)     
            };

            //So, if fear is 0, ninja will not cast heal (0) and will not flee (-7), he will always attack (10). 
            //If fear is 5, ninja might cast heal (5/50%) and will never flee (-2). He might attack (5/50%). 
            //If fear is 10, ninja will probably cast heal (10/76%) and might flee (3/23%). He's too afraid to attack (0/0%). 
            
            Selector.Add(ActionCandidates);

            var SelectedAction = Selector.Select();

            Assert.IsTrue(SelectedAction.Name == "Cast Heal" || 
                          SelectedAction.Name == "Flee" || 
                          SelectedAction.Name == "Attack");
        }

      
    }

    public class MonsterAction
    {
        public string Name = string.Empty;

        public MonsterAction(string name)
        {
            this.Name = name;
        }
    }

}
