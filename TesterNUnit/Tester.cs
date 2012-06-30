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

    /// <summary>
    /// Adds events to the tracker and checks their validity from the tracker.
    /// </summary>
    [Test]
    public void AddEvents()
    {
      DateTime time = DateTime.Now;
      TimeTracker tracker = new TimeTracker(time);
      int eventCount = 0;

      Console.WriteLine("Adding Lock after 5 minutes");
      time = time.AddMinutes(5);
      tracker.AddEvent(new TrackableEvent(TrackableEvent.EventType.Lock, time));
      Assert.That(tracker.EventCount, Is.EqualTo(++eventCount));

      Console.WriteLine("Adding Unlock after 1 hour");
      time = time.AddHours(1);
      tracker.AddEvent(new TrackableEvent(TrackableEvent.EventType.Unlock, time));
      Assert.That(tracker.EventCount, Is.EqualTo(++eventCount));
    }
  }
}
