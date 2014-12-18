using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ether.WeightedSelector.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ether.WeightedSelector.Tests
{
    [TestClass]
    public class NinjaStringTests
    {
        
        [TestMethod]
        public void NinjaFearLevelStringTests()
        {
            //Create our selector, indicating that we will be choosing between a set of strings.
            //This is fully generic and could be any type. 
            var Selector = new WeightedSelector<string>();

            var NinjaFearLevel = 5; //A value from 0 to 10, where 10 is the most afraid. 
                                    //As fear approaches 10, the monster is more likely to run.

            //Next we add our choices. The first parameter is the choice, the second is the weight.  
            Selector.Add("Cast Heal", NinjaFearLevel);
            Selector.Add("Flee", NinjaFearLevel - 7); //Ninjas fight to the death... Usually.
            Selector.Add("Attack", 10 - NinjaFearLevel);
               
            //So, if fear is 0, ninja will not cast heal (0) and will not flee (-7), he will always attack (10). 
            //If fear is 5, ninja might cast heal (5/50%) and will never flee (-2). He might attack (5/50%). 
            //If fear is 10, ninja will probably cast heal (10/76%) and might flee (3/23%). He's too afraid to attack (0/0%). 
            
            //This is where the magic happens. NinjaAction will be one of the choices we entered above.
            string NinjaAction = Selector.Select();
            
            //This test is mostly for documentation, however this does have to be true:
            Assert.IsTrue(NinjaAction == "Cast Heal" || 
                          NinjaAction == "Flee" || 
                          NinjaAction == "Attack");
        }

      
    }

}
