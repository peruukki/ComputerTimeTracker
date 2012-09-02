using System;
using System.Windows.Forms;
using System.Drawing;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Interface implemented by the main form of the application.
  /// </summary>
  public interface IMainForm
  {
    /// <summary>
    /// Gets the color to indicate an active period.
    /// </summary>
    /// <returns>Active color.</returns>
    Color GetActiveColor();

    /// <summary>
    /// Gets the color to indicate an inactive period.
    /// </summary>
    /// <returns>Inactive color.</returns>
    Color GetInactiveColor();

    /// <summary>
    /// Gets the color of the last of the panels that illustrate the periods
    /// between events.
    /// </summary>
    /// <returns>Color of last period panel.</returns>
    Color GetLastPeriodPanelColor();

    /// <summary>
    /// Updates the main form based on the given time tracker state.
    /// </summary>
    /// <param name="timeTracker">Time tracker.</param>
    void UpdateForm(TimeTracker timeTracker);

    /// <summary>
    /// Called before the main form is closed.
    /// </summary>
    /// <param name="sender">Form closing event sender.</param>
    /// <param name="e">Form closing event arguments.</param>
    void MainFormClosing(object sender, FormClosingEventArgs e);
  }
}
