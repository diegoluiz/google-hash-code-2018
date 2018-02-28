using System;

namespace hashcode
{
    public class SliceMap {
        public int[][] map;

        public SliceMap(int rows, int columns) {
            map = new int[rows][];
            for (int i = 0; i < columns; ++i) {
                map[i] = new int[columns];
                Array.Fill(map[i], -1);
            }
        }

        public void Write(Slice slice, int sliceIndex) {
            this.Write(slice.Row1, slice.Col1, slice.Row2, slice.Col2, sliceIndex);
        }

        public void Write(int row1, int col1, int row2, int col2, int sliceIndex) {
            for (var y = row1; y <= row2; ++y) {
                for (var x = col1; x <= col2; ++x) {
                    map[y][x] = sliceIndex;
                }
            }
        }

        public bool IsBusy(int row, int col) {
            return map[row][col] != -1;
        }

        public bool IsSliceOverlapping(Slice slice) {
           return IsSliceOverlapping(slice.Row1, slice.Col1, slice.Row2, slice.Col2);
        }

        public bool IsSliceOverlapping(int row1, int col1, int row2, int col2) {
            for (var y = row1; y <= row2; ++y) {
                for (var x = col1; x <= col2; ++x) {
                    if (map[y][x] != -1)
                        return true;
                }
            }
            return false;
        }
    }
}