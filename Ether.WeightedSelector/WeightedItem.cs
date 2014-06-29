namespace Ether.WeightedSelector
{
    public class WeightedItem<T>
    {
        public int Weight;
        public readonly T Value;
        internal int CumulativeWeight; //used for binary chop/search.

        public WeightedItem(T value, int weight)
        {
            Value = value;
            Weight = weight;
            CumulativeWeight = 0;
        } 
    }
}
