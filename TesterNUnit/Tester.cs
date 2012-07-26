using System;
using System.Text;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Framework;
using ComputerTimeTracker;
using Microsoft.Win32;
using System.Collections.Generic;

namespace TesterNUnit
{
  /// <summary>
  /// The class that contains all the NUnit tests.
  /// </summary>
  [TestFixture]
  public class Tester
  {
    NotifyIconApplicationContext _context;

    /// <summary>
    /// Initialization method called before every test method.
    /// </summary>
    [SetUp]
    public void Init()
    {
      _context = new NotifyIconApplicationContext(DateTime.Now);
    }

    /// <summary>
    /// Cleanup method called after every test method.
    /// </summary>
    [TearDown]
    public void Uninit()
    {
      _context.Exit();
    }

    /// <summary>
    /// Ensures that the application runs and exits without problems.
    /// </summary>
    [Test]
    public void RunApplication()
    {
      // Nothing to do, just runs the SetUp and TearDown methods
    }

    /// <summary>
    /// Ensures that the application main form is only closed when it should be.
    /// </summary>
    [Test]
    public void CloseMainForm()
    {
      Console.WriteLine("User closing main form");
      FormClosingEventArgs e1 = new FormClosingEventArgs(CloseReason.UserClosing, false);
      _context.MainForm.MainFormClosing(this, e1);
      Assert.That(e1.Cancel, Is.True);

      Console.WriteLine("Windows shutdown closing main form");
      FormClosingEventArgs e2 = new FormClosingEventArgs(CloseReason.WindowsShutDown, false);
      _context.MainForm.MainFormClosing(this, e2);
      Assert.That(e2.Cancel, Is.False);
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
    /// Checks that the time tracker start time is valid after application restarts.
    /// </summary>
    [Test]
    public void CheckStartTimeAfterRestarts()
    {
      DateTime launchTime1 = new DateTime(2012, 1, 1, 1, 1, 1);

      // Exit the common context and reuse it
      _context.Exit();

      // Run the application, forcing the start time to be updated
      Console.WriteLine("Launching application at " + launchTime1);
      _context = new NotifyIconApplicationContext(launchTime1, true);
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(launchTime1));
      _context.Exit();

      // Run the application again on the same day
      DateTime launchTime2 = launchTime1.AddMinutes(1);
      Console.WriteLine("Launching application at " + launchTime2);
      _context = new NotifyIconApplicationContext(launchTime2);
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(launchTime1));
      _context.Exit();

      // Run the application again on the next day
      DateTime launchTime3 = launchTime1.AddDays(1);
      Console.WriteLine("Launching application at " + launchTime3);
      _context = new NotifyIconApplicationContext(launchTime3);
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(launchTime3));

      // The context will be exited in TearDown
    }

    /// <summary>
    /// Sends system session switch events to the application and verifies that
    /// it creates trackable events from them as expected.
    /// </summary>
    [Test]
    public void HandleSessionEvents()
    {
      SendHandledSessionEvent("Session locked", _context,
                              new SessionSwitchEventArgs(SessionSwitchReason.SessionLock),
                              TrackableEvent.EventType.Lock);

      SendHandledSessionEvent("Session unlocked", _context,
                              new SessionSwitchEventArgs(SessionSwitchReason.SessionUnlock),
                              TrackableEvent.EventType.Unlock);

      SendIgnoredSessionEvent("Remote connection", _context,
                              new SessionSwitchEventArgs(SessionSwitchReason.RemoteConnect));
    }

    /// <summary>
    /// Sends a session event to the application that it is expected to handle.
    /// </summary>
    /// <param name="description">Event description for logging purposes.</param>
    /// <param name="context">Application context.</param>
    /// <param name="e">Session event to send.</param>
    /// <param name="expectedEventType">Expected trackable event type added by
    /// the application.</param>
    private void SendHandledSessionEvent(string description,
                                         NotifyIconApplicationContext context,
                                         SessionSwitchEventArgs e,
                                         TrackableEvent.EventType expectedEventType)
    {
      IList<TrackableEvent> events = context.TimeTracker.Events;
      int eventCount = events.Count;

      Console.WriteLine(description);
      context.SessionEventOccurred(this, e);

      Assert.That(events.Count, Is.EqualTo(++eventCount));
      TrackableEvent lastEvent = events[events.Count - 1];
      Assert.That(lastEvent.Type, Is.EqualTo(expectedEventType));
    }

    /// <summary>
    /// Sends a session event to the application that it is expected to ignore.
    /// </summary>
    /// <param name="description">Event description for logging purposes.</param>
    /// <param name="context">Application context.</param>
    /// <param name="e">Session event to send.</param>
    /// the application.</param>
    private void SendIgnoredSessionEvent(string description,
                                         NotifyIconApplicationContext context,
                                         SessionSwitchEventArgs e)
    {
      IList<TrackableEvent> events = context.TimeTracker.Events;
      int eventCount = events.Count;

      Console.WriteLine(description);
      context.SessionEventOccurred(this, e);

      Assert.That(events.Count, Is.EqualTo(eventCount));
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

    /// <summary>
    /// Verifies that the work time is correctly calculated.
    /// </summary>
    [Test]
    public void CheckWorkTime()
    {
      DateTime startTime = DateTime.Now;
      TimeTracker tracker = new TimeTracker(startTime);
      TimeSpan workTime = new TimeSpan(1, 2, 3);
      Assert.That(tracker.GetWorkTime(startTime.Add(workTime)), Is.EqualTo(workTime));
    }
  }
}
