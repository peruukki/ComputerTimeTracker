using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ComputerTimeTracker
{
  public class CustomApplicationContext : ApplicationContext
  {
    private NotifyIcon _notifyIcon;
    private TimeReport _reportForm;
    private IContainer _components;

    public CustomApplicationContext()
    {
      _reportForm = new TimeReport();
      _components = new Container();
      _notifyIcon = CreateNotifyIcon(_components);
      _notifyIcon.ContextMenu = CreateNotifyIconContextMenu();
    }

    public void Exit()
    {
      _components.Dispose();
      _reportForm.ForceClose();
      ExitThread();
    }

    private void ExitApplication(object Sender, EventArgs e)
    {
      Exit();
    }

    private ContextMenu CreateNotifyIconContextMenu()
    {
      ContextMenu contextMenu = new ContextMenu();

      MenuItem itemReport = new MenuItem("View &Report", new EventHandler(ShowReport));
      itemReport.DefaultItem = true;
      MenuItem itemExit = new MenuItem("E&xit", new EventHandler(ExitApplication));
      contextMenu.MenuItems.AddRange(new MenuItem[] { itemReport, itemExit });

      return contextMenu;
    }

    private NotifyIcon CreateNotifyIcon(IContainer container)
    {
      NotifyIcon notifyIcon = new NotifyIcon(container);

      notifyIcon.Icon = SystemIcons.Application;
      notifyIcon.Text = "Computer Time Tracker";
      notifyIcon.Visible = true;
      notifyIcon.DoubleClick += new System.EventHandler(ShowReport);

      return notifyIcon;
    }

    private void ShowReport(object Sender, EventArgs e)
    {
      _reportForm.Show();
      _reportForm.Activate();
    }
  }
}