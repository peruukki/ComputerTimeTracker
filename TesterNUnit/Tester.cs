using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Core;
using NUnit.Framework;
using ComputerTimeTracker;

namespace TesterNUnit
{
  /// <summary>
  /// The class that contains all the NUnit tests.
  /// </summary>
  [TestFixture]
  public class Tester
  {
    /// <summary>
    /// Initialization method called before every test method.
    /// </summary>
    [SetUp]
    public void Init()
    {
      // Nothing to initialize
    }

    /// <summary>
    /// Ensures that the application runs and exits without problems.
    /// </summary>
    [Test]
    public void RunApplication()
    {
      new NotifyIconApplicationContext().Exit();
    }

    /// <summary>
    /// Checks that the tracker start time is as expected.
    /// </summary>
    [Test]
    public void CheckStartTime()
    {
      DateTime now = DateTime.Now;
      TimeTracker tracker = new TimeTracker(now);
      Console.WriteLine("Checking start time " + now);
      tracker.StartTime = now;
      Assert.That(tracker.StartTime, Is.EqualTo(now));
    }
  }
}
