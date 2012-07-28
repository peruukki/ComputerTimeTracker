using System;
using System.Windows.Forms;

namespace ComputerTimeTracker
{
  /// <summary>
  /// The form that shows the computer usage time report.
  /// </summary>
  public partial class TimeReport: Form, IMainForm
  {
    private bool _close = false;

    /// <summary>
    /// Creates a new TimeReport instance.
    /// </summary>
    /// <param name="startTime">Computer usage start time.</param>
    public TimeReport(DateTime startTime)
    {
      InitializeComponent();
      _lblTimeStart.Text = startTime.ToLongTimeString();
      FormClosing += new FormClosingEventHandler(MainFormClosing);
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
    /// Updates the time report based on the given time tracker state.
    /// </summary>
    /// <param name="timeTracker">Time tracker.</param>
    public void UpdateReport(TimeTracker timeTracker)
    {
      UpdateEventContent(timeTracker);
      UpdateStaticContent(timeTracker);
    }

    /// <summary>
    /// Updates the labels that describe tracked events.
    /// </summary>
    /// <param name="timeTracker">Time tracker.</param>
    private void UpdateEventContent(TimeTracker timeTracker)
    {
      _lblTextStart.Text = timeTracker.Events[0].ToString();
      _lblTimeStart.Text = timeTracker.Events[0].Time.ToLongTimeString();
    }

    /// <summary>
    /// Updates the labels that always appear in the form.
    /// </summary>
    /// <param name="timeTracker">Time tracker.</param>
    private void UpdateStaticContent(TimeTracker timeTracker)
    {
      DateTime currentTime = DateTime.Now;
      _lblTimeCurrent.Text = currentTime.ToLongTimeString();

      TimeSpan workTime = timeTracker.GetWorkTime(currentTime);
      _lblTimeWork.Text = String.Format("{0:0#}:{1:0#}:{2:0#}",
                                        workTime.Hours, workTime.Minutes,
                                        workTime.Seconds);
    }

    /// <summary>
    /// Called before the form is closed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Closing event related data.</param>
    public void MainFormClosing(object sender, FormClosingEventArgs e)
    {
      if ((e.CloseReason == CloseReason.UserClosing) && !_close)
      {
        e.Cancel = true;
        Hide();
      }
    }

    /// <summary>
    /// Called when the OK button is clicked.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void _btnOk_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}