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
      Assert.That(tracker.StartTime, Is.EqualTo(now));
      Assert.That(tracker.Events[0].Time, Is.EqualTo(now));
      Assert.That(tracker.Events[0].Type, Is.EqualTo(TrackableEvent.EventType.Start));
    }

    /// <summary>
    /// Adds events to the tracker and checks their validity from the tracker.
    /// </summary>
    [Test]
    public void AddEvents()
    {
      TimeTracker tracker = new TimeTracker(DateTime.Now);
      AddEvent(tracker, TrackableEvent.EventType.Lock, new TimeSpan(0, 5, 0));
      AddEvent(tracker, TrackableEvent.EventType.Unlock, new TimeSpan(1, 5, 0));
    }

    /// <summary>
    /// Adds a trackable event to the tracker and verifies that it matches the
    /// last event in the tracker.
    /// </summary>
    /// <param name="tracker">Time tracker to test.</param>
    /// <param name="type">Type of event to add.</param>
    /// <param name="delay">Delay of the new event since the previous event.</param>
    private void AddEvent(TimeTracker tracker,
                          TrackableEvent.EventType type, TimeSpan delay)
    {
      Console.WriteLine(String.Format("Adding {0} after {1}", type, delay));

      // Add delay to last event time
      int eventCount = tracker.Events.Count;
      DateTime time = tracker.Events[eventCount - 1].Time.Add(delay);

      // Add and verify the event
      tracker.Events.Add(new TrackableEvent(type, time));
      Assert.That(tracker.Events.Count, Is.EqualTo(++eventCount));
      TrackableEvent lastEvent = tracker.Events[tracker.Events.Count - 1];
      Assert.That(lastEvent.Type, Is.EqualTo(type));
      Assert.That(lastEvent.Time, Is.EqualTo(time));
    }
  }
}
