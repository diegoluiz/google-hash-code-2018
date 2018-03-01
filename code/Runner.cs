using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace lasagnas {

  public class Car {
    private int _nextFreeTick;

    public int Id { get; set; }
    public List<Ride> Rides { get; set; } = new List<Ride> ();
    public Intersection CurrentPosition { get; private set; } = new Intersection ();
    public bool IsFree (int tick) {
      return tick >= _nextFreeTick;
    }

    public Car (int id) {
      Id = id;
    }

    public void Assign (Ride ride, int tick) {
      Rides.Add (ride);
      _nextFreeTick = tick + CurrentPosition.GetDistance (ride.Start) + ride.Distance;
      CurrentPosition = ride.End;
      Log.Debug ("Car {0} Ride {1}", Id, ride.Id);
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

    internal int GetDistance (Intersection start) {
      return Math.Abs (Row - start.Row) + Math.Abs (Col - start.Col);
    }
  }

  public class Ride {
    public Ride FreePrev;
    public Ride FreeNext;

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

    private static Random rng = new Random();

    public static void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    public readonly InputFile Input;
    public readonly ProblemInputData InputData;

    public Runner (InputFile input) {
      this.Input = input;
      this.InputData = input.InputData;
    }

    public void Run () {
      var checkpoint = DateTime.Now.AddSeconds (30);

      Console.WriteLine ($"* Running {Input.Name}");

      var cars = new List<Car> ();
      for (var i = 0; i < InputData.Vehicles; i++) {
        cars.Add (new Car (i));
      }

      var ridePool = new RidePool (InputData.Items);

      Log.Write ($"Cars {cars.Count} Rides {ridePool.CountRide}");

      for (var tick = 0; tick < InputData.Steps; tick++) {

        if ((tick % 10000) == 0)
          Log.Write ($"tick {tick}");

        if (checkpoint.Ticks < DateTime.Now.Ticks) {
          checkpoint = DateTime.Now.AddSeconds (30);
          Log.Write ($"Checkpoint [{checkpoint.Ticks}] tick {tick}");
        }

        Shuffle(cars);

        foreach (var car in cars) {
          if (!car.IsFree (tick)) continue;

          var ride = ridePool.GetBestRideFor (car, tick);
          if (ride != null) {
            car.Assign (ride, tick);
            ridePool.RemoveFree (ride);
          }
        }
      }

      Log.Write ($"Finished.. Saving...");
      PrintOutput (cars);
    }

    private void PrintOutput (List<Car> cars) {
      var sb = new StringBuilder ();

      foreach (var car in cars) {
        sb.Append (string.Format (car.Rides.Count.ToString ()));
        foreach (var ride in car.Rides) {
          sb.Append (" " + ride.Id);
        }
        sb.Append ("\n");
      }

      var text = sb.ToString ();

      Log.Debug ("--------------- OUTPUT ---------------");
      Log.Debug (text);
      Log.Debug ("--------------- OUTPUT ---------------");

      Input.WriteOutput (text);
    }
  }
}
