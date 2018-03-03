using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using lasagnas.Entity;
using lasagnas.Parser;
using lasagnas.Util;

namespace lasagnas {

  public class Runner {
    public readonly InputFile Input;
    public readonly ProblemInputData InputData;

    public Runner(InputFile input) {
      this.Input = input;
      this.InputData = input.InputData;
    }

    public void Run() {
      var checkpoint = DateTime.Now.AddSeconds(30);

      Console.WriteLine($"* Running {Input.Name}");

      var cars = new List<Car>();
      for (var i = 0; i < InputData.Vehicles; i++) {
        cars.Add(new Car(i));
      }

      var ridePool = new RidePool(InputData.Items);

      Log.Write($"Cars {cars.Count} Rides {ridePool.CountRide}");

      for (var tick = 0; tick < InputData.Steps; tick++) {

        if ((tick % 10000) == 0)
          Log.Write($"tick {tick}");

        if (checkpoint.Ticks < DateTime.Now.Ticks) {
          checkpoint = DateTime.Now.AddSeconds(30);
          Log.Write($"Checkpoint [{checkpoint.Ticks}] tick {tick}");
        }

        // This was added to try to improve the score shuffling the car to remove the bias of always getting the next free car
        // As it changes the output it's commented
        // Shuffler.Shuffle(cars);

        foreach (var car in cars) {
          if (!car.IsFree(tick)) continue;

          var ride = ridePool.GetBestRideFor(car, tick);
          if (ride != null) {
            car.Assign(ride, tick);
            ridePool.RemoveFree(ride);
          }
        }
      }

      Log.Write($"Finished.. Saving...");
      PrintOutput(cars);
    }

    private void PrintOutput(List<Car> cars) {
      var sb = new StringBuilder();

      foreach (var car in cars) {
        sb.Append(string.Format(car.Rides.Count.ToString()));
        foreach (var ride in car.Rides) {
          sb.Append(" " + ride.Id);
        }
        sb.Append("\n");
      }

      var text = sb.ToString();

      Log.Debug("--------------- OUTPUT ---------------");
      Log.Debug(text);
      Log.Debug("--------------- OUTPUT ---------------");

      Input.WriteOutput(text);
    }
  }
}
