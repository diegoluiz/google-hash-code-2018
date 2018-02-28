using System;

namespace hashcode
{
    public class SliceMap {
        public int[][] map;

        public SliceMap(int width, int height) {
            map = new int[height][];
            for (int i = 0; i < width; ++i) {
                map[i] = new int[width];
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
    }
}