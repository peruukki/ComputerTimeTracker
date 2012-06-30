using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Core;
using NUnit.Framework;
using ComputerTimeTracker;

namespace TesterNUnit
{
  [TestFixture]
  public class Tester
  {
    [SetUp]
    public void Init()
    {
      // Nothing to initialize
    }

    /// <summary>
    /// Ensures that the application runs and exits without problems.
    /// </summary>
    [Test]
    public void RunApplication()
    {
      new CustomApplicationContext().Exit();
    }
  }
}
