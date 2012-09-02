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
    /// Trackable event activity type. Specifies which kind of a period the
    /// event starts.
    /// </summary>
    public enum EventActivity
    {
      /// <summary>
      /// The event starts an active period.
      /// </summary>
      Active,

      /// <summary>
      /// The event starts an inactive period.
      /// </summary>
      Inactive,
    }

    /// <summary>
    /// Gets the <see cref="EventActivity"/> of the given <see cref="EventType"/>.
    /// </summary>
    /// <param name="type">Event type.</param>
    /// <returns>Event activity.</returns>
    private static EventActivity GetActivity(EventType type)
    {
      switch (type)
      {
        case EventType.Lock:
          return EventActivity.Inactive;
        case EventType.Start:
        case EventType.Unlock:
          return EventActivity.Active;
        default:
          throw new ArgumentOutOfRangeException("Unknown event type " + type);
      }
    }

    /// <summary>
    /// Gets the <see cref="EventType"/> of this event.
    /// </summary>
    public EventType Type { get { return _type; } }
    private readonly EventType _type;

    /// <summary>
    /// Gets the time of occurrence of this event.
    /// </summary>
    public DateTime Time { get { return _time; } }
    private readonly DateTime _time;

    /// <summary>
    /// Gets the <see cref="EventActivity"/> of this event.
    /// </summary>
    public EventActivity Activity { get { return _activity; } }
    private readonly EventActivity _activity;

    /// <summary>
    /// Creates an immutable trackable event that occurred at the given time.
    /// </summary>
    /// <param name="type">Type of event that occured.</param>
    /// <param name="time">Time of occurrence.</param>
    public TrackableEvent(EventType type, DateTime time)
    {
      _type = type;
      _time = time;
      _activity = GetActivity(type);
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
          return "Locked";
        case EventType.Start:
          return "Usage started";
        case EventType.Unlock:
          return "Unlocked";
        default:
          throw new InvalidOperationException("Unknown type " + Type);
      }
    }
  }
}
