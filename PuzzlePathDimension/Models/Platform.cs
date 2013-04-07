using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Platform class describes a platform.
  /// </summary>
  class Platform {
    /// <summary>
    /// The texture that the platform uses.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// Whether the platform is active.
    /// </summary>
    private bool _active;

    private bool _breakable;

    /************************
     * Brian's Physics stuff*
     * *********************/
    public Body body;

    /// <summary>
    /// Gets or sets the center of the platform.
    /// </summary>
    public Vector2 Center {
      get { return UnitConverter.ToPixels(body.Position); }
      set { body.Position = UnitConverter.ToMeters(value); }
    }
    private Vector2 size;
    public Vector2 Size {
      get { return UnitConverter.ToPixels(size); }
      set { size = UnitConverter.ToMeters(value); }
    }

    /// <summary>
    /// Gets whether the platform is active.
    /// </summary>
    public bool Active {
      get { return _active; }
      set { _active = value; }
    }

    /// <summary>
    /// Gets whether the platform is breakable.
    /// </summary>
    public bool Breakable {
      get { return _breakable; }
    }

    public Platform(World world, Texture2D texture, Vector2 size, Vector2 position) {
      // Get the center of the rectangle, which the physics engine needs.
      Vector2 center = new Vector2();
      center.X = position.X + (size.X / 2.0f);
      center.Y = position.Y + (size.Y / 2.0f);

      body = BodyFactory.CreateRectangle(world, UnitConverter.ToMeters(size.X), 
        UnitConverter.ToMeters(size.Y), 1);
      body.BodyType = BodyType.Static;
      body.Friction = 0f;
      body.Restitution = .8f;
      // This Position field is actually body.Position, which is expected to be the center, 
      // and not the upper left corner of the platform. Perhaps we can rewrite parts of the class 
      // to make this distinction clearer? - Jorenz
      Center = center; 
      this.Size = size;
      this._texture = texture;
    }
    /*****************************
     * Brian's Physics stuff ends*
     * **************************/

    /// <summary>
    /// Draws the platform to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      // Get the upper-left corner of the rectangle.
      // Vector2 drawPos = new Vector2(Center.X - (Size.X / 2.0f), Center.Y - (Size.Y / 2.0f));

      // Scale the texture to the appropriate size.
      Vector2 scale = new Vector2(Size.X / (float)_texture.Width, Size.Y / (float)_texture.Height);
      // Draw it!
      // TODO: replace hard-coded origin
      spriteBatch.Draw(_texture, Center, null, Color.White, 0f, new Vector2(10, 10), scale, SpriteEffects.None, 0f);
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
