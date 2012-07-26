using System;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Represents a current time that can be set freely.
  /// </summary>
  public class CustomClock: IClock
  {
    /// <summary>
    /// Returns the last set time as current time.
    /// </summary>
    public DateTime Now { get { return _now; } set { _now = value; } }
    private DateTime _now;
  }
}
