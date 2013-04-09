using System;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using PuzzlePathDimension;

namespace PuzzlePathUnitTests {
  /// <summary>
  /// The UnitConvertTest tests the methods of the UnitConverter class.
  /// </summary>
  [TestFixture, Description("Tests the methods of the UnitConverter class.")]
  public class UnitConvertTest {
    /// <summary>
    /// The conversion factor that should be used to convert from meters to pixels.
    /// </summary>
    private const float CorrectMeterToPixelRatio = 100.0f;
    /// <summary>
    /// The conversion factor that should be used to convert from pixels to meters.
    /// </summary>
    private const float CorrectPixelToMeterRatio = 1 / CorrectMeterToPixelRatio;

    /* The [SetUp] and [Teardown] methods are omitted since I don't
     * need them. - Jorenz */

    /// <summary>
    /// This test checks if floating-point pixel values are converted to meters correctly.
    /// </summary>
    /// <param name="f">The number, in pixels, to convert to meters.</param>
    [Test, Description("Checks if floating-point pixel values are converted to meters correctly.")]
    public void PixelFloatsToMeters([Range(0f, 1000f, 200f)] float f) {
      float expectedResult = f * CorrectPixelToMeterRatio;
      Assert.AreEqual(expectedResult, UnitConverter.ToMeters(f));
    }

    /// <summary>
    /// This test checks if floating-point meter values are converted to pixels correctly.
    /// </summary>
    /// <param name="f">The number, in meters, to convert to pixels.</param>
    [Test, Description("Checks if floating-point meter values are converted to pixels correctly.")]
    public void MeterFloatsToPixels([Range(0f, 1000f, 200f)] float f) {
      float expectedResult = f * CorrectMeterToPixelRatio;
      Assert.AreEqual(expectedResult, UnitConverter.ToPixels(f));
    }

    /// <summary>
    /// This test checks if vectors in pixel coordinates are converted to vectors 
    /// in meter coordinates correctly.
    /// </summary>
    /// <param name="x">The x-component of the vector, in pixels.</param>
    /// <param name="y">The y-component of the vector, in pixels.</param>
    [Test, Description("Checks if vectors in pixel coordinates are converted to vectors in meter coordinates correctly.")]
    public void PixelVectorsToPixels([Range(0f, 1000f, 200f)] float x,
      [Range(0f, 1000f, 200f)] float y) {
      float expectedX = x * CorrectPixelToMeterRatio;
      float expectedY = y * CorrectPixelToMeterRatio;

      Vector2 result = UnitConverter.ToMeters(new Vector2(x, y));
      Assert.AreEqual(expectedX, result.X);
      Assert.AreEqual(expectedY, result.Y);
    }

    /// <summary>
    /// This test checks if vectors in meter coordinates are converted to vectors 
    /// in pixel coordinates correctly.
    /// </summary>
    /// <param name="x">The x-component of the vector, in meters.</param>
    /// <param name="y">The y-component of the vector, in meters.</param>
    [Test, Description("Checks if vectors in meter coordinates are converted to vectors in pixel coordinates correctly.")]
    public void MeterVectorsToPixels([Range(0f, 1000f, 200f)] float x,
      [Range(0f, 1000f, 200f)] float y) {
      float expectedX = x * CorrectMeterToPixelRatio;
      float expectedY = y * CorrectMeterToPixelRatio;

      Vector2 result = UnitConverter.ToPixels(new Vector2(x, y));
      Assert.AreEqual(expectedX, result.X);
      Assert.AreEqual(expectedY, result.Y);
    }
  }
}
