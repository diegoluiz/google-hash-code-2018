using System.Linq;

namespace hashcode
{
    public class Slice
    {
        public int Row1 { get; set; }
        public int Row2 { get; set; }
        public int Col1 { get; set; }
        public int Col2 { get; set; }
        public string Components { get; set; }

        public string Print()
        {
            return $"From row {Row1} to {Row2} and col {Col1} to {Col2}. Components: {Components}";
        }
        
        public bool IsValidSlice()
        {
            var uniqueSliceComponents = Components.ToCharArray().GroupBy(x => x);

            if (uniqueSliceComponents.Count() != 2)
            {
                Log.Write($"Slice not valid {Print()}");
                return false;
            }

            var validSlice = uniqueSliceComponents.All(x => x.Count() >= Context.minIngredients);
            Log.Write($"Slice. {Print()}");

            return validSlice;
        }
    }
}
