using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// The VirtualAdapter class and its derived types map the input state from an input device
  /// to input data that the VirtualController understands.
  /// </summary>
  public abstract class VirtualAdapter {
    /// <summary>
    /// The equivalent of the Confirm input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Confirm input is pressed or released.</returns>
    public abstract VirtualButtonState Confirm();
    /// <summary>
    /// The equivalent of the Back input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Back input is pressed or released.</returns>
    public abstract VirtualButtonState Back();
    /// <summary>
    /// The equivalent of the Up input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Up input is pressed or released.</returns>
    public abstract VirtualButtonState Up();
    /// <summary>
    /// The equivalent of the Down input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Down input is pressed or released.</returns>
    public abstract VirtualButtonState Down();
    /// <summary>
    /// The equivalent of the Left input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Left input is pressed or released.</returns>
    public abstract VirtualButtonState Left();
    /// <summary>
    /// The equivalent of the Right input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Right input is pressed or released.</returns>
    public abstract VirtualButtonState Right();

    /// <summary>
    /// The equivalent of the Point input is defined here.
    /// </summary>
    /// <returns>The current position of the input device's Point input.</returns>
    public abstract Point Point();
  }
}
