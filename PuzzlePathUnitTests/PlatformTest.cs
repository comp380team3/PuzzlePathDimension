using System;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using PuzzlePathDimension;

namespace PuzzlePathUnitTests {
  /// <summary>
  /// The PlatformTest class tests the functionality of the Platform class.
  /// </summary>
  [TestFixture, Description("Tests the functionality of the Platform class.")]
  public class PlatformTest {
    /// <summary>
    /// This test checks if the center is being calculated properly.
    /// </summary>
    [Test, Description("Checks if the center is being calculated properly.")]
    public void CheckCenterCalculation() {
      // Create a platform at (100, 100) with a width of 100 and a height of 200.
      Vector2 position = new Vector2(100f, 100f);
      Vector2 size = new Vector2(100f, 200f);
      Platform plat = new Platform(null, position, size, false);

      // We know what the answers should be...
      Vector2 expected = new Vector2(150f, 200f);

      // ...so make sure that the center is calculated correctly.
      Console.WriteLine("Comparing " + plat.Center.X + " to " + expected.X);
      Console.WriteLine("Comparing " + plat.Center.Y + " to " + expected.Y);
      Assert.IsTrue(expected.Equals(plat.Center));
    }

    /// <summary>
    /// This test checks if platforms are moved properly. In this test, InitBody() is
    /// not called.
    /// </summary>
    [Test, Description("Checks if platforms are moved properly. InitBody() is not called.")]
    public void CheckMovePlatformNoBody() {
      // Create a platform at (100, 100) with a width of 100 and a height of 100.
      Vector2 position = new Vector2(100f, 100f);
      Vector2 size = new Vector2(100f, 100f);
      Platform plat = new Platform(null, position, size, false);

      // We know what the new position should be, so change the platform's position...
      Vector2 newPos = new Vector2(200f, 300f);
      plat.Origin = new Vector2(newPos.X, newPos.Y);

      // ...and check to see if it actually moved.
      Console.WriteLine("Comparing " + plat.Origin.X + " to " + newPos.X);
      Console.WriteLine("Comparing " + plat.Origin.Y + " to " + newPos.Y);
      Assert.IsTrue(newPos.Equals(plat.Origin));

      // This is supposed to be the new center of the platform, now that it moved...
      Vector2 expectedCenter = new Vector2(250f, 350f);

      // .. so check if that's the case.
      Console.WriteLine("Comparing " + plat.Center.X + " to " + expectedCenter.X);
      Console.WriteLine("Comparing " + plat.Center.Y + " to " + expectedCenter.Y);
      Assert.IsTrue(expectedCenter.Equals(plat.Center));
    }

    /// <summary>
    /// This test checks if platforms can be resized properly. In this test, InitBody()
    /// is not called.
    /// </summary>
    [Test, Description("Checks if platforms are resized properly. InitBody() is not called.")]
    public void CheckResizePlatformNoBody() {
      // Create a platform at (100, 100) with a width of 100 and a height of 100.
      Vector2 position = new Vector2(100f, 100f);
      Vector2 size = new Vector2(100f, 100f);
      Platform plat = new Platform(null, position, size, false);

      // We know what the new size should be, so change the size of the platform...
      Vector2 newSize = new Vector2(200f, 50f);
      plat.Size = new Vector2(newSize.X, newSize.Y);

      // ...and check to see if it actually resized.
      Console.WriteLine("Comparing " + plat.Size.X + " to " + newSize.X);
      Console.WriteLine("Comparing " + plat.Size.Y + " to " + newSize.Y);
      Assert.IsTrue(newSize.Equals(plat.Size));

      // Check if the position changed (it shouldn't).
      Vector2 expectedPos = new Vector2(100f, 100f);
      Console.WriteLine("Comparing " + plat.Origin.X + " to " + expectedPos.X);
      Console.WriteLine("Comparing " + plat.Origin.Y + " to " + expectedPos.Y);
      Assert.IsTrue(expectedPos.Equals(plat.Origin));

      // Check if the center was re-calculated correctly.
      Vector2 expectedCenter = new Vector2(200f, 125f);
      Console.WriteLine("Comparing " + plat.Center.X + " to " + expectedCenter.X);
      Console.WriteLine("Comparing " + plat.Center.Y + " to " + expectedCenter.Y);
      Assert.IsTrue(expectedCenter.Equals(plat.Center));
    }

    /// <summary>
    /// This test checks if the Platform constructor validates any input given to it.
    /// </summary>
    [Test, Description("This test checks if the Platform constructor validates any input given to it.")]
    public void ValidateConstructorParameters() {
      Assert.Catch<ArgumentException>(GarbagePosition);
      Assert.Catch<ArgumentException>(GarbageSize);
      Assert.DoesNotThrow(OkayInput);
    }

    // TODO: add tests that involve the Body object

    /// <summary>
    /// Creates a platform with an invalid position.
    /// </summary>
    private void GarbagePosition() {
      Vector2 position = new Vector2(-23423124, -54646);
      Vector2 size = new Vector2(100, 100);
      Platform plat = new Platform(null, position, size, false);
    }

    /// <summary>
    /// Creates a platform with an invalid size.
    /// </summary>
    private void GarbageSize() {
      Vector2 position = new Vector2(100, 100);
      Vector2 size = new Vector2(-87623, -466043);
      Platform plat = new Platform(null, position, size, false);
    }

    /// <summary>
    /// Creates a platform with valid parameters.
    /// </summary>
    private void OkayInput() {
      Vector2 position = new Vector2(200, 300);
      Vector2 size = new Vector2(100, 40);
      Platform plat = new Platform(null, position, size, false);
    }
  }
}
