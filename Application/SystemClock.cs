using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Represents the current system time.
  /// </summary>
  public class SystemClock: IClock
  {
    /// <summary>
    /// Returns the current system time.
    /// </summary>
    public DateTime Now { get { return DateTime.Now; } }
  }
}
