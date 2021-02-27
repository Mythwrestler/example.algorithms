namespace Examples.Algorithms.Utility.Models
{
    public class ArrayPivotSet
    {
        public int Iteration { get; set; }
        public int RecursionLevel { get; set; }
        public int ParentIteration { get; set; }
        public int[] WorkingArray { get; set; }
        public int WorkingArrayPosition { get; set; }
        public int ArrayLength { get; set; }
        public int[] StartingArray { get; set; }
        public bool IsBaseCase { get; set; }
        public int[] EndingArray { get; set; }
        public int EndingArrayPivotIndex { get; set; }
        public int LeftSplitStart { get; set; }
        public int LeftSplitLength { get; set; }
        public int[] LeftSplit { get; set; }
        public int RightSplitStart { get; set; }
        public int RightSplitLength { get; set; }
        public int[] RightSplit { get; set; }
        public int PivotValue { get; set; }
        public int PivotIndex { get; set; }
        public ItemArray[] WorkingArrays { get; set; }

    }
}