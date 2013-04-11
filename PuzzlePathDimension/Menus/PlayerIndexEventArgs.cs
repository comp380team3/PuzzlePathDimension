//-----------------------------------------------------------------------------
// PlayerIndexEventArgs.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// Custom event argument which includes the index of the player who
  /// triggered the event. This is used by the MenuEntry.Selected event.
  /// </summary>
  class PlayerIndexEventArgs : EventArgs {
    /// <summary>
    /// Gets the index of the player who triggered this event.
    /// </summary>
    public PlayerIndex PlayerIndex { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PlayerIndexEventArgs(PlayerIndex playerIndex) {
      PlayerIndex = playerIndex;
    }
  }
}
