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
      _context = new NotifyIconApplicationContext(new SystemClock());
      _context.ShowReport(null, null);
    }

    /// <summary>
    /// Ensures that the application main form is only closed when it should be.
    /// </summary>
    [Test]
    public void CloseMainForm()
    {
      _context = new NotifyIconApplicationContext(new SystemClock());

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
      DateTime now = new SystemClock().Now;
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
      // Run the application, forcing the start time to be updated
      DateTime launchTime1 = new DateTime(2012, 1, 1, 1, 1, 1);
      CustomClock clock = new CustomClock(launchTime1);
      Console.WriteLine("Launching application at " + clock.Now);
      _context = new NotifyIconApplicationContext(clock, true);
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(launchTime1));
      _context.Exit();

      // Run the application again on the same day
      DateTime launchTime2 = launchTime1.AddMinutes(1);
      clock.Now = launchTime2;
      Console.WriteLine("Launching application at " + clock.Now);
      _context = new NotifyIconApplicationContext(clock);
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(launchTime1));
      _context.Exit();

      // Run the application again on the next day
      DateTime launchTime3 = launchTime1.AddDays(1);
      clock.Now = launchTime3;
      Console.WriteLine("Launching application at " + clock.Now);
      _context = new NotifyIconApplicationContext(clock);
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
      _context = new NotifyIconApplicationContext(clock, true);

      Console.WriteLine("Session locked at " + clock.Now);
      int eventCount = _context.TimeTracker.GetEvents().Count;
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionLock));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(TrackableEvent.EventType.Lock));
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(clock.Now));
      Assert.That(_context.TimeTracker.GetEvents().Count, Is.EqualTo(++eventCount));

      // Verify that the start time remains unchanged when a Lock event is received the following day
      clock.Now = clock.Now.AddDays(1);
      Console.WriteLine("Session locked at " + clock.Now);
      DateTime startTimeBefore = _context.TimeTracker.StartTime;
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionLock));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(TrackableEvent.EventType.Lock));
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(startTimeBefore));
      Assert.That(_context.TimeTracker.GetEvents().Count, Is.EqualTo(++eventCount));

      // Verify that the start time is updated when an Unlock event is received the following day
      clock.Now = clock.Now.AddHours(1);
      Console.WriteLine("Session unlocked at " + clock.Now);
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionUnlock));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(TrackableEvent.EventType.Start));
      Assert.That(_context.TimeTracker.StartTime, Is.EqualTo(clock.Now));
      Assert.That(_context.TimeTracker.GetEvents().Count, Is.EqualTo(1));

      // Verify that the remote connection event is ignored
      TrackableEvent.EventType lastType = _context.TimeTracker.LastEvent.Type;
      eventCount = _context.TimeTracker.GetEvents().Count;
      Console.WriteLine("Remote connection");
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.RemoteConnect));
      Assert.That(_context.TimeTracker.LastEvent.Type, Is.EqualTo(lastType));
      Assert.That(_context.TimeTracker.GetEvents().Count, Is.EqualTo(eventCount));
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
      _context = new NotifyIconApplicationContext(clock, true);

      clock.Now = clock.Now.AddHours(1);
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionLock));
      clock.Now = clock.Now.AddHours(1);
      _context.SessionEventOccurred(this, new SessionSwitchEventArgs(SessionSwitchReason.SessionUnlock));
      int eventCount = _context.TimeTracker.GetEvents().Count;
      Console.WriteLine("Exiting application at " + clock.Now +
                        ", " + eventCount + " events tracked");
      _context.Exit();

      clock.Now = clock.Now.AddHours(1);
      Console.WriteLine("Restarting application at " + clock.Now);
      _context = new NotifyIconApplicationContext(clock, false);
      Assert.That(_context.TimeTracker.GetEvents().Count, Is.EqualTo(eventCount));
    }

    /// <summary>
    /// Adds events to the tracker and checks their validity from the tracker.
    /// </summary>
    [Test]
    public void AddEvents()
    {
      TimeTracker tracker = new TimeTracker(new SystemClock().Now);
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
      int eventCount = tracker.GetEvents().Count;
      DateTime time = tracker.GetEvents()[eventCount - 1].Time.Add(delay);

      // Add and verify the event
      tracker.AddEvent(new TrackableEvent(type, time));
      Assert.That(tracker.GetEvents().Count, Is.EqualTo(++eventCount));
      TrackableEvent lastEvent = tracker.LastEvent;
      Assert.That(lastEvent.Type, Is.EqualTo(type));
      Assert.That(lastEvent.Time, Is.EqualTo(time));
    }

    /// <summary>
    /// Verifies that the work time is correctly calculated.
    /// </summary>
    [Test]
    public void CheckWorkTime()
    {
      DateTime startTime = new SystemClock().Now;
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
      _context = new NotifyIconApplicationContext(new SystemClock());

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
      DateTime time = new SystemClock().Now;
      TimeSpan duration;

      duration = TimeTracker.GetPeriodDuration(time, time.AddDays(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(24, 0, 0)));

      duration = TimeTracker.GetPeriodDuration(time, time.AddHours(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(1, 0, 0)));

      duration = TimeTracker.GetPeriodDuration(time, time.AddMinutes(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(0, 1, 0)));

      duration = TimeTracker.GetPeriodDuration(time, time.AddSeconds(1));
      Assert.That(duration, Is.EqualTo(new TimeSpan(0, 0, 1)));
    }

    /// <summary>
    /// Verifies that an exception is thrown if a time period duration is calculated
    /// with a time that has milliseconds.
    /// </summary>
    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CheckInvalidTimePeriodDuration()
    {
      DateTime time = new SystemClock().Now;
      TimeTracker.GetPeriodDuration(time, time.AddMilliseconds(1));
    }

    /// <summary>
    /// Verifies that the system clock returns expected times.
    /// </summary>
    [Test]
    public void CheckSystemClock()
    {
      Clock clock = new SystemClock();

      // The current time must always have zero milliseconds
      for (int i = 0; i < 5; i++)
      {
        Assert.That(clock.Now.Millisecond, Is.EqualTo(0));
      }
    }

    /// <summary>
    /// Verifies that the work time is calculated correctly when time period
    /// work time statuses are changed.
    /// </summary>
    [Test]
    public void ChangeTimePeriodWorkTimeStatus()
    {
      Clock clock = new CustomClock(DateTime.Now);
      _context = new NotifyIconApplicationContext(clock);
      TimeTracker tracker = _context.TimeTracker;

      // Add some events
      for (int i = 0; i < 3; i++)
      {
        clock.Now = clock.Now.AddMinutes(1);
        tracker.AddEvent(new TrackableEvent(TrackableEvent.EventType.Lock, clock.Now));
        clock.Now = clock.Now.AddMinutes(1);
        tracker.AddEvent(new TrackableEvent(TrackableEvent.EventType.Unlock, clock.Now));
      }

      // Change all time periods to be non-work
      clock.Now = clock.Now.AddMinutes(1);
      foreach (TimePeriod period in tracker.GetPeriods(clock.Now))
      {
        period.IsWorkTime = false;
      }
      Assert.That(tracker.GetWorkTime(clock.Now), Is.EqualTo(new TimeSpan()));

      // Change some periods to work time
      clock.Now = clock.Now.AddMinutes(1);
      IList<TimePeriod> periods = tracker.GetPeriods(clock.Now);
      TimeSpan workTime = new TimeSpan();
      workTime = SetPeriodToWorkTime(periods, 0, workTime);
      workTime = SetPeriodToWorkTime(periods, periods.Count - 1, workTime);
      workTime = SetPeriodToWorkTime(periods, periods.Count / 2, workTime);
      Assert.That(tracker.GetWorkTime(clock.Now), Is.EqualTo(workTime));
    }

    /// <summary>
    /// Marks the time period with the given index as work time and adds the
    /// period duration to the given work time.
    /// </summary>
    /// <param name="periods">Tracked time periods.</param>
    /// <param name="index">Index of time period to mark.</param>
    /// <param name="workTime">Work time before the operation.</param>
    /// <returns>Work time after the operation.</returns>
    private TimeSpan SetPeriodToWorkTime(IList<TimePeriod> periods, int index,
                                         TimeSpan workTime)
    {
      periods[index].IsWorkTime = true;
      return workTime.Add(periods[index].Duration);
    }

    /// <summary>
    /// Verifies that the last, non-completed time period always has the correct duration.
    /// </summary>
    [Test]
    public void CheckNonCompletedPeriodDuration()
    {
      Clock clock = new CustomClock(DateTime.Now);
      _context = new NotifyIconApplicationContext(clock, true);
      DateTime startTime = clock.Now;

      TimeSpan duration = new TimeSpan(1, 1, 1);
      Assert.That(_context.TimeTracker.GetPeriods(startTime.Add(duration))[0].Duration,
                  Is.EqualTo(duration));

      duration = duration.Add(new TimeSpan(1, 1, 1));
      Assert.That(_context.TimeTracker.GetPeriods(startTime.Add(duration))[0].Duration,
                  Is.EqualTo(duration));
    }

    /// <summary>
    /// Checks that the time tracker events and periods are correct when the day changes.
    /// </summary>
    [Test]
    public void CheckTrackedStateAfterDayChange()
    {
      // Run the application, forcing the start time to be updated
      CustomClock clock = new CustomClock(new DateTime(2012, 1, 1, 1, 1, 1));
      Console.WriteLine("Launching application at " + clock.Now);
      _context = new NotifyIconApplicationContext(clock, true);
      TimeTracker tracker = _context.TimeTracker;

      // Add some events on the same day
      clock.Now = clock.Now.AddMinutes(1);
      tracker.AddEvent(new TrackableEvent(TrackableEvent.EventType.Lock, clock.Now));
      clock.Now = clock.Now.AddMinutes(1);
      tracker.AddEvent(new TrackableEvent(TrackableEvent.EventType.Unlock, clock.Now));

      // Add some events the next day
      clock.Now = clock.Now.AddDays(1);
      Console.WriteLine("Adding unlock event the next day at " + clock.Now);
      _context.AddEvent(new TrackableEvent(TrackableEvent.EventType.Unlock, clock.Now));
      clock.Now = clock.Now.AddSeconds(1);

      Assert.That(tracker.GetEvents().Count, Is.EqualTo(1));
      Assert.That(tracker.GetPeriods(clock.Now).Count, Is.EqualTo(1));
      Assert.That(tracker.LastCompletedTimePeriod, Is.EqualTo(null));
    }
  }
}
