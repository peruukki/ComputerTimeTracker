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
    /// Gets or sets the duration of the period.
    /// </summary>
    public TimeSpan Duration { get { return _duration; } set { _duration = value; } }
    private TimeSpan _duration;

    /// <summary>
    /// Gets or sets the value that tells if the period is considered work time.
    /// </summary>
    public bool IsWorkTime { get { return _isWorkTime; } set { _isWorkTime = value; } }
    private bool _isWorkTime = true;

    /// <summary>
    /// Gets the textual representation of the period duration.
    /// </summary>
    public string DurationText
    {
      get
      {
        if (Duration.Hours > 0)
        {
          return String.Format("{0}:{1:0#}:{2:0#}", Duration.Hours,
                               Duration.Minutes, Duration.Seconds);
        }
        else
        {
          return String.Format("{0}:{1:0#}", Duration.Minutes, Duration.Seconds);
        }
      }
    }

    /// <summary>
    /// Creates a new time period with no duration.
    /// </summary>
    public TimePeriod(PeriodType type)
    {
      _type = type;
    }

    /// <summary>
    /// Creates a new time period with the given duration.
    /// </summary>
    /// <param name="type">Period type.</param>
    /// <param name="duration">Period duration.</param>
    public TimePeriod(PeriodType type, TimeSpan duration)
      : this(type)
    {
      _duration = duration;
    }
  }
}
