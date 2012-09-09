using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using ComputerTimeTracker.Properties;
using Microsoft.Win32;

namespace ComputerTimeTracker
{
  /// <summary>
  /// An application context that displays a notification area icon
  /// instead of the main form at startup.
  /// </summary>
  public class NotifyIconApplicationContext : ApplicationContext
  {
    /// <summary>
    /// Gets the TimeTracker instance of the application.
    /// </summary>
    public TimeTracker TimeTracker { get { return _timeTracker; } }
    private TimeTracker _timeTracker;

    /// <summary>
    /// Gets the main form of the application.
    /// </summary>
    new public IMainForm MainForm { get { return _reportForm; } }
    private TimeReport _reportForm;
    private NotifyIcon _notifyIcon;
    private IContainer _components;

    private Clock _clock;

    /// <summary>
    /// Creates a new CustomApplicationContext instance.
    /// </summary>
    /// <param name="clock">The clock to use for getting the current time.</param>
    public NotifyIconApplicationContext(Clock clock)
      : this(clock, false)
    {
    }

    /// <summary>
    /// Creates a new CustomApplicationContext instance, possibly forcing
    /// update of the time tracker start time.
    /// </summary>
    /// <param name="clock">The clock to use for getting the current time.</param>
    /// <param name="forceUpdateStartTime">Whether time tracker start time is
    /// forcefully updated to the application launch time.</param>
    public NotifyIconApplicationContext(Clock clock, bool forceUpdateStartTime)
    {
      UpdateTimeTrackerStartTime(clock.Now, forceUpdateStartTime);
      _reportForm = new TimeReport();
      _components = new Container();
      _clock = clock;

      RestoreSettings();

      _notifyIcon = CreateNotifyIcon(_components);
      _notifyIcon.ContextMenu = CreateNotifyIconContextMenu();

      SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SessionEventOccurred);
    }

    /// <summary>
    /// Called when a system session switch event has occurred.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Session switch related data.</param>
    public void SessionEventOccurred(object sender, SessionSwitchEventArgs e)
    {
      TrackableEvent.EventType eventType;

      switch (e.Reason)
      {
        case SessionSwitchReason.SessionLock:
          eventType = TrackableEvent.EventType.Lock;
          break;

        case SessionSwitchReason.SessionUnlock:
          eventType = TrackableEvent.EventType.Unlock;
          break;

        default:
          return;
      }

      AddEvent(_timeTracker, new TrackableEvent(eventType, _clock.Now));
    }

    /// <summary>
    /// Adds an event to the time tracker and resets the tracker state if appropriate.
    /// </summary>
    /// <param name="tracker">Time tracker to update.</param>
    /// <param name="trackableEvent">Event to add.</param>
    private void AddEvent(TimeTracker tracker, TrackableEvent trackableEvent)
    {
      if ((trackableEvent.Time.DayOfYear != tracker.FirstEvent.Time.DayOfYear) &&
          (trackableEvent.Type == TrackableEvent.EventType.Unlock))
      {
        // The new event starts a new work day
        tracker.Events.Clear();
        tracker.Events.Add(new TrackableEvent(TrackableEvent.EventType.Start,
                                              trackableEvent.Time));
      }
      else
      {
        // Add event normally
        tracker.Events.Add(trackableEvent);
      }

      SaveSettings();
    }

    /// <summary>
    /// Updates the time tracker start time in the application settings
    /// if necessary and returns its current value.
    /// </summary>
    /// <param name="appLaunchTime">Current application launch time.</param>
    /// <param name="forceUpdateStartTime">Whether time tracker start time is
    /// forcefully updated to the application launch time.</param>
    private void UpdateTimeTrackerStartTime(DateTime appLaunchTime,
                                            bool forceUpdateStartTime)
    {
      DateTime startTime = Settings.Default.TimeTrackerStartTime;
      if (forceUpdateStartTime || (startTime == null) ||
          (startTime.Date != appLaunchTime.Date))
      {
        // Update the existing start time if this is the first time the application
        // has been launched today
        Settings.Default.TimeTrackerStartTime = appLaunchTime;
        Settings.Default.Save();
      }
    }

    /// <summary>
    /// Saves the application settings.
    /// </summary>
    private void SaveSettings()
    {
      Settings.Default.TimeTrackerStartTime = _timeTracker.StartTime;
      Settings.Default.Save();
    }

    /// <summary>
    /// Restores the application settings.
    /// </summary>
    private void RestoreSettings()
    {
      _timeTracker = new TimeTracker(Settings.Default.TimeTrackerStartTime);
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public void Exit()
    {
      _components.Dispose();
      _reportForm.ForceClose();
      ExitThread();
    }

    /// <summary>
    /// Exits the application. This is an event handler version of the
    /// <see cref="Exit"/> method.
    /// </summary>
    /// <param name="Sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void ExitApplication(object Sender, EventArgs e)
    {
      Exit();
    }

    /// <summary>
    /// Creates the context menu of the notification area icon.
    /// </summary>
    /// <returns>The created context menu.</returns>
    private ContextMenu CreateNotifyIconContextMenu()
    {
      ContextMenu contextMenu = new ContextMenu();

      MenuItem itemReport = new MenuItem("View &Report", new EventHandler(ShowReport));
      itemReport.DefaultItem = true;
      MenuItem itemExit = new MenuItem("E&xit", new EventHandler(ExitApplication));
      contextMenu.MenuItems.AddRange(new MenuItem[] { itemReport, itemExit });

      return contextMenu;
    }

    /// <summary>
    /// Creates the notification area icon.
    /// </summary>
    /// <param name="container">The container for the icon.</param>
    /// <returns>The created icon.</returns>
    private NotifyIcon CreateNotifyIcon(IContainer container)
    {
      NotifyIcon notifyIcon = new NotifyIcon(container);

      notifyIcon.Icon = SystemIcons.Application;
      notifyIcon.Text = "Computer Time Tracker";
      notifyIcon.Visible = true;
      notifyIcon.DoubleClick += new System.EventHandler(ShowReport);

      return notifyIcon;
    }

    /// <summary>
    /// Shows the computer usage time report.
    /// </summary>
    /// <param name="Sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    public void ShowReport(object Sender, EventArgs e)
    {
      _reportForm.UpdateForm(_timeTracker, _clock);
      _reportForm.Show();
      _reportForm.Activate();
    }
  }
}