//-----------------------------------------------------------------------------
// MenuScreen.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  /// <summary>
  /// Base class for screens that contain a menu of options. The user can
  /// move up and down to select an entry, or cancel to back out of the screen.
  /// </summary>
  abstract class MenuScreen : GameScreen {
    List<MenuEntry> menuEntries = new List<MenuEntry>();
    string menuTitle;

    /// <summary>
    /// Gets the list of menu entries, so derived classes can add
    /// or change the menu contents.
    /// </summary>
    protected IList<MenuEntry> MenuEntries {
      get { return menuEntries; }
    }
    
    public SpriteFont TitleFont { get; private set; }
    public SpriteFont TextFont { get; private set; }

    protected int SelectedEntry { get; set; }



    /// <summary>
    /// Constructor.
    /// </summary>
    public MenuScreen(string menuTitle) {
      this.menuTitle = menuTitle;

      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);

      TitleFont = shared.Load<SpriteFont>("menufont");
      TextFont = shared.Load<SpriteFont>("textfont");
    }


    /// <summary>
    /// Responds to user input, changing the selected entry and accepting
    /// or cancelling the menu.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {
      // Move to the previous menu entry?
      if (vtroller.CheckForRecentRelease(VirtualButtons.Up)) {
        SelectedEntry -= 1;

        if (SelectedEntry < 0)
          SelectedEntry = menuEntries.Count - 1;
      }

      // Move to the next menu entry?
      if (vtroller.CheckForRecentRelease(VirtualButtons.Down)) {
        SelectedEntry += 1;

        if (SelectedEntry >= menuEntries.Count)
          SelectedEntry = 0;
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        OnSelectEntry(SelectedEntry, PlayerIndex.One);
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        OnCancel(PlayerIndex.One);
      }
    }

    /// <summary>
    /// Handler for when the user has chosen a menu entry.
    /// </summary>
    protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex) {
      menuEntries[entryIndex].OnSelectEntry(playerIndex);
    }

    /// <summary>
    /// Handler for when the user has cancelled the menu.
    /// </summary>
    protected virtual void OnCancel(PlayerIndex playerIndex) {
      ExitScreen();
    }

    /// <summary>
    /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
    /// </summary>
    protected void OnCancel(object sender, PlayerIndexEventArgs e) {
      OnCancel(e.PlayerIndex);
    }


    /// <summary>
    /// Allows the screen the chance to position the menu entries. By default
    /// all menu entries are lined up in a vertical list, centered on the screen.
    /// </summary>
    protected virtual void UpdateMenuEntryLocations() {
      // TODO: Replace physical screen viewport with virtual coordinate system.
      Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

      // Make the menu slide into place during transitions, using a
      // power curve to make things look more interesting (this makes
      // the movement slow down as it nears the end).
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

      // start at Y = 175; each X value is generated per entry
      Vector2 position = new Vector2(0f, 175f);

      // update each menu entry's location in turn
      for (int i = 0; i < menuEntries.Count; i++) {
        MenuEntry menuEntry = menuEntries[i];

        // each entry is to be centered horizontally
        position.X = viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

        if (ScreenState == ScreenState.TransitionOn)
          position.X -= transitionOffset * 256;
        else
          position.X += transitionOffset * 512;

        // set the entry's position
        menuEntry.Position = position;

        // move down for the next entry the size of this entry
        position.Y += menuEntry.GetHeight(this);
      }
    }

    /// <summary>
    /// Updates the menu.
    /// </summary>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                   bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      // Update each nested MenuEntry object.
      for (int i = 0; i < menuEntries.Count; i++) {
        bool isSelected = IsActive && (i == SelectedEntry);

        menuEntries[i].Update(this, isSelected, gameTime);
      }
    }


    /// <summary>
    /// Draws the menu.
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      GraphicsDevice graphics = spriteBatch.GraphicsDevice;

      // make sure our entries are in the right place before we draw them
      UpdateMenuEntryLocations();

      spriteBatch.Begin();

      // Draw each menu entry in turn.
      for (int i = 0; i < menuEntries.Count; i++) {
        MenuEntry menuEntry = menuEntries[i];

        bool isSelected = IsActive && (i == SelectedEntry);

        menuEntry.Draw(this, spriteBatch, isSelected, gameTime);
      }

      // Make the menu slide into place during transitions, using a
      // power curve to make things look more interesting (this makes
      // the movement slow down as it nears the end).
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

      // Draw the menu title centered on the screen
      Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
      Vector2 titleOrigin = TitleFont.MeasureString(menuTitle) / 2;
      Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
      float titleScale = 1.25f;

      titlePosition.Y -= transitionOffset * 100;

      spriteBatch.DrawString(TitleFont, menuTitle, titlePosition, titleColor, 0,
                             titleOrigin, titleScale, SpriteEffects.None, 0);

      spriteBatch.End();
    }
  }
}
