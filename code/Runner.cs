using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace lasagnas {

  public class Car {
    public int Id { get; set; }
    public List<Ride> Rides { get; set; } = new List<Ride> ();
    public Intersection CurrentPosition { get; private set; } = new Intersection ();

    public Car (int id) {
      Id = id;
    }

    internal void Assign (Ride ride) {
      ride.Assigned = true;
      throw new NotImplementedException ();
    }
  }

  public class ClassRide {
    public Ride Ride { get; set; }
    public int StartTick { get; set; }

    public int Distance {
      get {
        return Ride.Distance;
      }
    }
  }

  public class Intersection {
    public int Row { get; set; }
    public int Col { get; set; }
  }

  public class Ride {
    public Intersection Start { get; } = new Intersection ();
    public Intersection End { get; } = new Intersection ();
    public int EarlierStart;
    public int LatestFinish;
    public bool Assigned { get; set; }

    public int Distance {
      get {
        return Math.Abs (Start.Row - End.Row) + Math.Abs (Start.Col - End.Col);
      }
    }

    public int Id { get; }

    public Ride (int id, int startRow, int startColumn, int finishRow, int finishColumn, int earlierStart, int latestFinish) {
      Id = id;
      Start.Row = startRow;
      Start.Col = startColumn;
      End.Col = finishColumn;
      End.Row = finishRow;
      this.EarlierStart = earlierStart;
      this.LatestFinish = latestFinish;
    }
  }

  public class Runner {

    public readonly InputFile Input;
    public readonly ProblemInputData InputData;

    public Runner (InputFile input) {
      this.Input = input;
      this.InputData = input.InputData;
    }

    public void Run () {
      Console.WriteLine ($"* Running {Input.Name}");

      var cars = new List<Car> ();
      for (var i = 0; i < InputData.Vehicles; i++) {
        cars.Add (new Car (i));
      }

      var rides = new List<Ride> ();
      var count = 0;
      foreach (var c in InputData.Items) {
        rides.Add (new Ride (count++, c.StartRow, c.StartColumn, c.FinishRow, c.FinishColumn, c.EarlierStart, c.LatestFinish));
      }

      rides = rides.OrderBy (x => x.EarlierStart).ToList ();

      for (var tick = 0; tick < InputData.Steps; tick++) {
        foreach (var car in cars) {
          if (!car.IsFree (tick)) continue;

          var ride = rides.First (x => !x.Assigned);
          car.Assign (ride);
        }
      }

      Log.Write ($"Cars {cars.Count} Rides {rides.Count}");
      PrintOutput ();
    }

    private void PrintOutput () {
      var sb = new StringBuilder ();

      //sb.AppendLine(pizza.Slices.Count.ToString());
      //foreach (var slice in pizza.Slices) {
      //  sb.AppendLine(string.Format("{0} {1} {2} {3}", slice.Row1, slice.Col1, slice.Row2, slice.Col2));
      //}

      var text = sb.ToString ();

      //Log.Debug("--------------- OUTPUT ---------------");
      //Log.Debug(text);
      //Log.Debug("--------------- OUTPUT ---------------");

      Input.WriteOutput (text);
    }
  }
}
