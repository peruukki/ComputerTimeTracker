using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Tracks computer usage time.
  /// </summary>
  public class TimeTracker
  {
    /// <summary>
    /// Computer usage start time.
    /// </summary>
    public DateTime StartTime
    {
      get { return _startTime; }
      set { _startTime = value; }
    }
    private DateTime _startTime;

    /// <summary>
    /// Creates a TimeTracker instance.
    /// </summary>
    /// <param name="startTime">Computer usage start time.</param>
    public TimeTracker(DateTime startTime)
    {
      StartTime = startTime;
    }
  }
}
