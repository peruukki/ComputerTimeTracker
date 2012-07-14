using System;
using System.Windows.Forms;

namespace ComputerTimeTracker
{
  /// <summary>
  /// Interface implemented by the main form of the application.
  /// </summary>
  public interface IMainForm
  {
    /// <summary>
    /// Called before the main form is closed.
    /// </summary>
    /// <param name="sender">Form closing event sender.</param>
    /// <param name="e">Form closing event arguments.</param>
    void MainFormClosing(object sender, FormClosingEventArgs e);
  }
}
