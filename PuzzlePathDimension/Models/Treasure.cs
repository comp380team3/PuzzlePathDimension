using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Treasure class contains a treasure's data.
  /// </summary>
  class Treasure {
    /// <summary>
    /// The hard-coded height of a treasure, in pixels.
    /// </summary>
    private const int _height = 30;
    /// <summary>
    /// The hard-coded width of a treasure, in pixels.
    /// </summary>
    private const int _width = 30;

    /// <summary>
    /// The texture that the treasure will be drawn with.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// The texture's color data.
    /// </summary>
    private Color[] _colorData;

    /// <summary>
    /// The position of the treasure.
    /// </summary>
    private Vector2 _position;

    /// <summary>
    /// Whether the treasure is active.
    /// </summary>
    private bool _active;

    /// <summary>
    /// Gets the position of the treasure.
    /// </summary>
    public Vector2 Position {
      get { return _position; }
    }

    /// <summary>
    /// Gets the height of the treasure object.
    /// </summary>
    public int Height {
      get { return _height; }
    }

    /// <summary>
    /// Gets the width of the treasure object.
    /// </summary>
    public int Width {
      get { return _width; }
    }

    /// <summary>
    /// Gets whether the ball is active.
    /// </summary>
    public bool Active {
      get { return _active; }
    }

    /// <summary>
    /// Initializes a treasure.
    /// </summary>
    /// <param name="texture">The texture to draw the treasure with.</param>
    /// <param name="position">The position of the texture.</param>
    public void Initialize(Texture2D texture, Vector2 position) {
      _texture = texture;
      _position = position;

      _active = true;

      // Get the texture's color data, which is used for per-pixel collision
      _colorData = new Color[_texture.Width * _texture.Height];
      _texture.GetData<Color>(_colorData);

      // Check to make sure that the visual representation of the texture is actually the right
      // size, and print a warning to the console if that isn't the case.
      if (_texture != null && _texture.Width * _texture.Height != _width * _height) {
        Console.WriteLine("Warning: the treasure's texture does not have the expected dimensions.");
        Console.WriteLine("Expected: " + _width + ", " + _height);
        Console.WriteLine("...but the texture is: " + _texture.Width + ", " + _texture.Height);
      }
    }

    /// <summary>
    /// Updates the texture's state.
    /// </summary>
    public void Update() {

    }

    /// <summary>
    /// Marks a treasure as being collected.
    /// </summary>
    public void Collect() {
      _active = false;
    }

    /// <summary>
    /// Draws the treasure to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the treasure.</param>
    public void Draw(SpriteBatch spriteBatch) {
      if (_active) {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      }
    }
  }
}
