using System;
using System.IO;

namespace lasagnas {
  class Program {
    static void Main (string[] args) {
      foreach (var input in InputFile.Load (args.Length > 0 ? args[0] : "../data/")) {
        var runner = new Runner (input);
        using (Log.Time ($"Input {input.Name}"))
        runner.Run ();
      }
    }
  }
}
