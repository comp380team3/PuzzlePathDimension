using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  public abstract class VirtualAdapter {
    public abstract DigitalButtonState Confirm();
    public abstract DigitalButtonState Back();
    public abstract DigitalButtonState Up();
    public abstract DigitalButtonState Down();
    public abstract DigitalButtonState Left();
    public abstract DigitalButtonState Right();

    public abstract Point Point();
  }
}
