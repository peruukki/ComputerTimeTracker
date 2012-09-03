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
    /// Gets all completed time periods between events tracked by the tracker.
    /// A completed time period is one whose starting and ending events have occurred.
    /// </summary>
    public IList<TimePeriod> CompletedPeriods
    {
      get
      {
        IList<TimePeriod> periods = new List<TimePeriod>();
        TrackableEvent previousEvent = null;
        foreach (TrackableEvent currentEvent in Events)
        {
          if (previousEvent != null)
          {
            periods.Add(new TimePeriod((previousEvent.Activity ==
                                        TrackableEvent.EventActivity.Active) ?
                                       TimePeriod.PeriodType.Active :
                                       TimePeriod.PeriodType.Inactive,
                                       currentEvent.Time.Subtract(previousEvent.Time)));
          }
          previousEvent = currentEvent;
        }
        return periods;
      }
    }

    /// <summary>
    /// Gets the last completed time period. A completed time period is
    /// one whose starting and ending events have occurred.
    /// </summary>
    /// <returns>Last completed time period or null if none exists.</returns>
    public TimePeriod LastCompletedTimePeriod
    {
      get
      {
        IList<TimePeriod> periods = CompletedPeriods;
        return (periods.Count > 0) ? periods[periods.Count - 1] : null;
      }
    }

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

    /// <summary>
    /// Gets all tracked time periods.
    /// </summary>
    /// <param name="currentTime">Current time. Needed for calculating the
    /// duration of the current time period.</param>
    /// <returns>All time periods.</returns>
    public IList<TimePeriod> GetPeriods(DateTime currentTime)
    {
      IList<TimePeriod> periods = CompletedPeriods;
      periods.Add(new TimePeriod((LastEvent.Activity == TrackableEvent.EventActivity.Active) ?
                                 TimePeriod.PeriodType.Active : TimePeriod.PeriodType.Inactive,
                                 currentTime.Subtract(LastEvent.Time)));
      return periods;
    }
  }
}
