using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace lasagnas.Parser {

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
