using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  struct GraphicsCursor {
    public float X { get; set; }
    public float Y { get; set; }
    public float Scaling { get; set; }

    public Color TextColor { get; set; }
    public SpriteFont Font { get; set; }

    float alpha;
    public float Alpha {
      get {
        return alpha;
      }

      set {
        alpha = MathHelper.Clamp(value, 0.0f, 1.0f);
      }
    }


    /// <summary>
    /// The X and Y coordinates as a Vector2.
    /// </summary>
    public Vector2 Position {
      get { return new Vector2(X, Y); }
      set { X = value.X; Y = value.Y; }
    }

    public Color BlendedTextColor() {
      return TextColor * Alpha;
    }
  }
}