using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Represents the current system time.
  /// </summary>
  public class SystemClock: Clock
  {
    /// <summary>
    /// Gets the current system time.
    /// </summary>
    public override DateTime Now
    {
      get
      {
        return CreateValidTime(DateTime.Now);
      }

      set
      {
        throw new InvalidOperationException("The current time of a SystemClock cannot be set.");
      }
    }
  }
}
