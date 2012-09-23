using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

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
    /// Dynamic controls that may be permanently removed when updating the content.
    /// </summary>
    private readonly IList<Control> _dynamicControls = new List<Control>();

    /// <summary>
    /// The number of vertical pixels between trackable event labels.
    /// </summary>
    private readonly int EVENT_LABEL_HEIGHT;

    /// <summary>
    /// Initial height of the form, fitting one trackable event.
    /// </summary>
    private readonly int FORM_INITIAL_HEIGHT;

    /// <summary>
    /// The color of the last period panel.
    /// </summary>
    private Color _lastPeriodPanelColor;

    /// <summary>
    /// Creates a new TimeReport instance.
    /// </summary>
    public TimeReport()
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
        _dynamicControls.Add(timeLabel);

        Label descriptionLabel = new Label();
        descriptionLabel.AutoSize = true;
        descriptionLabel.Location = new Point(descriptionLeft, top);
        descriptionLabel.Text = trackableEvent.ToString();
        Controls.Add(descriptionLabel);
        _dynamicControls.Add(descriptionLabel);

        top += EVENT_LABEL_HEIGHT;
        labelCount++;
      }

      // Adjust the height; the initial height can fit one event
      Height = FORM_INITIAL_HEIGHT + ((labelCount - 1) * EVENT_LABEL_HEIGHT);
    }

    /// <summary>
    /// Updates the components that describe time periods between tracked events.
    /// </summary>
    /// <param name="timeTracker">Time tracker.</param>
    /// <param name="currentTime">Current time.</param>
    private void UpdateTimePeriodContent(TimeTracker timeTracker, DateTime currentTime)
    {
      int panelLeft = _pnlPeriod1.Left;
      int panelTop = _pnlPeriod1.Top;
      Size panelSize = _pnlPeriod1.Size;

      int checkBoxLeft = _chkPeriod1.Left;
      int checkBoxTop = _chkPeriod1.Top;
      Size checkBoxSize = _chkPeriod1.Size;

      foreach (TimePeriod period in timeTracker.GetPeriods(currentTime))
      {
        Panel periodPanel = new Panel();
        periodPanel.Size = panelSize;
        periodPanel.Location = new Point(panelLeft, panelTop);
        periodPanel.BackColor = (period.Type == TimePeriod.PeriodType.Active) ?
                                GetActiveBackColor() : GetInactiveBackColor();
        _lastPeriodPanelColor = periodPanel.BackColor;
        Controls.Add(periodPanel);
        _dynamicControls.Add(periodPanel);

        Label periodDurationLabel = new Label();
        periodDurationLabel.Size = periodPanel.Size;
        periodDurationLabel.Location = new Point(0, 0);
        periodDurationLabel.TextAlign = ContentAlignment.MiddleCenter;
        periodDurationLabel.ForeColor = (period.Type == TimePeriod.PeriodType.Active) ?
                                        GetActiveForeColor() : GetInactiveForeColor();
        periodDurationLabel.Text = period.DurationText;
        periodPanel.Controls.Add(periodDurationLabel);

        CheckBox periodCheckBox = new CheckBox();
        periodCheckBox.Size = checkBoxSize;
        periodCheckBox.Location = new Point(checkBoxLeft, checkBoxTop);
        periodCheckBox.Checked = true;
        periodCheckBox.Tag = period;
        Controls.Add(periodCheckBox);
        _dynamicControls.Add(periodCheckBox);

        panelTop += EVENT_LABEL_HEIGHT;
        checkBoxTop += EVENT_LABEL_HEIGHT;
      }
    }

    /// <summary>
    /// Updates the labels that always appear in the form.
    /// </summary>
    /// <param name="timeTracker">Time tracker.</param>
    /// <param name="currentTime">Current time.</param>
    private void UpdateStaticContent(TimeTracker timeTracker, DateTime currentTime)
    {
      _lblTimeCurrent.Text = currentTime.ToLongTimeString();

      TimeSpan workTime = timeTracker.GetWorkTime(currentTime);
      _lblTimeWork.Text = String.Format("{0:0#}:{1:0#}:{2:0#}",
                                        workTime.Hours, workTime.Minutes,
                                        workTime.Seconds);
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

    #region IMainForm Members

    public Color GetActiveBackColor()
    {
      return Color.Green;
    }

    public Color GetActiveForeColor()
    {
      return SystemColors.ControlLightLight;
    }

    public Color GetInactiveBackColor()
    {
      return Color.YellowGreen;
    }

    public Color GetInactiveForeColor()
    {
      return SystemColors.ControlDarkDark;
    }

    public Color GetLastPeriodPanelColor()
    {
      return _lastPeriodPanelColor;
    }

    public void UpdateForm(TimeTracker timeTracker, Clock clock)
    {
      foreach (Control component in _dynamicControls)
      {
        Controls.Remove(component);
      }
      _dynamicControls.Clear();

      DateTime now = clock.Now;
      UpdateEventContent(timeTracker);
      UpdateTimePeriodContent(timeTracker, now);
      UpdateStaticContent(timeTracker, now);
    }

    public void MainFormClosing(object sender, FormClosingEventArgs e)
    {
      if ((e.CloseReason == CloseReason.UserClosing) && !_close)
      {
        e.Cancel = true;
        Hide();
      }
    }

    #endregion // IMainForm Members
  }
}