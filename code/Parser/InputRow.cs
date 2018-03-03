namespace lasagnas.Parser {
  public class InputRow {
    public int StartRow;
    public int StartColumn;
    public int FinishRow;
    public int FinishColumn;
    public int EarlierStart;
    public int LatestFinish;

    public InputRow() { }

    public InputRow(string line) {
      var args = line.Split(' ');
      var i = 0;
      this.StartRow = int.Parse(args[i++]);
      this.StartColumn = int.Parse(args[i++]);
      this.FinishRow = int.Parse(args[i++]);
      this.FinishColumn = int.Parse(args[i++]);
      this.EarlierStart = int.Parse(args[i++]);
      this.LatestFinish = int.Parse(args[i++]);
    }
  }
}
