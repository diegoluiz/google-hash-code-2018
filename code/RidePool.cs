using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace lasagnas {
  public class RidePool {
    private List<Ride> _rides;
    public RidePool () {
      _rides = new List<Ride> ();
    }

    public void AddRide (Ride ride) {
      _rides.Add (ride);
      _rides = _rides.OrderBy (x => x.EarlierStart).ToList ();
    }

    public int CountRide {
      get {
        return _rides.Count;
      }
    }

    public Ride GetBestRideFor (Car car) {
      var availableRides = _rides.Where (x => !x.Assigned);

      var minDistance = int.MaxValue;
      Ride closestRideToCar = null;
      foreach (var ride in availableRides) {
        var distanceFromStartingPoint = car.CurrentPosition.GetDistance (ride.Start);
        if (minDistance > distanceFromStartingPoint) {
          minDistance = distanceFromStartingPoint;
          closestRideToCar = ride;
        }
      }

      return closestRideToCar;
    }
  }
}
