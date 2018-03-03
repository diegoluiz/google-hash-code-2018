using System.Collections.Generic;
using lasagnas.Util;

namespace lasagnas.Entity {
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
}
