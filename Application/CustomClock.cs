using System;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Represents a current time that can be set freely.
  /// </summary>
  public class CustomClock: IClock
  {
    /// <summary>
    /// Sets or gets the current time.
    /// </summary>
    public DateTime Now { get { return _now; } set { _now = value; } }
    private DateTime _now;

    /// <summary>
    /// Creates and initializes a CustomClock.
    /// </summary>
    /// <param name="now">Current time.</param>
    public CustomClock(DateTime now)
    {
      Now = now;
    }
  }
}
