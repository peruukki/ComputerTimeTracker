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
    /// Events tracked by the tracker.
    /// </summary>
    private IList<TrackableEvent> _events = new List<TrackableEvent>();

    /// <summary>
    /// Completed time periods tracked by the tracker. A completed time period is
    /// one whose starting and ending events have occurred.
    /// </summary>
    private IList<TimePeriod> _completedPeriods = new List<TimePeriod>();

    /// <summary>
    /// Current, non-completed time period.
    /// </summary>
    private TimePeriod _currentTimePeriod;

    /// <summary>
    /// Gets the first event in the tracker.
    /// </summary>
    public TrackableEvent FirstEvent { get { return _events[0]; } }

    /// <summary>
    /// Gets the last event in the tracker.
    /// </summary>
    public TrackableEvent LastEvent { get { return _events[_events.Count - 1]; } }

    /// <summary>
    /// Gets the last completed time period. A completed time period is
    /// one whose starting and ending events have occurred.
    /// </summary>
    /// <returns>Last completed time period or null if none exists.</returns>
    public TimePeriod LastCompletedTimePeriod
    {
      get
      {
        return (_completedPeriods.Count > 0) ?
               _completedPeriods[_completedPeriods.Count - 1] : null;
      }
    }

    /// <summary>
    /// Gets the time period from its starting event activity type.
    /// </summary>
    /// <param name="activity">Starting event activity type.</param>
    /// <returns>Time period type.</returns>
    public static TimePeriod.PeriodType GetPeriodTypeFromEvent(TrackableEvent.EventActivity activity)
    {
      return (activity == TrackableEvent.EventActivity.Active) ?
             TimePeriod.PeriodType.Active : TimePeriod.PeriodType.Inactive;
    }

    /// <summary>
    /// Creates a TimeTracker instance.
    /// </summary>
    /// <param name="startTime">Computer usage start time.</param>
    public TimeTracker(DateTime startTime)
    {
      _events.Add(new TrackableEvent(TrackableEvent.EventType.Start, startTime));
    }

    /// <summary>
    /// Gets a copy of the events tracked by the tracker.
    /// </summary>
    public IList<TrackableEvent> GetEvents()
    {
      return new List<TrackableEvent>(_events);
    }

    /// <summary>
    /// Clears all tracked events and sets a new start event.
    /// <param name="startEvent">The new start event.</param>
    /// </summary>
    public void ClearEvents(TrackableEvent startEvent)
    {
      _events.Clear();
      _events.Add(startEvent);
    }

    /// <summary>
    /// Adds a trackable event to the time tracker.
    /// </summary>
    /// <param name="trackableEvent">Trackable event.</param>
    public void AddEvent(TrackableEvent trackableEvent)
    {
      TrackableEvent previousEvent = LastEvent;
      _events.Add(trackableEvent);

      TimePeriod newPeriod = new TimePeriod(GetPeriodTypeFromEvent(previousEvent.Activity),
                                            GetPeriodDuration(previousEvent.Time,
                                                              trackableEvent.Time));
      if (_currentTimePeriod != null)
      {
        newPeriod.IsWorkTime = _currentTimePeriod.IsWorkTime;
        _currentTimePeriod = null;
      }
      _completedPeriods.Add(newPeriod);
    }

    /// <summary>
    /// Returns the current work time as calculated from current time and
    /// computer usage start time.
    /// </summary>
    /// <param name="currentTime">Current time.</param>
    /// <returns>Current work time.</returns>
    public TimeSpan GetWorkTime(DateTime currentTime)
    {
      TimeSpan workTime = new TimeSpan();
      foreach (TimePeriod period in GetPeriods(currentTime))
      {
        if (period.IsWorkTime)
        {
          workTime = workTime.Add(period.Duration);
        }
      }
      return workTime;
    }

    /// <summary>
    /// Gets all tracked time periods.
    /// </summary>
    /// <param name="currentTime">Current time. Needed for calculating the
    /// duration of the current time period.</param>
    /// <returns>All time periods.</returns>
    public IList<TimePeriod> GetPeriods(DateTime currentTime)
    {
      IList<TimePeriod> periods = new List<TimePeriod>(_completedPeriods);
      if (_currentTimePeriod == null)
      {
        _currentTimePeriod = new TimePeriod(GetPeriodTypeFromEvent(LastEvent.Activity),
                                            GetPeriodDuration(LastEvent.Time, currentTime));
      }
      periods.Add(_currentTimePeriod);
      return periods;
    }

    /// <summary>
    /// Gets the period duration from its start time and end time, ignoring milliseconds
    /// and rounding up the seconds correctly.
    /// </summary>
    /// <param name="startTime">Time period start time.</param>
    /// <param name="endTime">Time period end time.</param>
    /// <returns>Time period duration.</returns>
    public static TimeSpan GetPeriodDuration(DateTime startTime, DateTime endTime)
    {
      if (!Clock.IsValidTime(startTime))
      {
        throw new ArgumentOutOfRangeException("startTime", startTime,
                                              "The period start time is invalid " +
                                              "(probably too precise).");
      }
      if (!Clock.IsValidTime(endTime))
      {
        throw new ArgumentOutOfRangeException("endTime", endTime,
                                              "The period end time is invalid " +
                                              "(probably too precise).");
      }
      return endTime.Subtract(startTime);
    }
  }
}
