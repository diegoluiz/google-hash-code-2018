using System.Collections.Generic;

namespace lasagnas.Parser {
  public class ProblemInputData {
    public int Rows;
    public int Columns;
    public int Vehicles;
    public int Rides;
    public int Bonus;
    public int Steps;

    public List<InputRow> Items;

    public ProblemInputData() { }

    public void LoadLines(string[] lines) {
      var i = 0;
      var args = lines[0].Split(' ');
      this.Rows = int.Parse(args[i++]);
      this.Columns = int.Parse(args[i++]);
      this.Vehicles = int.Parse(args[i++]);
      this.Rides = int.Parse(args[i++]);
      this.Bonus = int.Parse(args[i++]);
      this.Steps = int.Parse(args[i++]);
      var items = new List<InputRow>();
      for (var n = 1; n < lines.Length; ++n)
        if (lines[n].Length > 0)
          items.Add(new InputRow(lines[n]));
      this.Items = items;
    }
  }
}
