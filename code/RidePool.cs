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
    }

    public int CountRide {
      get {
        return _rides.Count;
      }
    }

    public Ride GetBestRideFor (Car car) {
      return _rides
        .OrderBy (x => x.EarlierStart)
        .FirstOrDefault (x => !x.Assigned);
    }
  }
}
