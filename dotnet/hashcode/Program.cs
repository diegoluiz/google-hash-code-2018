using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace hashcode
{
    public class Pizza
    {
        public int RowsCount { get; private set; }
        public int ColsCount { get; private set; }

        public List<List<string>> Grid { get; }
        public List<Slice> Slices { get; } = new List<Slice>();

        public Pizza(List<string> data)
        {
            RowsCount = data.Count;
            ColsCount = data[0].Length;

            Grid = data.Select(l =>
            {
                return new List<string>(l.Split(""));
            }).ToList();
        }
    }

    public class SliceType
    {
        public int RowsCount { get; set; }
        public int ColsCount { get; set; }

    }

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
    }

    class Program
    {
        private static int minIngredients;
        private static int maxItems;

        static void Main(string[] args)
        {
            var filePath = args.Length >= 1 ? args[0] : "../data/example.in";
            var file = File.ReadAllLines(filePath);
            var info = file.First().Split(" ");
            var pizza = new Pizza(file.Skip(1).ToList());

            minIngredients = int.Parse(info[2]);
            maxItems = int.Parse(info[3]);

            var sliceTypes = new List<SliceType>();
            for (var rows = 1; rows <= maxItems; rows++)
            {
                for (var cols = 1; cols <= maxItems; cols++)
                {
                    if (rows * cols > 1 && rows * cols <= maxItems)
                    {
                        sliceTypes.Add(new SliceType { RowsCount = rows, ColsCount = cols });
                    }
                }
            }

            sliceTypes = sliceTypes.OrderByDescending(x => x.ColsCount * x.RowsCount).ToList();

            foreach (var i in pizza.Grid)
            {
                Console.WriteLine(string.Join("", i));
            }

            // foreach (var i in sliceTypes)
            // {
            //     Console.WriteLine($"{i.RowsCount} - {i.ColsCount}");
            // }

            for (var row = 0; row < pizza.RowsCount; row++)
            {
                for (var col = 0; col < pizza.ColsCount; col++)
                {
                    foreach (var sliceType in sliceTypes)
                    {
                        if (row + sliceType.RowsCount > pizza.RowsCount || col + sliceType.ColsCount > pizza.ColsCount)
                            continue;

                        var row1 = row;
                        var row2 = row + sliceType.RowsCount;
                        var col1 = col;
                        var col2 = col + sliceType.ColsCount;

                        var a = "";
                        for (var c = row; c < row2; c++)
                        {
var b = pizza.Grid[c];
                        }
                        var slice = new Slice
                        {
                            Row1 = row1,
                            Row2 = row2 - 1,
                            Col1 = col1,
                            Col2 = col2 - 1,
                            Components = pizza.Grid.Skip(row)
                                            .Take(row2 - row1)
                                            .SelectMany(x => x.Skip(col1).Take(col2 - col1))
                                            .Aggregate((cur, next) => cur + next)
                        };

                        if (!IsValidSlice(slice))
                            continue;

                        if (IsSliceOverLapping(pizza.Grid, slice))
                            continue;

                        pizza.Slices.Add(slice);
                    }
                }
            }
        }

        private static bool IsValidSlice(Slice slice)
        {
            var uniqueSliceComponents = slice.Components.ToCharArray().GroupBy(x => x);

            if (uniqueSliceComponents.Count() != 2)
            {
                Console.WriteLine($"Slice not valid {slice.Print()}");
                return false;
            }

            var validSlice = uniqueSliceComponents.All(x => x.Count() >= minIngredients);
            Console.WriteLine($"Slice. {slice.Print()}");

            return validSlice;
        }

        private static bool IsSliceOverLapping(List<List<string>> grid, Slice slice)
        {
            throw new NotImplementedException();
        }
    }
}
