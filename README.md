What is WeightedSelector.NET? 
==============

WeightedSelector.NET lets you assign weights to a set of choices, then make  decisions based on each choice's proportion of the total weight. 

This is useful for scenarios where choices need to be made based on complicated but quantifiable factors. Great examples include suggestion engines and game AI.

WeightedSelector.NET has a very simple API that makes it fun to use. In one of the examples, we implement a game AI that decides between attacking, fleeing, and casting a heal spell based on a dynamic "fear" factor... In 9 lines of code!

How can I get started?
==============

Check out the <a href="https://github.com/kinetiq/Ether.WeightedSelector/wiki/Getting-started">getting started guide</a>.

Where can I get it?
==============

First, <a href="http://docs.nuget.org/docs/start-here/installing-nuget">install NuGet</a>. Then, install WeightedSelector.NET from the package manager console:

>PM> Install-Package Ether.WeightedSelector 

WeightedSelector.NET is Copyright © 2014 Brian MacKay, <a href="getkinetiq.com">Kinetiq</a>, and other contributors under the MIT license.
