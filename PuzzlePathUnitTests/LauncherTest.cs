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
    /// The starting magnitude of the launcher.
    /// </summary>
    private const float initialMagnitude = 10f;
    /// <summary>
    /// The lowest valid angle for the launcher.
    /// </summary>
    private const float lowestAngle = 0f;
    /// <summary>
    /// The highest valid angle for the launcher.
    /// </summary>
    private const float highestAngle = (float)Math.PI;
    /// <summary>
    /// The lowest magnitude for the launcher.
    /// </summary>
    private const float lowestMagnitude = 5f;
    /// <summary>
    /// The highest magnitude for the launcher.
    /// </summary>
    private const float highestMagnitude = 15f;

    /// <summary>
    /// This method is called before running each of these tests.
    /// </summary>
    [SetUp]
    public void Init() {
      // Right now, I don't need something that needs to be applied to every
      // test, but you might.
    }

    /// <summary>
    /// This method is called after running each of these tests.
    /// </summary>
    [TearDown]
    public void Cleanup() {
      // Nothing to call Dispose() on in this class. It's possible that you might
      // use something that implements the IDisposable interface, though.
    }

    /// <summary>
    /// This test checks if the launcher's angle is adjusted properly.
    /// </summary>
    /// <param name="moveStep">The amount to adjust the launcher's angle by.</param>
    [Test, Description("Checks if the launcher's angle is adjusted correctly.")]
    public void CheckAdjustAngle([Values((float)-Math.PI / 16, 0, (float)Math.PI / 16)] float moveStep) {
      // Create a launcher with a ball in it.
      Launcher launcher = CreateLauncher(true);

      // Actually move the launcher.
      launcher.AdjustAngle(moveStep);

      /* For floating point numbers, NUnit has a setting known as 
       * GlobalSettings.DefaultFloatingPointTolerance that it uses when comparing floating
       * point numbers, which takes into account the precision problems that floating point
       * numbers often have. */

      // Test to see that we get the expected result.
      float expectedResult = initialAngle + moveStep;
      Console.WriteLine("Comparing " + launcher.Angle + " to " + expectedResult);
      Assert.AreEqual(expectedResult, launcher.Angle);
    }

    /// <summary>
    /// This test checks if the launcher's angle is kept between 0 and 180 degrees.
    /// </summary>
    [Test, Description("Checks if the launcher's angle stays between 0 and 180 degrees.")]
    public void CheckAngleBounds() {
      // Create a launcher with a ball in it.
      Launcher launcher = CreateLauncher(true);

      // Move the launcher by a really large value.
      launcher.AdjustAngle(-10000000f);

      /* For floating point numbers, NUnit has a setting known as 
       * GlobalSettings.DefaultFloatingPointTolerance that it uses when comparing floating
       * point numbers, which takes into account the precision problems that floating point
       * numbers often have. */

      // Test to see that the launcher hasn't went below 0 degrees.
      Assert.GreaterOrEqual(lowestAngle, launcher.Angle);

      // Move the launcher the other way.
      launcher.AdjustAngle(10000000f);

      // Test to see that the launcher hasn't went above 180 degrees.
      Assert.LessOrEqual(highestAngle, launcher.Angle);
    }

    /// <summary>
    /// This test checks if the launcher's magnitude is being adjusted correctly.
    /// </summary>
    /// <param name="magnitudeStep"></param>
    [Test, Description("Checks if the launcher's magnitude is adjusted correctly.")]
    public void CheckAdjustMagnitude([Values(-1f, 0, 1f)] float magnitudeStep) {
      // Create a launcher with a ball in it.
      Launcher launcher = CreateLauncher(true);

      // Actually adjust the launcher's magnitude.
      launcher.AdjustMagnitude(magnitudeStep);

      /* For floating point numbers, NUnit has a setting known as 
       * GlobalSettings.DefaultFloatingPointTolerance that it uses when comparing floating
       * point numbers, which takes into account the precision problems that floating point
       * numbers often have. */

      // Test to see that we get the expected result.
      float expectedResult = initialMagnitude + magnitudeStep;
      Console.WriteLine("Comparing " + launcher.Magnitude + " to " + expectedResult);
      Assert.AreEqual(expectedResult, launcher.Magnitude);
    }

    /// <summary>
    /// This test checks if the launcher's magnitude is kept within the limits.
    /// </summary>
    [Test, Description("Checks if the launcher's magnitude is kept within the limits.")]
    public void CheckMagnitudeBounds() {
      // Create a launcher with a ball in it.
      Launcher launcher = CreateLauncher(true);

      // Adjust the launcher's magnitude by a really large value.
      launcher.AdjustMagnitude(-10000000f);

      /* For floating point numbers, NUnit has a setting known as 
       * GlobalSettings.DefaultFloatingPointTolerance that it uses when comparing floating
       * point numbers, which takes into account the precision problems that floating point
       * numbers often have. */

      // Test to see that the launcher hasn't went below the minimum magnitude.
      Assert.GreaterOrEqual(lowestMagnitude, launcher.Magnitude);

      // Move the launcher the other way.
      launcher.AdjustMagnitude(10000000f);

      // Test to see that the launcher hasn't went above the maximum magnitude.
      Assert.LessOrEqual(highestMagnitude, launcher.Magnitude);
    }

    /// <summary>
    /// This test checks if the Launcher class throws exceptions when given garbage position vectors,
    /// and it also checks if the Launcher class does not throw any false exceptions.
    /// </summary>
    [Test, Description("Checks to see if the Launcher class validates its input for the position argument.")]
    public void ValidatePosition() {
      Assert.Catch<ArgumentException>(GarbageInput);
      Assert.DoesNotThrow(OkayInput);
    }

    /// <summary>
    /// Creates a launcher and passes in bad coordinates for its position.
    /// </summary>
    private void GarbageInput() {
      Launcher launcher = new Launcher(null, new Vector2(-1491471, -2523953));
    }

    /// <summary>
    /// Creates a launcher with a valid position.
    /// </summary>
    private void OkayInput() {
      Launcher launcher = new Launcher(null, new Vector2(200, 300));
    }

    /* Put more tests here */

    /// <summary>
    /// Creates and returns a launcher.
    /// </summary>
    /// <param name="createBall">Whether a ball should be loaded in the launcher.</param>
    /// <returns>A Launcher object.</returns>
    private Launcher CreateLauncher(bool createBall) {
      // Create the launcher. 
      // We don't need a texture for unit tests, so pass in null for the texture parameter.
      Launcher launcher = new Launcher(null, new Vector2(0, 0));

      if (createBall) {
        // Give the launcher a ball; otherwise, it won't move.
        Ball ball = new Ball(null);
        launcher.LoadBall(ball);
      }

      return launcher;
    }
  }
}
