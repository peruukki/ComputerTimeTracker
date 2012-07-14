using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using ComputerTimeTracker.Properties;

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

    /// <summary>
    /// Creates a new CustomApplicationContext instance.
    /// </summary>
    /// <param name="appLaunchTime">Application launch time.</param>
    public NotifyIconApplicationContext(DateTime appLaunchTime)
      : this(appLaunchTime, false)
    {
    }

    /// <summary>
    /// Creates a new CustomApplicationContext instance, possibly forcing
    /// update of the time tracker start time.
    /// </summary>
    /// <param name="appLaunchTime">Application launch time.</param>
    /// <param name="forceUpdateStartTime">Whether time tracker start time is
    /// forcefully updated to the application launch time.</param>
    public NotifyIconApplicationContext(DateTime appLaunchTime,
                                        bool forceUpdateStartTime)
    {
      DateTime trackerStartTime = UpdateTimeTrackerStartTime(appLaunchTime,
                                                             forceUpdateStartTime);
      _timeTracker = new TimeTracker(trackerStartTime);
      _reportForm = new TimeReport(trackerStartTime);
      _components = new Container();

      _notifyIcon = CreateNotifyIcon(_components);
      _notifyIcon.ContextMenu = CreateNotifyIconContextMenu();
    }

    /// <summary>
    /// Updates the time tracker start time in the application settings
    /// if necessary and returns its current value.
    /// </summary>
    /// <param name="appLaunchTime">Current application launch time.</param>
    /// <returns>Up-to-date time tracker start time.</returns>
    private DateTime UpdateTimeTrackerStartTime(DateTime appLaunchTime,
                                                bool forceUpdateStartTime)
    {
      DateTime startTime = Settings.Default.TimeTrackerStartTime;
      if (forceUpdateStartTime || (startTime.Date != appLaunchTime.Date))
      {
        // Update the existing start time if this is the first time the application
        // has been launched today
        startTime = appLaunchTime;
        Settings.Default.TimeTrackerStartTime = startTime;
        Settings.Default.Save();
      }
      return startTime;
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
    private void ShowReport(object Sender, EventArgs e)
    {
      _reportForm.UpdateReport(_timeTracker);
      _reportForm.Show();
      _reportForm.Activate();
    }
  }
}