using System;

namespace lasagnas.Entity {
  public class Intersection {
    public int Row { get; set; }
    public int Col { get; set; }

    internal int GetDistance (Intersection start) {
      return Math.Abs (Row - start.Row) + Math.Abs (Col - start.Col);
    }
  }
}
