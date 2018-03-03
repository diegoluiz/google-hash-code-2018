using System;
using System.Collections.Generic;

namespace lasagnas.Entity {
  public class Ride {
    public int Id { get; }
    public Ride Previous;
    public Ride Next;

    public Intersection Start { get; } = new Intersection();
    public Intersection End { get; } = new Intersection();
    public int EarlierStart;
    public int LatestFinish;
    public bool Assigned { get; set; }

    public int Distance {
      get {
        return Math.Abs(Start.Row - End.Row) + Math.Abs(Start.Col - End.Col);
      }
    }


    public Ride(int id, int startRow, int startColumn, int finishRow, int finishColumn, int earlierStart, int latestFinish) {
      Id = id;
      Start.Row = startRow;
      Start.Col = startColumn;
      End.Col = finishColumn;
      End.Row = finishRow;
      this.EarlierStart = earlierStart;
      this.LatestFinish = latestFinish;
    }
  }
}
