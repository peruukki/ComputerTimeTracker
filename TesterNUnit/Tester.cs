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
      _context = null;
    }

    /// <summary>
    /// Cleanup method called after every test method.
    /// </summary>
    [TearDown]
    public void Uninit()
    {
      if (_context != null)
      {
        _context.Exit();
      }
    }

    /// <summary>
    /// Ensures that the application runs and exits without problems.
    /// </summary>
    [Test]
    public void RunApplication()
    {
      _context = new NotifyIconApplicationContext(DateTime.Now);
      _context.ShowReport(null, null);
    }

    /// <summary>
    /// Ensures that the application main form is only closed when it should be.
    /// </summary>
    [Test]
    public void CloseMainForm()
    {
      _context = new NotifyIconApplicationContext(DateTime.Now);

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
      CustomClock clock = new CustomClock(new DateTime(2012, 1, 1, 1, 1, 1));
      _context = new NotifyIconApplicationContext(clock.Now, true, clock);

      Console.WriteLine("Session locked");
      int eventCount = _context.TimeTracker.Events.Count;
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionLock));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(TrackableEvent.EventType.Lock));
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(clock.Now));
      Assert.That(_context.TimeTracker.Events.Count, Is.EqualTo(++eventCount));

      // Verify that the start time is updated when an Unlock event is received the next day
      Console.WriteLine("Session unlocked");
      clock.Now = clock.Now.AddDays(1);
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionUnlock));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(TrackableEvent.EventType.Start));
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(clock.Now));
      Assert.That(_context.TimeTracker.Events.Count, Is.EqualTo(1));

      // Verify that the remote connection event is ignored
      TrackableEvent.EventType lastType = _context.TimeTracker.LastEvent.Type;
      eventCount = _context.TimeTracker.Events.Count;
      Console.WriteLine("Remote connection");
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.RemoteConnect));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(lastType));
      Assert.That(_context.TimeTracker.Events.Count, Is.EqualTo(eventCount));
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
