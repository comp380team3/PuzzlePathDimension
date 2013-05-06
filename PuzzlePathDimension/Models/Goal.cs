using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Goal class represents the goal of a level.
  /// </summary>
  public class Goal:ILevelObject {
    /// <summary>
    /// The hard-coded height of the goal, in pixels.
    /// </summary>
    private const int _height = 60;
    /// <summary>
    /// The hard-coded width of the goal, in pixels.
    /// </summary>
    private const int _width = 60;

    /// <summary>
    /// The texture that the goal will be drawn with.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// The Body that represents the goal in the physics simulation.
    /// </summary>
    private Body _body;

    /// <summary>
    /// The upper-left corner of the goal, in pixels.
    /// </summary>
    private Vector2 _origin;
    /// <summary>
    /// Gets the upper-left corner of the goal, in pixels.
    /// </summary>
    public Vector2 Origin {
      get { return _origin; }
      set {
        _origin = value;
        // Moving the goal also moves its center, so figure out the position of
        // the new center.
        _center = CalculateCenter();

        // Reposition the Body, but only if it has been initialized.
        if (_body != null) {
          _body.Position = UnitConverter.ToMeters(_center);
        }
      }
    }

    /// <summary>
    /// The position of the center of the goal, in pixels.
    /// </summary>
    private Vector2 _center;

    /// <summary>
    /// Gets the position of the center of the goal, in pixels.
    /// </summary>
    public Vector2 Center {
      get { return _center; }
    }

    /// <summary>
    /// Gets the height of the goal.
    /// </summary>
    public int Height {
      get { return _height; }
    }

    /// <summary>
    /// Gets the width of the goal.
    /// </summary>
    public int Width {
      get { return _width; }
    }

    /// <summary>
    /// This delegate is called when the goal is touched.
    /// </summary>
    public delegate void GoalTouched();
    /// <summary>
    /// Occurs when the goal is touched.
    /// </summary>
    public event GoalTouched OnGoalCollision;

    /// <summary>
    /// Constructs a Goal object.
    /// </summary>
    /// <param name="texture">The texture to draw the goal with.</param>
    /// <param name="position">The upper-left corner of the goal, in pixels.</param>
    public Goal(Texture2D texture, Vector2 position) {
      _texture = texture;
      // Check to make sure that the visual representation of the goal is actually the right
      // size, and print a warning to the console if that isn't the case.
      if (_texture != null && _texture.Width * _texture.Height != _width * _height) {
        Console.WriteLine("Warning: the goal's texture does not have the expected dimensions.");
        Console.WriteLine("Expected: " + _width + ", " + _height);
        Console.WriteLine("...but the texture is: " + _texture.Width + ", " + _texture.Height);
      }
      // Figure out the origin and center of the goal.
      _origin = position;
      _center = CalculateCenter();
      // Leave the Body object uninitialized until a World object comes by to initialize it.
      //originalPosition = new Vector2(position.X, position.Y);
      _body = null;
    }

    /// <summary>
    /// Calculates the center of the goal.
    /// </summary>
    /// <returns>The position of the center of the goal.</returns>
    private Vector2 CalculateCenter() {
      Vector2 center = new Vector2();
      center.X = _origin.X + (_width / 2.0f);
      center.Y = _origin.Y + (_height / 2.0f);
      return center;
    }

    /// <summary>
    /// Initializes the goal's Body object.
    /// </summary>
    /// <param name="world">The World object that the goal will be a part of.</param>
    public void InitBody(World world) {
      if (_body != null) {
        throw new InvalidOperationException("There is already a Body object for this goal.");
      }
      // Obtain the radius of the goal, in meters.
      float radius = UnitConverter.ToMeters(_width / 2);

      // Create the Body object.
      _body = BodyFactory.CreateCircle(world, radius, 1);
      // Set its position to be the center of the goal, in meters, which is what the
      // physics engine expects.
      _body.Position = UnitConverter.ToMeters(_center);
      // The goal should never be subjected to the World's physical forces.
      _body.BodyType = BodyType.Static;
      // The ball should not actually bounce off the goal.
      _body.FixtureList[0].IsSensor = true;
      // Associate the Body with the goal.
      _body.UserData = "goal";
      // Listen for collision events.
      _body.OnCollision += new OnCollisionEventHandler(HandleCollision);
    }

    /// <summary>
    /// Called when a collision with the goal occurs.
    /// </summary>
    /// <param name="fixtureA">The first fixture that has collided.</param>
    /// <param name="fixtureB">The second fixture that has collided.</param>
    /// <param name="contact">The Contact object that contains information about the collision.</param>
    /// <returns>Whether the collision should still happen.</returns>
    private bool HandleCollision(Fixture fixtureA, Fixture fixtureB, Contact contact) {
      // Check if one of the Fixtures belongs to a ball.
      bool causedByBall = (string)fixtureA.Body.UserData == "ball" || (string)fixtureB.Body.UserData == "ball";

      // A subtle fact about the OnCollision event is that it is only called
      // when the associated Contact object is changed from not-touching to touching.
      // While two objects are still touching each other, OnCollision won't be called again.
      if (contact.IsTouching() && causedByBall) {
        // Call any methods that are listening to this event.
        if (OnGoalCollision != null) {
          OnGoalCollision();
        }
      }
      // A goal isn't an object that should be bounced off of, so don't actually
      // cause the collision to happen in the physics simulation.
      return false;
    }

    /// <summary>
    /// Draws the goal to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the goal.</param>
    public void Draw(SpriteBatch spriteBatch) {
      // Draw the goal, using its center as the origin to draw from.
      Vector2 origin = new Vector2((_width / 2.0f), (_height / 2.0f));
      spriteBatch.Draw(_texture, _center, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
    }





    
    private Side CheckLeftRight(Vector2 v) {
      // Subtract 1 to account for the fact that the origin is at (0,0).
      if (v.X < 5) {
        return Side.Left;
      }
      if (v.X + Width > Simulation.FieldWidth - 5) {
        return Side.Right;
      }
      //else if(v.X >=5 && v.X+Width <= Simulation.FieldWidth - 5 && v.Y >= 5 && v.Y+Height <= Simulation.FieldHeight - 5)
      //  return Side.None; 
      return Side.None;
    }
    private Side CheckTopBottom(Vector2 v) {
      // Subtract 1 to account for the fact that the origin is at (0,0).
      if (v.Y < 5) {
        return Side.Top;
      }
      if (v.Y + Height > Simulation.FieldHeight - 5) {
        return Side.Bottom;
      }
      //else if(v.X >=5 && v.X+Width <= Simulation.FieldWidth - 5 && v.Y >= 5 && v.Y+Height <= Simulation.FieldHeight - 5)
      //  return Side.None; 
      return Side.None;
    }

    //Vector2 originalPosition;
    public enum Side { Top, Right, Bottom, Left, None };

    public Boolean IsSelected(Point ms) {
      Vector2 mouse = new Vector2(ms.X, ms.Y);
      if (Vector2.Distance(Center, mouse) < Vector2.Distance(Center, Center + new Vector2(0, Height / 2)))
        return true;
      return false;
    }

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
