using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ComputerTimeTracker
{
  /// <summary>
  /// An application context that displays a notification area icon
  /// instead of the main form at startup.
  /// </summary>
  public class NotifyIconApplicationContext : ApplicationContext
  {
    private TimeReport _reportForm;
    private NotifyIcon _notifyIcon;
    private IContainer _components;

    /// <summary>
    /// Creates a new CustomApplicationContext instance.
    /// </summary>
    public NotifyIconApplicationContext()
    {
      _reportForm = new TimeReport();
      _components = new Container();

      _notifyIcon = CreateNotifyIcon(_components);
      _notifyIcon.ContextMenu = CreateNotifyIconContextMenu();
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
      _reportForm.Show();
      _reportForm.Activate();
    }
  }
}