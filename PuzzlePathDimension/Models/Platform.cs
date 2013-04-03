﻿using System;
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

    /************************
     * Brian's Physics stuff*
     * *********************/
    public const float unitToPixel = 100.0f;
    public const float pixelToUnit = 1 / unitToPixel;
  
    public Body body;
    public Vector2 Position {
      get { return body.Position * unitToPixel; }
      set { body.Position = value * pixelToUnit; }
    }
    private Vector2 size;
    public Vector2 Size {
      get { return size * unitToPixel; }
      set { size = value * pixelToUnit; }
    }

    public Platform(World world, Texture2D texture, Vector2 size, float mass, Vector2 position) {
      // Get the center of the rectangle, which the physics engine needs.
      Vector2 center = new Vector2();
      center.X = position.X + (size.X / 2.0f);
      center.Y = position.Y + (size.Y / 2.0f);

      body = BodyFactory.CreateRectangle(world, size.X * pixelToUnit, size.Y * pixelToUnit, 1);
      body.BodyType = BodyType.Static;
      body.Friction = 0f;
      body.Restitution = .8f;
      // This Position field is actually body.Position, which is expected to be the center, 
      // and not the upper left corner of the platform. Perhaps we can rewrite parts of the class 
      // to make this distinction clearer? - Jorenz
      Position = center; 
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
      Vector2 drawPos = new Vector2(Position.X - (Size.X / 2.0f), Position.Y - (Size.Y / 2.0f));

      // Scale the texture to the appropriate size.
      Vector2 scale = new Vector2(Size.X / (float)_texture.Width, Size.Y / (float)_texture.Height);
      // Draw it!
      spriteBatch.Draw(_texture, drawPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Checks if the origin of the platform is within the level boundaries.
    /// </summary>
    /// <param name="v">The origin.</param>
    /// <returns>Whether the origin of the platform is inside the level.</returns>
    private bool InBounds(Vector2 v) {
      // It's probably best if these numbers aren't hard-coded. -Jorenz
      return v.X >= 0 && v.X <= 799 && v.Y >= 0 && v.Y <= 599;
    }
  }
}
