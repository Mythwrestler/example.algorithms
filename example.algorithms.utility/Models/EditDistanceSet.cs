using System.Collections.Generic;

namespace Examples.Algorithms.Utility.Models
{
    public class EditDistanceSet
    {
        public string String01 { get; set;}
        public string String02 { get; set;}
        public bool IsCaseInsensitive { get; set; }
        public int[][] EditDistanceMatrix { get; set; }
        public int EditDistance { get; set; }
    }
}