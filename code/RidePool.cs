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

    private Ride _freeFirst;
    private Ride _freeLast;

    private void AddFree(Ride ride) {
      if (_freeLast != null) {
        ride.FreePrev = _freeLast;
        _freeLast.FreeNext = ride;
      } else {
        _freeFirst = ride;
      }
      _freeLast = ride;
    }

    public void RemoveFree(Ride ride) {
      if (ride.Assigned)
        return;
      ride.Assigned = true;
      var prev = ride.FreePrev;
      var next = ride.FreeNext;
      if (prev != null) {
        prev.FreeNext = next;
      } else {
        _freeFirst = next;
      }
      if (next != null) {
        next.FreePrev = prev;
      } else {
        _freeLast = prev;
      }
    }

    public RidePool(List<InputRow> rows) {
      var rides = new Ride[rows.Count];
      for (var i = 0; i < rides.Length; ++i) {
        var c = rows[i];
        rides[i] = new Ride(i, c.StartRow, c.StartColumn, c.FinishRow, c.FinishColumn, c.EarlierStart, c.LatestFinish);
      }
      Array.Sort(rides, (a, b) => a.EarlierStart - b.EarlierStart);
      this._rides = rides;
      foreach (var ride in rides)
        AddFree(ride);
    }

    public int CountRide =>
      _rides.Length;

    public Ride GetBestRideFor(Car car) {
      //var availableRides = _rides.Where(x => !x.Assigned);

      var minDistance = int.MaxValue;
      Ride closestRideToCar = null;
      for (var ride = _freeFirst; ride != null; ride = ride.FreeNext) {
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
