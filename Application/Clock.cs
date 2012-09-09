using System;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Abstract base class for classes that can be used to get the current time.
  /// </summary>
  public abstract class Clock
  {
    /// <summary>
    /// Sets or gets the current time.
    /// </summary>
    public abstract DateTime Now { get; set; }

    /// <summary>
    /// Tells if the given time value could be returned as a time by a Clock instance.
    /// </summary>
    /// <param name="time">Time value to verify.</param>
    /// <returns>Whether the time is a valid one for a Clock.</returns>
    public static bool IsValidTime(DateTime time)
    {
      return CreateValidTime(time).Equals(time);
    }

    /// <summary>
    /// Converts the give time to a time value that can be returned by a Clock instance.
    /// </summary>
    /// <param name="time">Time value to convert.</param>
    /// <returns>Valid time value.</returns>
    protected static DateTime CreateValidTime(DateTime time)
    {
      return new DateTime(time.Year, time.Month, time.Day, time.Hour,
                          time.Minute, time.Second);
    }
  }
}
