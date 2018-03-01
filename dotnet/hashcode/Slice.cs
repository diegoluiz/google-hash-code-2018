using System.Linq;
using System.Collections.Generic;

namespace hashcode
{
    public class Slice
    {
        public Slice()
        {
            Components = new List<char>();
        }

        public int Row1 { get; set; }
        public int Row2 { get; set; }
        public int Col1 { get; set; }
        public int Col2 { get; set; }
        public List<char> Components { get; set; }

        public int Size => (Row2 - Row1 + 1) * (Col2 - Col1 + 1);

        public string Print()
        {
            return $"From row {Row1} to {Row2} and col {Col1} to {Col2}. Area {Size}. Components: {string.Join("", Components)}";
        }
        
        public bool IsValidSlice()
        {
            var uniqueSliceComponents = Components.GroupBy(x => x);

            if (uniqueSliceComponents.Count() != 2)
                return false;
                
            var validSlice = uniqueSliceComponents.All(x => x.Count() >= Context.minIngredients);
            return validSlice;
        }
    }
}
