using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  enum TextAlignment { CENTER = 0, LEFT, RIGHT };

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

    public TextAlignment Align { get; set; }

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
      Vector2 origin = Font.MeasureString(Text) / 2;

      if (Align == TextAlignment.RIGHT)
        cursor = (new OffsetEffect(Width / 2, 0)).ApplyTo(cursor);
      else if (Align == TextAlignment.LEFT)
        cursor = (new OffsetEffect(-Width / 2, 0)).ApplyTo(cursor);

      Color color = Color * cursor.Alpha;
      spriteBatch.DrawString(Font, Text, cursor.Position, color, 0,
                                origin, Scale, SpriteEffects.None, 0);

      return Height;
    }
  }

  class ImageMenuLine : IMenuLine {
    public Texture2D Image { get; set; }
    public IMenuLine Line { get; set; }

    public ImageMenuLine(Texture2D image, IMenuLine menuLine) {
      Image = image;
      Line = menuLine;
    }

    public int Draw(SpriteBatch spriteBatch, GraphicsCursor cursor, GameTime gameTime) {
      spriteBatch.Draw(Image, cursor.Position, Color.White);

      cursor.Y += Image.Height / 2;
      cursor.X -= 20;

      return 10 + Math.Max(Image.Height, Line.Draw(spriteBatch, cursor, gameTime));
    }
  }
}