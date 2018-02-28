using System.Collections.Generic;
using System.Linq;

namespace hashcode
{
    public class Pizza
    {
        public int RowsCount { get; private set; }
        public int ColsCount { get; private set; }

        public List<List<char>> Grid { get; }
        public List<Slice> Slices { get; } = new List<Slice>();

        public Pizza(List<string> data)
        {
            RowsCount = data.Count;
            ColsCount = data[0].Length;

            Grid = new List<List<char>>();

            foreach (var line in data)
            {
                var components = line.ToCharArray().ToList();
                Grid.Add(components);
            }
        }
    }
}
