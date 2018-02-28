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

        // public int X1 { get; set; }
        // public int X2 { get; set; }
        // public int Y1 { get; set; }
        // public int Y2 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var filePath = args[0];
            var file = File.ReadAllLines(filePath);
            var info = file.First().Split(" ");
            var pizza = new Pizza(file.Skip(1).ToList());

            var minIngredients = int.Parse(info[2]);
            var maxItems = int.Parse(info[3]);

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

            sliceTypes.OrderByDescending(x => x.ColsCount * x.RowsCount);

            foreach (var i in pizza.Grid)
            {
                Console.WriteLine(string.Join("", i));
            }

            foreach (var i in sliceTypes)
            {
                Console.WriteLine($"{i.RowsCount} - {i.ColsCount}");
            }

            for (var row = 0; row < pizza.Grid.Count; row++)
            {
                for (var col = 0; col < row; col++)
                {
                    foreach (var sliceType in sliceTypes)
                    {
                        if (row + sliceType.RowsCount > pizza.RowsCount || col + sliceType.ColsCount > pizza.ColsCount)
                        {
                            continue;
                        }

                        if (!IsValidSlice(row, col, pizza.Grid, minIngredients))
                        {

                        }

                    }
                }
            }
        }

        private static bool IsValidSlice(int row, int col, List<List<string>> grid, int minIngredients)
        {
            throw new NotImplementedException();
        }
    }
}
