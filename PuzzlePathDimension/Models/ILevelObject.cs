using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace PuzzlePathDimension {
  public interface ILevelObject {
    Boolean IsSelected(MouseState ms);
    /// <summary>
    /// Moves the LevelObject and makes sure it does not exit the screen.
    /// </summary>
    /// <param name="change"></param>
    void Move(Vector2 change);
  }
}
