using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComputerTimeTracker
{
  /// <summary>
  /// The class that contains the application main entry point.
  /// </summary>
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      //Application.Run(new TimeReport());
      Application.Run(new CustomApplicationContext());
    }
  }
}