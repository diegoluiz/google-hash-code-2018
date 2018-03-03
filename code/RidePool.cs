using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using lasagnas.Entity;
using lasagnas.Parser;

namespace lasagnas {
  public class RidePool {
    private const int START_THRESHOLD = 100;
    private Ride[] _rides;

    private Ride _freeFirst;
    private Ride _freeLast;

    private void AddFree(Ride ride) {
      if (_freeLast != null) {
        ride.Previous = _freeLast;
        _freeLast.Next = ride;
      } else {
        _freeFirst = ride;
      }
      _freeLast = ride;
    }

    public void RemoveFree(Ride ride) {
      if (ride.Assigned)
        return;
      ride.Assigned = true;
      var prev = ride.Previous;
      var next = ride.Next;
      if (prev != null) {
        prev.Next = next;
      } else {
        _freeFirst = next;
      }
      if (next != null) {
        next.Previous = prev;
      } else {
        _freeLast = prev;
      }
    }

    public RidePool(List<InputRow> rows, int bonusPoints = 0) {
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

    public int CountRide => _rides.Length;

    public Ride GetBestRideFor(Car car, int tick) {
      var minDistance = int.MaxValue;
      var promisingRides = new List<Ride>();
      for (var ride = _freeFirst; ride != null; ride = ride.Next) {
        var isRideStartAboveThreshold = ride.EarlierStart - tick > START_THRESHOLD;
        if (isRideStartAboveThreshold) {
          break;
        }

        var distanceFromStartingPoint = car.CurrentPosition.GetDistance(ride.Start);
        var tickRideWillFinish = tick + distanceFromStartingPoint + ride.Distance;
        var isValidRide = tickRideWillFinish > ride.LatestFinish;
        if (isValidRide) {
          continue;
        }

        if (minDistance > distanceFromStartingPoint) {
          minDistance = distanceFromStartingPoint;
          promisingRides = new List<Ride>();
          promisingRides.Add(ride);
        } else if (minDistance == distanceFromStartingPoint) {
          promisingRides.Add(ride);
        }

      }

      var firstBonusRide = promisingRides
        .Where(x => x.EarlierStart >= tick + minDistance)
        .OrderBy(x => x.EarlierStart).FirstOrDefault();

      var earliestPromiseRide = promisingRides.OrderByDescending(x => x.EarlierStart).FirstOrDefault();

      return firstBonusRide ?? earliestPromiseRide;
    }
  }
}
