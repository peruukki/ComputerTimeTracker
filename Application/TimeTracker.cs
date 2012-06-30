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
    /// Gets the computer usage start time.
    /// </summary>
    public DateTime StartTime { get { return _startTime; } }
    private DateTime _startTime;

    /// <summary>
    /// Gets the events tracked by the tracker.
    /// </summary>
    public IList<TrackableEvent> Events { get { return _events; } }
    private IList<TrackableEvent> _events;

    /// <summary>
    /// Creates a TimeTracker instance.
    /// </summary>
    /// <param name="startTime">Computer usage start time.</param>
    public TimeTracker(DateTime startTime)
    {
      _startTime = startTime;
      _events = new List<TrackableEvent>();
      _events.Add(new TrackableEvent(TrackableEvent.EventType.Start, startTime));
    }
  }
}
