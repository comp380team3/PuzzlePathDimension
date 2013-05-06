using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics.Contacts;
using System.Collections.Generic;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Platform class describes a platform.
  /// </summary>
  public class Platform : ILevelObject {
    /// <summary>
    /// The hard-coded dictionary that maps platform dimensions to their respective textures. This
    /// dictionary is for normal platforms.
    /// </summary>
    /// <remarks>
    /// There is no read-only dictionary in .NET 4.0, so please don't modify this! The "readonly"
    /// keyword only affects the reference to the dictionary, not the dictionary itself.
    /// </remarks>
    public static readonly Dictionary<Vector2, string> NormalPlatNames = new Dictionary<Vector2, string>
    {
      {new Vector2(100, 25), "Texture/SHorizontalPlatform"},
      {new Vector2(150, 25), "Texture/MHorizontalPlatform"},
      {new Vector2(200, 25), "Texture/LHorizontalPlatform"},
      {new Vector2(25, 100), "Texture/SVerticalPlatform"},
      {new Vector2(25, 150), "Texture/MVerticalPlatform"},
      {new Vector2(25, 200), "Texture/LVerticalPlatform"}
    };

    /// <summary>
    /// The hard-coded dictionary that maps platform dimensions to their respective textures. This
    /// dictionary is for breakable platforms.
    /// </summary>
    /// <remarks>
    /// There is no read-only dictionary in .NET 4.0, so please don't modify this! The "readonly"
    /// keyword only affects the reference to the dictionary, not the dictionary itself.
    /// </remarks>
    public static readonly Dictionary<Vector2, string> BreakablePlatNames = new Dictionary<Vector2, string>
    {
      {new Vector2(100, 25), "Texture/SHorizontalPlatformBreakable"},
      {new Vector2(150, 25), "Texture/MHorizontalPlatformBreakable"},
      {new Vector2(200, 25), "Texture/LHorizontalPlatformBreakable"},
      {new Vector2(25, 100), "Texture/SVerticalPlatformBreakable"},
      {new Vector2(25, 150), "Texture/MVerticalPlatformBreakable"},
      {new Vector2(25, 200), "Texture/LVerticalPlatformBreakable"}
    };

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

        // If there is a Body, then things get too complicated. Sorry! - Jorenz
        if (_body != null) {
          throw new NotSupportedException("Setting the Size of a Platform while the Body is active is not supported.");
        }
      }
    }

    /// <summary>
    /// Gets the height of the platform.
    /// </summary>
    public int Height {
      get { return (int)_size.Y; }
    }

    /// <summary>
    /// Gets the width of the platform.
    /// </summary>
    public int Width {
      get { return (int)_size.X; }
    }

    /// <summary>
    /// Gets whether the platform is visible.
    /// </summary>
    public bool Visible {
      get { return _visible; }
    }

    /// <summary>
    /// Gets whether the platform is breakable.
    /// </summary>
    public bool Breakable {
      get { return _breakable; }
    }

    /// <summary>
    /// This delegate is called when the ball bounces off something solid.
    /// </summary>
    public delegate void PlatformTouched(bool breakable);
    /// <summary>
    /// Occurs when the ball bounces off something solid.
    /// </summary>
    public event PlatformTouched OnPlatformCollision;

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

      if (position.X < 0 || position.Y < 0) {
        throw new ArgumentOutOfRangeException("The position should not be negative.");
      }
      if (size.X < 1 || size.Y < 1) {
        throw new ArgumentOutOfRangeException("Both compoments of the size vector need to be at least 1.");
      }

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
    /// <param name="world">The World object that the platform will be a part of.</param>
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
      // Associate the Body object with the platform.
      _body.UserData = "platform";
      // Listen for collision events.
      _body.OnCollision += new OnCollisionEventHandler(HandleCollision);
    }

    /// <summary>
    /// Called when a collision with the platform occurs.
    /// </summary>
    /// <param name="fixtureA">The first fixture that has collided.</param>
    /// <param name="fixtureB">The second fixture that has collided.</param>
    /// <param name="contact">The Contact object that contains information about the collision.</param>
    /// <returns>Whether the collision should still happen.</returns>
    private bool HandleCollision(Fixture fixtureA, Fixture fixtureB, Contact contact) {
      // Check if one of the Fixtures belongs to a ball.
      bool causedByBall = (string)fixtureA.Body.UserData == "ball" || (string)fixtureB.Body.UserData == "ball";

      if (contact.IsTouching() && causedByBall) {
        bool shouldCollisionOccur = false;

        // A ball should only bounce off a breakable platform once.
        if (_breakable && _visible) {
          Break();
          // Don't collide with the ball anymore.
          _body.CollidesWith = Category.All & ~Category.Cat1;
          shouldCollisionOccur = true;
          // Don't bounce off broken platforms.
        } else if (_breakable && !_visible) {
          shouldCollisionOccur = false;
          // Always bounce off regular platforms.
        } else {
          shouldCollisionOccur = true;
        }

        // Call the methods listening for collision events.
        if (OnPlatformCollision != null && shouldCollisionOccur) {
          OnPlatformCollision(_breakable);
        }
        // Tell the physics engine the intended result.
        return shouldCollisionOccur;
      } else {
        // Otherwise, if it's not caused by a ball, then ignore the collision.
        return false;
      }
    }

    /// <summary>
    /// Breaks a breakable platform.
    /// </summary>
    private void Break() {
      if (!_breakable) {
        throw new InvalidOperationException("You can't break a non-breakable platform.");
      } else {
        _visible = false;
      }
    }

    /// <summary>
    /// Restores a platform to its original state.
    /// </summary>
    public void Reset() {
      if (_breakable) {
        _visible = true;
        // Restore collision with the ball.
        _body.CollidesWith = Category.All;
      }
    }

    /// <summary>
    /// Draws the platform to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      if (_visible) {
        // Scale the texture to the appropriate size.
        Vector2 scale = new Vector2(_size.X / (float)_texture.Width, _size.Y / (float)_texture.Height);
        // Get the center of the texture.
        Vector2 origin = new Vector2((float)_texture.Width / 2, (float)_texture.Height / 2);
        // Draw it!
        spriteBatch.Draw(_texture, _center, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
      }
    }
    //editor stuff
    /// <summary>
    /// Checks if the platforms falls of the left or right of the screen
    /// </summary>
    /// <param name="v">The origin.</param>
    /// <returns>Whether the origin of      the platform is inside the level.</returns>
    /// 
    private Side CheckLeftRight(Vector2 v) {
      if (v.X < 5) {
        return Side.Left;
      }
      if (v.X + Width > Simulation.FieldWidth - 5) {
        return Side.Right;
      }
      return Side.None;
    }
    /// <summary>
    /// Checks if the platform goes out of bounds on the top or bottom
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private Side CheckTopBottom(Vector2 v) {
      if (v.Y < 5) {
        return Side.Top;
      }
      if (v.Y + Height > Simulation.FieldHeight - 5) {
        return Side.Bottom;
      }
      return Side.None;
    }

    //Vector2 originalPosition;
    public enum Side { Top, Right, Bottom, Left, None };

    /// <summary>
    /// Returns true is a mouseclick was in the region of the platform,
    /// </summary>
    /// <param name="ms"></param>
    /// <returns></returns>
    public Boolean IsSelected(Point ms) {
      if (Origin.X <= ms.X && Origin.X + Width >= ms.X) {
        if (Origin.Y <= ms.Y && Origin.Y + Height >= ms.Y) {
          return true;
        }
      }
      return false;
    }


    /// <summary>
    /// Moves the plateform
    /// </summary>
    /// <param name="change"></param>
    public void Move(Vector2 change) {

      Vector2 newPosition = Origin + change;
      Side horizontal = CheckLeftRight(newPosition);
      Side vertical = CheckTopBottom(newPosition);

      if (horizontal == Side.Left) {
        change.X = 5 - Origin.X;
      } else if (horizontal == Side.Right) {
        change.X = (Simulation.FieldWidth - 5) - (Origin.X + Width);
      }

      if (vertical == Side.Top) {
        change.Y = 5 - Origin.Y;
      } else if (vertical == Side.Bottom) {
        change.Y = Simulation.FieldHeight - 5 - (Origin.Y + Height);
      }



      Origin = Origin + change;

    }


  }
}
