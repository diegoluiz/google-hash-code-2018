using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace hashcode
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args.Length >= 1 ? args[0] : "../data/";
            var files = Directory.GetFiles(path, "*.in");

            foreach (var filePath in files)
            {
                Log.Write("Processing file {0}", filePath);
                var outputFilePath = filePath.Replace(".in", ".out");

                var sliceTypes = GetSliceTypes();

                var pizza = ParseFile(filePath);
                Console.WriteLine($"pizza with {pizza.RowsCount * pizza.ColsCount} cells");
                Console.WriteLine($"MushroomCount={pizza.MushroomCount}");
                Console.WriteLine($"TomatoCount={pizza.TomatoCount}");

                pizza.Print();

                MapPizza(ref pizza, sliceTypes);

                PrintOutput(pizza, outputFilePath);
            }
        }

        private static void MapPizza(ref Pizza pizza, List<SliceType> sliceTypes)
        {
            var sliceMap = new SliceMap(pizza.RowsCount, pizza.ColsCount);
            for (var row = 0; row < pizza.RowsCount; row++)
            {
                for (var col = 0; col < pizza.ColsCount; col++)
                {
                    foreach (var sliceType in sliceTypes)
                    {
                        if (row + sliceType.RowsCount > pizza.RowsCount || col + sliceType.ColsCount > pizza.ColsCount)
                            continue;

                        var slice = BuildSlice(pizza, row, col, sliceType);

                        if (!slice.IsValidSlice())
                            continue;

                        if (sliceMap.IsSliceOverlapping(slice))
                            continue;

                        var sliceIndex = pizza.Slices.Count;
                        pizza.Slices.Add(slice);
                        sliceMap.Write(slice, sliceIndex);
                    }
                }
            }
            sliceMap.Print();
        }

        private static Slice BuildSlice(Pizza pizza, int row, int col, SliceType sliceType)
        {
            var row1 = row;
            var row2 = row + sliceType.RowsCount;
            var col1 = col;
            var col2 = col + sliceType.ColsCount;
            var components = new List<char>();

            for (var c = row1; c < row2; c++)
            {
                for (var d = col; d < col2; d++)
                {
                    components.Add(pizza.Grid[c][d]);
                }
            }

            var slice = new Slice { Row1 = row1, Row2 = row2 - 1, Col1 = col1, Col2 = col2 - 1, Components = components };
            return slice;
        }

        private static Pizza ParseFile(string filePath)
        {
            var file = File.ReadAllLines(filePath);
            var info = file.First().Split(" ");

            Context.minIngredients = int.Parse(info[2]);
            Context.maxItems = int.Parse(info[3]);

            var pizza = new Pizza(file.Skip(1).ToList());
            return pizza;
        }

        private static List<SliceType> GetSliceTypes()
        {
            var sliceTypes = new List<SliceType>();
            for (var rows = 1; rows <= Context.maxItems; rows++)
            {
                for (var cols = 1; cols <= Context.maxItems; cols++)
                {
                    if (rows * cols > 1 && rows * cols <= Context.maxItems)
                    {
                        sliceTypes.Add(new SliceType { RowsCount = rows, ColsCount = cols });
                    }
                }
            }

            sliceTypes = sliceTypes.OrderBy(x => x.ColsCount * x.RowsCount).ToList();
            return sliceTypes;
        }

        private static void PrintOutput(Pizza pizza, string filepath)
        {
            var sb = "";
            Log.Debug("--------------- OUTPUT ---------------");

            sb += pizza.Slices.Count.ToString() + Environment.NewLine;

            foreach (var slice in pizza.Slices)
            {
                sb += string.Format("{0} {1} {2} {3}", slice.Row1, slice.Col1, slice.Row2, slice.Col2) + Environment.NewLine;
            }

            Log.Debug(sb);
            Log.Debug("--------------- OUTPUT ---------------");

            File.WriteAllText(filepath, sb);
        }
    }
}
