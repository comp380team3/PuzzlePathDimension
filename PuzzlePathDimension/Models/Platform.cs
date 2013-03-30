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
    private Vector2 _origin;
    /*
    /// <summary>
    /// The texture's color data.
    /// </summary>
    private Color[] _colorData;

    /// <summary>
    /// The pixel coordinates of the upper left corner of the platform.
    /// </summary>
    private Vector2 _upperLeftCorner;
    /// <summary>
    /// The pixel coordinates of the lower right corner of the platform.
    /// </summary>
    private Vector2 _lowerRightCorner;

    /// <summary>
    /// Whether the platform is active.
    /// </summary>
    private bool _active;
    /// <summary>
    /// Gets the position, which is the upper-left corner, of the platform.
    /// </summary>
   /* public Vector2 Position {
      get { return _upperLeftCorner; }
    }
    
    /// <summary>
    /// Gets the pixel coordinates of the upper-left corner of the platform.
    /// </summary>
    public Vector2 UpperLeftCorner {
      get { return _upperLeftCorner; }
    }
    */

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
      body = BodyFactory.CreateRectangle(world, size.X * pixelToUnit, size.Y * pixelToUnit, 1);
      body.BodyType = BodyType.Static;
      _origin = new Vector2((texture.Width / 2.0f), (texture.Height/2.0f));
      body.Friction = 0f;
      body.Restitution = .8f;
      Position = position; 
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
      // Scale the texture appropriately to the platform's size.
      Console.WriteLine("body: "+body.Position);
      Console.WriteLine(Position);
      Console.WriteLine(_origin);
      Console.WriteLine(Size);
      Console.WriteLine();


      // Draw it!
      Vector2 scale = new Vector2(Size.X / (float)_texture.Width, Size.Y / (float)_texture.Height);
      spriteBatch.Draw(_texture, Position, null, Color.White, 0f, _origin, 1f, SpriteEffects.None, 0f);
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
