using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Launcher class represents the level's launcher.
  /// </summary>
  class Launcher {
    private Texture2D _texture;

    private Vector2 _position;

    private bool _active;

    private float _angle;

    public Vector2 Position {
      get { return _position; }
    }

    public void Initialize(Texture2D texture, Vector2 position) {
      _texture = texture;
      _position = position;
      _angle = (float)Math.PI / 4;
    }

    public void Update() {
    }

    public void AdjustAngle(float delta) {
      _angle += delta;

      if (_angle < 0f) {
        _angle = 0f;
      } else if (_angle > Math.PI) {
        _angle = (float)Math.PI;
      }
    }

    public void LaunchBall(Ball ball) {

    }

    public void CalculateTipPos() {
      float x = (float) (_texture.Width * Math.Cos(_angle) + _position.X);
      float y = (float) (_texture.Width * Math.Sin(-1 * _angle) + _position.Y);
      Console.WriteLine (x + ", " + y);
    }

    public void Draw(SpriteBatch spriteBatch) {
      float rotateAngle = -1 * _angle;

      spriteBatch.Draw(_texture, _position, null, Color.White, rotateAngle, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
  }
}
