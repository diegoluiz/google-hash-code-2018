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
                // var filePath = args.Length >= 1 ? args[0] : "../data/example.in";
                var file = File.ReadAllLines(filePath);
                var info = file.First().Split(" ");
                var pizza = new Pizza(file.Skip(1).ToList());
                var sliceMap = new SliceMap(pizza.RowsCount, pizza.ColsCount);

                Context.minIngredients = int.Parse(info[2]);
                Context.maxItems = int.Parse(info[3]);
                List<SliceType> sliceTypes = GetSliceTypes();
                
                List<SliceType> sliceTypesReverse = sliceTypes.ToList();
                sliceTypesReverse.Reverse();


                Console.WriteLine($"pizza with {pizza.RowsCount * pizza.ColsCount} cells");
                Console.WriteLine($"MushroomCount={pizza.MushroomCount}");
                Console.WriteLine($"TomatoCount={pizza.TomatoCount}");

                foreach (var i in pizza.Grid)
                {
                    Log.Debug(string.Join("", i));
                }

                for (var row = 0; row < pizza.RowsCount; row++)
                {
                    for (var col = 0; col < pizza.ColsCount; col++)
                    {
                        foreach (var sliceType in sliceTypesReverse)
                        {
                            var slice = BuildSlice(sliceType, pizza, row, col);
                            if (slice == null)
                                continue;

                            if (sliceMap.IsSliceOverlapping(slice))
                                continue;

                            var sliceIndex = pizza.Slices.Count;
                            pizza.Slices.Add(slice);
                            sliceMap.Write(slice, sliceIndex);
                        }
                    }
                }

                int replacesCount;
                int iteration = 0;
                do {
                    replacesCount = 0;
                    for (var row = 0; row < pizza.RowsCount; row++)
                    {
                        for (var col = 0; col < pizza.ColsCount; col++)
                        {
                            var sliceIndex = sliceMap.map[row][col];
                            if (sliceIndex == -1)
                                continue;
                                
                            var previousSlice = pizza.Slices[sliceIndex];
                            foreach (var sliceType in sliceTypes)
                            {
                                if (sliceType.RowsCount * sliceType.ColsCount > previousSlice.Size) {
                                    var slice = BuildSlice(sliceType, pizza, row, col);
                                    if (slice != null) {
                                        if (sliceMap.IsSliceOverlappingWithIgnore(slice, sliceIndex)) {
                                            continue;
                                        }

                                        pizza.Slices[sliceIndex] = slice;

                                        sliceMap.Write(previousSlice, -1);
                                        sliceMap.Write(slice, sliceIndex);

                                        // Console.WriteLine($"Replace slice #{sliceIndex} of size {previousSlice.Size} with {slice.Size}");
                                        ++replacesCount;
                                    }
                                }
                            }
                        }
                    }
                    Console.WriteLine("Replaced " + replacesCount + " in iteration " + iteration);
                    ++iteration;
                }
                while (replacesCount > 0);

                //sliceMap.Print();
                Log.Write("Total covered: " + sliceMap.CountTotal());
                PrintOutput(pizza, outputFilePath);
            }
        }

        private static Slice BuildSlice(SliceType sliceType, Pizza pizza, int row, int col) {
            if (row + sliceType.RowsCount > pizza.RowsCount || col + sliceType.ColsCount > pizza.ColsCount)
                return null;

            var row1 = row;
            var row2 = row + sliceType.RowsCount;
            var col1 = col;
            var col2 = col + sliceType.ColsCount;
            var components = new List<char>();
            for (var c = row; c < row2; c++)
            {
                for (var d = col; d < col2; d++)
                {
                    components.Add(pizza.Grid[c][d]);
                }
            }

            var slice = new Slice
            {
                Row1 = row1,
                Row2 = row2 - 1,
                Col1 = col1,
                Col2 = col2 - 1,
                Components = components
            };

            if (!slice.IsValidSlice())
                return null;

            return slice;
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

            sliceTypes = sliceTypes.OrderByDescending(x => x.ColsCount * x.RowsCount).ToList();
            return sliceTypes;
        }

        private static void PrintOutput(Pizza pizza, string filepath)
        {
            var sb = "";
            Log.Write("--------------- OUTPUT ---------------");

            sb += pizza.Slices.Count.ToString() + Environment.NewLine;

            foreach (var slice in pizza.Slices)
            {
                sb += string.Format("{0} {1} {2} {3}", slice.Row1, slice.Col1, slice.Row2, slice.Col2) + Environment.NewLine;
            }

            //Log.Write(sb);
            Log.Write("--------------- OUTPUT ---------------");

            File.WriteAllText(filepath, sb);
        }
    }
}
