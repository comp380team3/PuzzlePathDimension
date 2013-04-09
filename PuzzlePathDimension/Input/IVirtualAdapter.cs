using System;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// The IVirtualAdapter interface and any classes that implement it map the input state 
  /// from an input device to input data that the VirtualController understands.
  /// </summary>
  public interface IVirtualAdapter {
    /// <summary>
    /// Gets whether the device that an adapter is associated with is connected to the
    /// computer.
    /// </summary>
    bool Connected { get; }

    /// <summary>
    /// The equivalent of the Confirm input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Confirm input is pressed or released.</returns>
    VirtualButtonState Confirm();
    /// <summary>
    /// The equivalent of the Back input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Back input is pressed or released.</returns>
    VirtualButtonState Back();
    /// <summary>
    /// The equivalent of the Context input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Context input is pressed or released.</returns>
    VirtualButtonState Context();
    /// <summary>
    /// The equivalent of the Pause input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Pause input is pressed or released.</returns>
    VirtualButtonState Pause();

    /// <summary>
    /// The equivalent of the Up input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Up input is pressed or released.</returns>
    VirtualButtonState Up();
    /// <summary>
    /// The equivalent of the Down input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Down input is pressed or released.</returns>
    VirtualButtonState Down();
    /// <summary>
    /// The equivalent of the Left input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Left input is pressed or released.</returns>
    VirtualButtonState Left();
    /// <summary>
    /// The equivalent of the Right input is defined here.
    /// </summary>
    /// <returns>Whether the input device's Right input is pressed or released.</returns>
    VirtualButtonState Right();

    /// <summary>
    /// The equivalent of the Point input is defined here.
    /// </summary>
    /// <returns>The current position of the input device's Point input.</returns>
    Point Point();
  }
}
