using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lasagnas {

  public class Runner {

    public readonly InputFile Input;

    public Runner(InputFile input) {
      this.Input = input;
    }

    public void Run() {
      Console.WriteLine($"* Running {Input.Name}");

      Input.WriteOutput("hello");
    }
  }
}
