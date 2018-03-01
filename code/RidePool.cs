using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace lasagnas {
  public class RidePool {

    private Ride[] _rides;

    public RidePool(List<InputRow> rows) {
      var rides = new Ride[rows.Count];
      for (var i = 0; i < rides.Length; ++i) {
        var c = rows[i];
        rides[i] = new Ride(i, c.StartRow, c.StartColumn, c.FinishRow, c.FinishColumn, c.EarlierStart, c.LatestFinish);
      }
      Array.Sort(rides, (a, b) => a.EarlierStart - b.EarlierStart);
      this._rides = rides;
    }

    public int CountRide =>
      _rides.Length;

    public Ride GetBestRideFor(Car car) {
      var availableRides = _rides.Where(x => !x.Assigned);

      var minDistance = int.MaxValue;
      Ride closestRideToCar = null;
      foreach (var ride in availableRides) {
        var distanceFromStartingPoint = car.CurrentPosition.GetDistance(ride.Start);
        if (minDistance > distanceFromStartingPoint) {
          minDistance = distanceFromStartingPoint;
          closestRideToCar = ride;
        }
      }

      return closestRideToCar;
    }
  }
}
