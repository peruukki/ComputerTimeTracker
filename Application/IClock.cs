using System;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Interface implemented by classes that can be used to get the current time.
  /// </summary>
  public interface IClock
  {
    /// <summary>
    /// Returns the current time.
    /// </summary>
    DateTime Now { get; }
  }
}
