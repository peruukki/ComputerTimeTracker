using System;
using System.Windows.Forms;

namespace ComputerTimeTracker
{
  /// <summary>
  /// The form that shows the computer usage time report.
  /// </summary>
  public partial class TimeReport : Form
  {
    private bool _close = false;

    /// <summary>
    /// Creates a new TimeReport instance.
    /// </summary>
    public TimeReport()
    {
      InitializeComponent();
      FormClosing += new FormClosingEventHandler(ReportFormClosing);
    }

    /// <summary>
    /// Actually closes the form. Calling the <see cref="Close"/> method
    /// only hides the form.
    /// </summary>
    public void ForceClose()
    {
      _close = true;
      Close();
    }

    /// <summary>
    /// Called before the form is closed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Closing event related data.</param>
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