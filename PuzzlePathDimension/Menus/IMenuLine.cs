using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  interface IMenuLine {
    int Draw(SpriteBatch spriteBatch, GraphicsCursor cursor, GameTime gameTime);
  }

  class Spacer : IMenuLine {
    public int Height { get; private set; }

    public Spacer(int height) {
      Height = height;
    }

    public int Draw(SpriteBatch spriteBatch, GraphicsCursor cursor, GameTime gameTime) {
      return Height;
    }
  }

  class TextLine : IMenuLine {
    public Color Color { get; set; }
    private SpriteFont Font { get; set; }
    private string Text { get; set; }

    public float Scale { get; set; }

    public int Width {
      get {
        return (int)(Font.MeasureString(Text).X * Scale);
      }
    }

    public int Height {
      get {
        return (int)(Font.MeasureString(Text).Y * Scale);
      }
    }

    public TextLine(string text, SpriteFont font, Color textColor, float scale = 1.0f) {
      Text = text;
      Color = textColor;
      Font = font;
      Scale = scale;
    }

    public int Draw(SpriteBatch spriteBatch, GraphicsCursor cursor, GameTime gameTime) {
      Vector2 origin = new Vector2(0, Font.MeasureString(Text).Y / 2);

      cursor = (new OffsetEffect(-Width / 2, 0)).ApplyTo(cursor);

      Color color = Color * cursor.Alpha;
      spriteBatch.DrawString(Font, Text, cursor.Position, color, 0,
                                origin, Scale, SpriteEffects.None, 0);

      return Height;
    }
  }
}