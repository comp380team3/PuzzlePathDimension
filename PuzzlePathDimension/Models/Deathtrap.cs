using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Deathtrap class contains a death trap's data.
  /// </summary>
  class DeathTrap {
    /// <summary>
    /// The hard-coded height of the death trap.
    /// </summary>
    private const int _height = 60;
    /// <summary>
    /// The hard-coded width of the death trap.
    /// </summary>
    private const int _width = 60;

    /// <summary>
    /// The texture that the death trap will be drawn with.
    /// </summary>
    private Texture2D _texture;
    /// <summary>
    /// The texture's color data.
    /// </summary>
    private Color[] _colorData;

    /// <summary>
    /// The position of the death trap.
    /// </summary>
    private Vector2 _position;

    /// <summary>
    /// Whether the death trap is active.
    /// </summary>
    private bool _active;
    
    /// <summary>
    /// Gets the death trap's position.
    /// </summary>
    public Vector2 Position {
      get { return _position; }
    }

    /// <summary>
    /// Gets the death trap's height.
    /// </summary>
    public int Height {
      get { return _height; }
    }

    /// <summary>
    /// Gets the death trap's width.
    /// </summary>
    public int Width {
      get { return _width; }
    }

    /// <summary>
    /// Whether the death trap is active.
    /// </summary>
    public bool Active {
      get { return _active; }
    }

    /// <summary>
    /// Constructs a DeathTrap object.
    /// </summary>
    /// <param name="texture">The texture that the death trap will be drawn with.</param>
    /// <param name="position">The position of the death trap.</param>
    public DeathTrap(Texture2D texture, Vector2 position) {
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

    public void Update() {

    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
  }
}
