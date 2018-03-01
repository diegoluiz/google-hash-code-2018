using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace lasagnas {
  public class RidePool {

    private int _bonusPoints;
    private Ride[] _rides;

    private Ride _freeFirst;
    private Ride _freeLast;

    private void AddFree (Ride ride) {
      if (_freeLast != null) {
        ride.FreePrev = _freeLast;
        _freeLast.FreeNext = ride;
      } else {
        _freeFirst = ride;
      }
      _freeLast = ride;
    }

    public void RemoveFree (Ride ride) {
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

    public RidePool (List<InputRow> rows, int bonusPoints = 0) {
      _bonusPoints = bonusPoints;
      var rides = new Ride[rows.Count];
      for (var i = 0; i < rides.Length; ++i) {
        var c = rows[i];
        rides[i] = new Ride (i, c.StartRow, c.StartColumn, c.FinishRow, c.FinishColumn, c.EarlierStart, c.LatestFinish);
      }
      Array.Sort (rides, (a, b) => a.EarlierStart - b.EarlierStart);
      this._rides = rides;
      foreach (var ride in rides)
        AddFree (ride);
    }

    public int CountRide =>
      _rides.Length;

    public Ride GetBestRideFor (Car car, int tick) {
      //var availableRides = _rides.Where(x => !x.Assigned);

      var minDistance = int.MaxValue;
      var a = new List<Ride> ();
      // Ride closestRideToCar = null;
      for (var ride = _freeFirst; ride != null; ride = ride.FreeNext) {
        var distanceFromStartingPoint = car.CurrentPosition.GetDistance (ride.Start);

        if(ride.EarlierStart - tick > 50){
          break;
        }

        if(tick + distanceFromStartingPoint + ride.Distance > ride.LatestFinish) {
          continue;
        }

        if (minDistance > distanceFromStartingPoint) {
          minDistance = distanceFromStartingPoint;
          // closestRideToCar = ride;
          a = new List<Ride> ();
          a.Add (ride);
        } 
        else if (minDistance == distanceFromStartingPoint) {
          a.Add (ride);
        }

      }
      var z =  a
        .Where (x => x.EarlierStart >= tick + minDistance)
        .OrderBy (x => x.EarlierStart).FirstOrDefault ();

      return z ?? a.OrderByDescending (x => x.EarlierStart).FirstOrDefault ();

      // return closestRideToCar;
    }
  }
}
