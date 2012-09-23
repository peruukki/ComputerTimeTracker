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
    /// Gets the background color of an active period.
    /// </summary>
    /// <returns>Active background color.</returns>
    Color GetActiveBackColor();

    /// <summary>
    /// Gets the foreground color of an active period.
    /// </summary>
    /// <returns>Active foreground color.</returns>
    Color GetActiveForeColor();

    /// <summary>
    /// Gets the background color of an inactive period.
    /// </summary>
    /// <returns>Inactive background color.</returns>
    Color GetInactiveBackColor();

    /// <summary>
    /// Gets the foreground color of an inactive period.
    /// </summary>
    /// <returns>Inactive foreground color.</returns>
    Color GetInactiveForeColor();

    /// <summary>
    /// Gets the color of the last of the panels that illustrate the periods
    /// between events.
    /// </summary>
    /// <returns>Color of last period panel.</returns>
    Color GetLastPeriodPanelColor();

    /// <summary>
    /// Updates the main form based on its current time tracker state.
    /// </summary>
    /// <param name="clock">The clock to use for getting the current time.</param>
    void UpdateForm(Clock clock);

    /// <summary>
    /// Called before the main form is closed.
    /// </summary>
    /// <param name="sender">Form closing event sender.</param>
    /// <param name="e">Form closing event arguments.</param>
    void MainFormClosing(object sender, FormClosingEventArgs e);
  }
}
