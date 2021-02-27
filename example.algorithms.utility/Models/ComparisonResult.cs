using System.Collections.Generic;

namespace Examples.Algorithms.Utility.Models
{
    public class ComparisonResult
    {
        public int EditValue { get; set; } = 0;
        public int EditTotal { get; set; } = 0;
        public EditType EditType { get; set; } = EditType.None;
        public int[] ReferenceCell { get; set; } = new int[]{};
    }

    public enum EditType
    {
        None,
        Addition,
        Deletion,
        Substitution,
        Transposition
    }

}