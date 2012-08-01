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
    public DateTime StartTime { get { return FirstEvent.Time; } }

    /// <summary>
    /// Gets the events tracked by the tracker.
    /// </summary>
    public IList<TrackableEvent> Events { get { return _events; } }
    private IList<TrackableEvent> _events;

    /// <summary>
    /// Gets the first event in the tracker.
    /// </summary>
    public TrackableEvent FirstEvent { get { return Events[0]; } }

    /// <summary>
    /// Gets the last event in the tracker.
    /// </summary>
    public TrackableEvent LastEvent { get { return Events[Events.Count - 1]; } }

    /// <summary>
    /// Creates a TimeTracker instance.
    /// </summary>
    /// <param name="startTime">Computer usage start time.</param>
    public TimeTracker(DateTime startTime)
    {
      _events = new List<TrackableEvent>();
      _events.Add(new TrackableEvent(TrackableEvent.EventType.Start, startTime));
    }

    /// <summary>
    /// Returns the current work time as calculated from current time and
    /// computer usage start time.
    /// </summary>
    /// <param name="currentTime">Current time.</param>
    /// <returns>Current work time.</returns>
    public TimeSpan GetWorkTime(DateTime currentTime)
    {
      return currentTime.Subtract(StartTime);
    }
  }
}
