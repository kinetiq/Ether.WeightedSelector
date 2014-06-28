namespace Ether.WeightedSelector
{
    public class WeightedItem<T>
    {
        public int Weight;
        public readonly T Value;

        public WeightedItem(T value, int weight)
        {
            Value = value;
            Weight = weight;
        } 
    }
}
