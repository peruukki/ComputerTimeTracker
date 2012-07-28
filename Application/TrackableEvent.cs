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
      /// Time tracking started.
      /// </summary>
      Start,

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

    /// <summary>
    /// Returns a description of the event.
    /// </summary>
    /// <returns>Event description.</returns>
    override public string ToString()
    {
      switch (Type)
      {
        case EventType.Lock:
          return "Computer locked";
        case EventType.Start:
          return "Computer usage started";
        case EventType.Unlock:
          return "Computer unlocked";
        default:
          throw new InvalidOperationException("Unknown type " + Type);
      }
    }
  }
}
