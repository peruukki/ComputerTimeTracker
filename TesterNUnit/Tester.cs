using System;
using System.Drawing;
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
      Assert.That(tracker.FirstEvent.Time, Is.EqualTo(now));
      Assert.That(tracker.FirstEvent.Type, Is.EqualTo(TrackableEvent.EventType.Start));
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

      Console.WriteLine("Session locked at " + clock.Now);
      int eventCount = _context.TimeTracker.Events.Count;
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionLock));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(TrackableEvent.EventType.Lock));
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(clock.Now));
      Assert.That(_context.TimeTracker.Events.Count, Is.EqualTo(++eventCount));

      // Verify that the start time remains unchanged when a Lock event is received the following day
      clock.Now = clock.Now.AddDays(1);
      Console.WriteLine("Session locked at " + clock.Now);
      DateTime startTimeBefore = _context.TimeTracker.StartTime;
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionLock));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(TrackableEvent.EventType.Lock));
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(startTimeBefore));
      Assert.That(_context.TimeTracker.Events.Count, Is.EqualTo(++eventCount));

      // Verify that the start time is updated when an Unlock event is received the following day
      clock.Now = clock.Now.AddHours(1);
      Console.WriteLine("Session unlocked at " + clock.Now);
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
    /// Verifies that the tracked events are persisted when the application
    /// is exited and launched again on the same day.
    /// </summary>
    [Test]
    public void CheckThatEventsAreStored()
    {
      CustomClock clock = new CustomClock(new DateTime(2012, 1, 1, 1, 1, 1));

      Console.WriteLine("Starting application at " + clock.Now);
      _context = new NotifyIconApplicationContext(clock.Now, true, clock);

      clock.Now = clock.Now.AddHours(1);
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionLock));
      clock.Now = clock.Now.AddHours(1);
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionUnlock));
      int eventCount = _context.TimeTracker.Events.Count;
      Console.WriteLine("Exiting application at " + clock.Now +
                        ", " + eventCount + " events tracked");
      _context.Exit();

      clock.Now = clock.Now.AddHours(1);
      Console.WriteLine("Restarting application at " + clock.Now);
      _context = new NotifyIconApplicationContext(clock.Now, false, clock);
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

    /// <summary>
    /// Verifies that the periods between events have correct value.
    /// </summary>
    [Test]
    public void CheckTimePeriods()
    {
      _context = new NotifyIconApplicationContext(DateTime.Now);

      TimeTracker tracker = _context.TimeTracker;
      CheckLastPeriod(tracker, TrackableEvent.EventType.Lock);
      CheckLastPeriod(tracker, TrackableEvent.EventType.Lock);
      CheckLastPeriod(tracker, TrackableEvent.EventType.Unlock);
      CheckLastPeriod(tracker, TrackableEvent.EventType.Unlock);
    }

    /// <summary>
    /// Adds an event and checks that the last time period has expected values.
    /// </summary>
    /// <param name="tracker">Time tracker to test.</param>
    /// <param name="type">Type of event to add.</param>
    private void CheckLastPeriod(TimeTracker tracker,
                                 TrackableEvent.EventType type)
    {
      TrackableEvent previousEvent = tracker.LastEvent;
      AddEvent(tracker, type, new TimeSpan(0, 1, 0));
      TimePeriod period = tracker.LastCompletedTimePeriod;
      TimePeriod.PeriodType expectedType = TimeTracker.GetPeriodTypeFromEvent(previousEvent.Activity);
      Assert.That(period.Type, Is.EqualTo(expectedType));
      Assert.That(period.Duration,
                  Is.EqualTo(tracker.LastEvent.Time.Subtract(previousEvent.Time)));
    }

    /// <summary>
    /// Verifies that time period durations are calculated as expected.
    /// </summary>
    [Test]
    public void CheckTimePeriodDuration()
    {
      DateTime time = DateTime.Now;
      TimeSpan duration;

      duration = TimeTracker.GetPeriodDuration(time, time.AddDays(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(24, 0, 0)));

      duration = TimeTracker.GetPeriodDuration(time, time.AddHours(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(1, 0, 0)));

      duration = TimeTracker.GetPeriodDuration(time, time.AddMinutes(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(0, 1, 0)));

      duration = TimeTracker.GetPeriodDuration(time, time.AddSeconds(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(0, 0, 1)));

      duration = TimeTracker.GetPeriodDuration(time, time.AddMilliseconds(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(0, 0, 0)));

      time = new DateTime(1, 1, 1, 1, 1, 1, 100);
      duration = TimeTracker.GetPeriodDuration(time, time.AddMilliseconds(900));
      Assert.That(duration, Is.EqualTo(new TimeSpan(0, 0, 1)));
    }
  }
}
