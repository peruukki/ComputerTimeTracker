using System;
using System.Windows.Forms;
using System.Drawing;

namespace ComputerTimeTracker
{
  /// <summary>
  /// The form that shows the computer usage time report.
  /// </summary>
  public partial class TimeReport: Form, IMainForm
  {
    /// <summary>
    /// Whether the form should be closed when the Close event is received.
    /// </summary>
    private bool _close = false;

    /// <summary>
    /// The number of vertical pixels between trackable event labels.
    /// </summary>
    private readonly int EVENT_LABEL_HEIGHT;

    /// <summary>
    /// Initial height of the form, fitting one trackable event.
    /// </summary>
    private readonly int FORM_INITIAL_HEIGHT;

    /// <summary>
    /// Creates a new TimeReport instance.
    /// </summary>
    /// <param name="startTime">Computer usage start time.</param>
    public TimeReport(DateTime startTime)
    {
      InitializeComponent();
      FormClosing += new FormClosingEventHandler(MainFormClosing);

      // The start labels are only used for helping in layout design
      _lblTimeStart.Visible = false;
      _lblTextStart.Visible = false;
      EVENT_LABEL_HEIGHT = _lblTextCurrent.Top - _lblTextStart.Top;
      FORM_INITIAL_HEIGHT = Height;
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
      int timeLeft = _lblTimeStart.Left;
      int descriptionLeft = _lblTextStart.Left;
      int top = _lblTextStart.Top;
      int labelCount = 0;

      foreach (TrackableEvent trackableEvent in timeTracker.Events)
      {
        Label timeLabel = new Label();
        timeLabel.AutoSize = true;
        timeLabel.Location = new Point(timeLeft, top);
        timeLabel.Text = trackableEvent.Time.ToLongTimeString(); ;
        Controls.Add(timeLabel);

        Label descriptionLabel = new Label();
        descriptionLabel.AutoSize = true;
        descriptionLabel.Location = new Point(descriptionLeft, top);
        descriptionLabel.Text = trackableEvent.ToString();
        Controls.Add(descriptionLabel);

        top += EVENT_LABEL_HEIGHT;
        labelCount++;
      }

      // Adjust the height; the initial height can fit one event
      Height = FORM_INITIAL_HEIGHT + ((labelCount - 1) * EVENT_LABEL_HEIGHT);
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