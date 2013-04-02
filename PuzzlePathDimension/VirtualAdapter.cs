using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  public abstract class VirtualAdapter {
    public abstract VirtualButtonState Confirm();
    public abstract VirtualButtonState Back();
    public abstract VirtualButtonState Up();
    public abstract VirtualButtonState Down();
    public abstract VirtualButtonState Left();
    public abstract VirtualButtonState Right();

    public abstract Point Point();
  }
}
