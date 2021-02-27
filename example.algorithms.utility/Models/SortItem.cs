namespace Examples.Algorithms.Utility.Models
{
    public class SortItem
    {
        public int Value { get; set; }
        public bool Pivot { get; set; }
        public bool BelowOrAtPivot { get; set; }
        public bool NextSwap { get; set; }
        public bool Swapped { get; set; }
        
    }
}