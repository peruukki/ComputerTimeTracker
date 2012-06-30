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
    /// The number of events added to the tracker.
    /// </summary>
    public int EventCount { get { return _eventCount; } }
    private int _eventCount;

    /// <summary>
    /// Creates a TimeTracker instance.
    /// </summary>
    /// <param name="startTime">Computer usage start time.</param>
    public TimeTracker(DateTime startTime)
    {
      StartTime = startTime;
    }

    /// <summary>
    /// Addes the given event to the tracker.
    /// </summary>
    /// <param name="trackEvent">A trackable event.</param>
    public void AddEvent(TrackableEvent trackEvent)
    {
      _eventCount++;
    }
  }
}
