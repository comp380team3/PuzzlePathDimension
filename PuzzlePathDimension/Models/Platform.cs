using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Platform class describes a platform.
  /// </summary>
  public class Platform {
    /// <summary>
    /// The texture that the platform uses.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// Whether the platform is visible.
    /// </summary>
    private bool _visible;

    /// <summary>
    /// Whether the platform is breakable.
    /// </summary>
    private bool _breakable;

    /// <summary>
    /// The upper-left corner of the platform, in pixels.
    /// </summary>
    private Vector2 _origin;

    /// <summary>
    /// Gets the upper-left corner of the platform, in pixels.
    /// </summary>
    public Vector2 Origin {
      get { return _origin; }
      set {
        _origin = value;
        // Moving the platform also moves its center, so figure out the position of
        // the new center.
        _center = CalculateCenter();

        // Reposition the Body, but only if it has been initialized.
        if (_body != null) {
          _body.Position = UnitConverter.ToMeters(_center);
        }
      }
    }

    /************************
     * Brian's Physics stuff*
     * *********************/

    /// <summary>
    /// The Body that represents this platform in the physics simulation.
    /// </summary>
    private Body _body;

    /// <summary>
    /// The center of the platform, in pixels.
    /// </summary>
    private Vector2 _center;

    /// <summary>
    /// Gets the center of the platform, in pixels.
    /// </summary>
    public Vector2 Center {
      get { return _center; }
    }

    /// <summary>
    /// The size of the platform, in pixels.
    /// </summary>
    private Vector2 _size;

    /// <summary>
    /// Gets or sets the size of the platform, in pixels.
    /// </summary>
    public Vector2 Size {
      get { return _size; }
      set { 
        _size = value;
        // Changing the platform's size also moves its center, so figure out 
        // the position of the new center.
        _center = CalculateCenter();

        // If there is a Body, we pretty much have to recreate it now...
        if (_body != null) {
          // TODO: do all sorts of stuff
          throw new NotImplementedException("I haven't determined how to code that yet. -Jorenz");
        }
      }
    }

    /// <summary>
    /// Gets whether the platform is visible.
    /// </summary>
    public bool Visible {
      get { return _visible; }
      // TODO: move the setter into some sort of Break() method.
      set { _visible = value; }
    }

    /// <summary>
    /// Gets whether the platform is breakable.
    /// </summary>
    public bool Breakable {
      get { return _breakable; }
    }

    /// <summary>
    /// Constructs a Platform object.
    /// </summary>
    /// <param name="texture">The texture to draw the platform with.</param>
    /// <param name="position">The upper-left corner of the platform, in pixels.</param>
    /// <param name="size">The size of the platform, in pixels.</param>
    /// <param name="breakable">Whether the platform should be breakable.</param>
    public Platform(Texture2D texture, Vector2 position, Vector2 size, bool breakable) {
      // Determine the positional and size data of the platform.
      _origin = position;
      _size = size;
      _center = CalculateCenter();

      // Set the platform to be visible.
      _texture = texture;
      _visible = true;

      // Set whether the platform is a breakable one.
      _breakable = breakable;

      // Leave the Body object uninitialized until a World object comes by to initialize it.
      _body = null;
    }

    /// <summary>
    /// Calculates the center of the platform.
    /// </summary>
    /// <returns>The position of the center of the platform.</returns>
    private Vector2 CalculateCenter() {
      Vector2 center = new Vector2();
      center.X = _origin.X + (_size.X / 2.0f);
      center.Y = _origin.Y + (_size.Y / 2.0f);
      return center;
    }

    /// <summary>
    /// Initializes the platform's Body object.
    /// </summary>
    /// <param name="world">The World object that the ball will be a part of.</param>
    public void InitBody(World world) {
      if (_body != null) {
        throw new InvalidOperationException("There is already a Body object for this platform.");
      }

      // Get the size of the platform, in meters.
      Vector2 meterSize = UnitConverter.ToMeters(_size);

      // Create the Body object.
      _body = BodyFactory.CreateRectangle(world, meterSize.X, meterSize.Y, 1);
      // Set its position to be the center of the platform, in meters, which is what the
      // physics engine expects.
      _body.Position = UnitConverter.ToMeters(_center);
      // The platform should never be subjected to the World's physical forces.
      _body.BodyType = BodyType.Static;
      // Set other properties of the Platform's body.
      _body.Friction = 0f;
      _body.Restitution = .8f;
    }

    /*****************************
     * Brian's Physics stuff ends*
     * **************************/

    /// <summary>
    /// Draws the platform to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      if (_visible) {
        // Scale the texture to the appropriate size.
        Vector2 scale = new Vector2(_size.X / (float)_texture.Width, _size.Y / (float)_texture.Height);
        // Draw it!
        // TODO: replace hard-coded origin
        spriteBatch.Draw(_texture, _center, null, Color.White, 0f, new Vector2(10, 10), scale, SpriteEffects.None, 0f);
      }
    }

    /// <summary>
    /// Checks if the origin of the platform is within the level boundaries.
    /// </summary>
    /// <param name="v">The origin.</param>
    /// <returns>Whether the origin of the platform is inside the level.</returns>
    private bool InBounds(Vector2 v) {
      // Subtract 1 to account for the fact that the origin is at (0,0).
      return v.X >= 0 && v.X <= Simulation.FieldWidth - 1 && v.Y >= 0 && v.Y <= Simulation.FieldHeight - 1;
    }
  }
}
