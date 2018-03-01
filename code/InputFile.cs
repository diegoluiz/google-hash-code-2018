using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace lasagnas {

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

  public class InputFile {

    private string _content;

    public readonly string Name;
    public readonly string InputPath;
    public readonly string OutputPath;

    public readonly string[] Lines;
    public readonly int Size;

    public ProblemInputData InputData;

    public string Content => _content ?? (_content = String.Join('\n', Lines));

    public override string ToString() => Name;

    public InputFile(string inputPath) {
      inputPath = Path.GetFullPath(inputPath);
      this.Name = Path.GetFileNameWithoutExtension(Path.GetFileName(inputPath));
      this.InputPath = inputPath;
      this.OutputPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(inputPath), "..", "out", this.Name + ".out"));
      this.Lines = File.ReadAllLines(inputPath);
      this.Size = (int)(new FileInfo(inputPath).Length);
      this.InputData = new ProblemInputData();
      this.InputData.LoadLines(this.Lines);
    }

    public void WriteOutput(string text) {
      Directory.CreateDirectory(Path.GetDirectoryName(this.OutputPath));
      File.WriteAllText(this.OutputPath, text);
    }

    public static List<InputFile> Load(string path, string searchPattern = "*.*") {
      var result = new List<InputFile>();
      if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory) {
        var filePaths = Directory.GetFiles(Path.Combine(path, "in"), searchPattern);
        foreach (var inputPath in filePaths)
          result.Add(new InputFile(inputPath));
        result.Sort((a, b) => a.Size - b.Size);
      } else {
        result.Add(new InputFile(path));
      }
      return result;
    }
  }
}
