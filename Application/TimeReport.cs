using System;
using System.Windows.Forms;

namespace ComputerTimeTracker
{
  public partial class TimeReport : Form
  {
    private bool _close = false;

    public TimeReport()
    {
      InitializeComponent();
      FormClosing += new FormClosingEventHandler(ReportFormClosing);
    }

    public void ForceClose()
    {
      _close = true;
      Close();
    }

    private void ReportFormClosing(object sender, FormClosingEventArgs e)
    {
      if (!_close)
      {
        e.Cancel = true;
        Hide();
      }
    }
  }
}