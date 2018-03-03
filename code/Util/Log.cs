using System;
using System.Diagnostics;

namespace lasagnas.Util {

  public static class Log {

    public static void Write (string text = "", params object[] args) {
      Console.WriteLine (string.Format (text, args));
    }

    [Conditional ("DEBUG")]
    public static void Debug (string text = "", params object[] args) {
      if (false)
        Console.WriteLine (string.Format (text, args));
    }

    public static IDisposable Time (string name = "") =>
      new DisposableTimer (name);

    public static void Time (Action action) {
      using (Time ())
      action ();
    }

    public static void Time (string name, Action action) {
      using (Time (name))
      action ();
    }

    private class DisposableTimer : Stopwatch, IDisposable {
      public readonly string Name;
      public DisposableTimer (string name = "") {
        this.Name = name ?? "";
        this.Start ();
      }
      public void Dispose () {
        Stop ();
        Write ($"{Name} took {Elapsed.TotalSeconds.ToString("0.###")} s");
      }
    }
  }

}
