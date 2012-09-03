using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerTimeTracker
{
  /// <summary>
  /// A time period between <see cref="TrackableEvent"/>s.
  /// </summary>
  public class TimePeriod
  {
    /// <summary>
    /// Time period type.
    /// </summary>
    public enum PeriodType
    {
      /// <summary>
      /// The computer may be actively used during the time period.
      /// </summary>
      Active,

      /// <summary>
      /// The computer is not used during the time period.
      /// </summary>
      Inactive,
    }

    /// <summary>
    /// Gets the <see cref="PeriodType"/> of the period.
    /// </summary>
    public PeriodType Type { get { return _type; } }
    private readonly PeriodType _type;

    /// <summary>
    /// Gets the duration of the period.
    /// </summary>
    public TimeSpan Duration { get { return _duration; } }
    private readonly TimeSpan _duration;

    /// <summary>
    /// Creates a new time period.
    /// </summary>
    /// <param name="type">Period type.</param>
    /// <param name="duration">Period duration.</param>
    public TimePeriod(PeriodType type, TimeSpan duration)
    {
      _type = type;
      _duration = duration;
    }
  }
}
