using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  interface IMenuLine {
    int Draw(SpriteBatch spriteBatch, Vector2 pos, GameTime gameTime);

    int Height { get; }
    int Width { get; }
  }

  class Spacer : IMenuLine {
    public int Height { get; private set; }

    public int Width {
      get {
        return 0;
      }
    }

    public Spacer(int height) {
      Height = height;
    }

    public int Draw(SpriteBatch spriteBatch, Vector2 pos, GameTime gameTime) {
      return Height;
    }
  }

  class TextLine : IMenuLine {
    public Color TextColor { get; set; }
    private SpriteFont Font { get; set; }
    private string Text { get; set; }

    public int Width {
      get {
        return (int)Font.MeasureString(Text).X;
      }
    }

    public int Height {
      get {
        return (int)Font.MeasureString(Text).Y;
      }
    }

    public TextLine(string text, SpriteFont font, Color textColor) {
      Text = text;
      TextColor = textColor;
      Font = font;
    }

    public int Draw(SpriteBatch spriteBatch, Vector2 pos, GameTime gameTime) {
      Vector2 origin = new Vector2(0, Font.MeasureString(Text).Y / 2);

      spriteBatch.DrawString(Font, Text, pos, TextColor, 0,
                                origin, 1.25f, SpriteEffects.None, 0);
      return Font.LineSpacing;
    }
  }
}