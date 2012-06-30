using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerTimeTracker
{
  /// <summary>
  /// An event that can be tracked by a <see cref="TimeTracker"/> instance.
  /// </summary>
  public class TrackableEvent
  {
    /// <summary>
    /// Trackable event type.
    /// </summary>
    public enum EventType
    {
      /// <summary>
      /// The computer is locked.
      /// </summary>
      Lock,

      /// <summary>
      /// The computer is unlocked.
      /// </summary>
      Unlock,
    }

    /// <summary>
    /// Gets the <see cref="EventType"/> of this event.
    /// </summary>
    public EventType Type { get { return _type; } }
    private EventType _type;

    /// <summary>
    /// Gets the time of occurrence of this event.
    /// </summary>
    public DateTime Time { get { return _time; } }
    private DateTime _time;

    /// <summary>
    /// Creates an immutable trackable event that occurred at the given time.
    /// </summary>
    /// <param name="type">Type of event that occured.</param>
    /// <param name="time">Time of occurrence.</param>
    public TrackableEvent(EventType type, DateTime time)
    {
      _type = type;
      _time = time;
    }
  }
}
