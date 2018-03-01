using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lasagnas {

  public class Runner {

    public readonly InputFile Input;
    public readonly ProblemInputData InputData;

    public Runner(InputFile input) {
      this.Input = input;
      this.InputData = input.InputData;
    }

    public void Run() {
      Console.WriteLine($"* Running {Input.Name}");

      PrintOutput();
    }

    private void PrintOutput() {
      var sb = new StringBuilder();

      //sb.AppendLine(pizza.Slices.Count.ToString());
      //foreach (var slice in pizza.Slices) {
      //  sb.AppendLine(string.Format("{0} {1} {2} {3}", slice.Row1, slice.Col1, slice.Row2, slice.Col2));
      //}

      var text = sb.ToString();

      //Log.Debug("--------------- OUTPUT ---------------");
      //Log.Debug(text);
      //Log.Debug("--------------- OUTPUT ---------------");

      Input.WriteOutput(text);
    }
  }
}
