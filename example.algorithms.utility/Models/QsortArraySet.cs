using System.Collections.Generic;

namespace Examples.Algorithms.Utility.Models
{
    public class QsortArraySet
    {
        public int[] InitialArray {get; set;}
        public int[] EndingArray {get; set;}
        public List<ArrayPivotSet> Pivots {get; set;} = new List<ArrayPivotSet>();
    }
}