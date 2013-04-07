using System;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// The UnitConverter class provides methods to handle conversions between
  /// meters, which is what the physics engine expects, and pixels, which is
  /// what the SpriteBatch API expects.
  /// </summary>
  public static class UnitConverter {
    /// <summary>
    /// The conversion factor used to convert from meters to pixels.
    /// </summary>
    public static readonly float MeterToPixelRatio = 100.0f;
    /// <summary>
    /// The converstion factor used to convert from pixels to meters.
    /// </summary>
    public static readonly float PixelToMeterRatio = 1 / MeterToPixelRatio;

    /// <summary>
    /// Converts a number from pixels to meters.
    /// </summary>
    /// <param name="pixels">The value in pixels.</param>
    /// <returns>The equivalent value in meters.</returns>
    public static float ToMeters(float pixels) {
      return pixels * PixelToMeterRatio;
    }

    /// <summary>
    /// Converts a number from meters to pixels.
    /// </summary>
    /// <param name="meters">The value in meters.</param>
    /// <returns>The equivalent value in pixels.</returns>
    public static float ToPixels(float meters) {
      return meters * MeterToPixelRatio;
    }

    /// <summary>
    /// Converts a two-dimensional vector from pixels to meters.
    /// </summary>
    /// <param name="pixels">The vector in pixels.</param>
    /// <returns>The vector in meters.</returns>
    public static Vector2 ToMeters(Vector2 pixels) {
      return pixels * PixelToMeterRatio;
    }

    /// <summary>
    /// Converts a two-dimensional vector from meters to pixels.
    /// </summary>
    /// <param name="meters">The vector in meters.</param>
    /// <returns>The vector in pixels.</returns>
    public static Vector2 ToPixels(Vector2 meters) {
      return meters * MeterToPixelRatio;
    }

  }
}
