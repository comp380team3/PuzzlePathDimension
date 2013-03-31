using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using PuzzlePathDimension;

namespace PuzzlePathUnitTests {
  /// <summary>
  /// The LauncherTest class tests the functionality of the launcher.
  /// </summary>
  [TestFixture]
  public class LauncherTest {
    /// <summary>
    /// The starting angle of the launcher.
    /// </summary>
    private const float initialAngle = (float)Math.PI / 4;

    /// <summary>
    /// This test checks if the launcher's angle is adjusted properly.
    /// </summary>
    [Test]
    public void MoveRightTest() {
      const float moveOffset = (float) Math.PI / 64;

      // Create the launcher.
      Launcher l = new Launcher();
      // We don't need a texture, so pass in null for the texture parameter.
      l.Initialize(null, new Vector2(0, 0));
      // Give the launcher a ball; otherwise, it won't move.
      Ball b = new Ball();
      l.LoadBall(b);

      l.AdjustAngle(moveOffset);

      // The last parameter is the tolerance since precision problems
      // tend to come up with floating point numbers.
      Assert.AreEqual(l.Angle, initialAngle + moveOffset, (float) 0.00001);
    }

    /// <summary>
    /// Demonstration of a test that fails.
    /// </summary>
    [Test]
    public void FailingTestExample() {
      Assert.AreEqual(1, 2); // lol
    }
  }
}
