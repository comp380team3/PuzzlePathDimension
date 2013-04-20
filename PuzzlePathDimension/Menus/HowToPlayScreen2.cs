using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  class HowToPlayScreen2 : GameScreen {

    DetailsTemplate detailsTemplate = new DetailsTemplate();

    /// <summary>
    /// Next button entry on the screen.
    /// </summary>
    MenuButton nextMenuEntry;

    /// <summary>
    /// Back button entry on the screen.
    /// </summary>
    MenuButton backMenuEntry;

    /// <summary>
    /// Exit button entry on the screen.
    /// </summary>
    MenuButton exitMenuEntry;

    SpriteFont font;

    string[] gameObjects = new string[] {
      "Ball: ",
      "Platform: ",
      "Breakable Platform: ",
      "Goal: ",
      "DeathTrap: ",
      "Treasure: ",
    };

    string[] gameContent = new string[] {
      "Texture/ball",
      "Texture/platform",
      "Texture/platform_breakable",
      "Texture/goal",
      "Texture/deathtrap",
      "Texture/treasure",
    };

    List<Texture2D> objectContent = new List<Texture2D>();



    /// <summary>
    /// Contructor
    /// </summary>
    public HowToPlayScreen2() {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      font = shared.Load<SpriteFont>("Font/menufont");

      detailsTemplate.Title = new TextLine("Game Objects", font, new Color(192, 192, 192));

      nextMenuEntry = new MenuButton("Next", font);
      nextMenuEntry.Selected += NextMenuEntrySelected;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Right] = nextMenuEntry;
      detailsTemplate.SelectedItem = DetailsTemplate.Selection.Right;

      backMenuEntry = new MenuButton("Back", font);
      backMenuEntry.Selected += BackMenuEntrySelected;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Left] = backMenuEntry;

      exitMenuEntry = new MenuButton("Exit", font);
      exitMenuEntry.Selected += OnCancel;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Middle] = exitMenuEntry;

      foreach ( string name in gameContent) {
        objectContent.Add(shared.Load<Texture2D>(name));
      }
    }
    
    /// <summary>
    /// Handle the input of the user. If the user wants to move
    /// to a diffenrent menu entry, they can press left or right.
    /// </summary>
    /// <param name="vtroller"></param>
    public override void HandleInput(VirtualController vtroller) {
      base.HandleInput(vtroller);

      if (vtroller.CheckForRecentRelease(VirtualButtons.Left)) {
        detailsTemplate.SelectPrev();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Right)) {
        detailsTemplate.SelectNext();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        detailsTemplate.Confirm();
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        OnCancel(null, new PlayerIndexEventArgs(PlayerIndex.One));
      }
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      detailsTemplate.TransitionPosition = TransitionPosition;
      detailsTemplate.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      int textPositionX = 100;
      int textPositionY = 150;

      detailsTemplate.Draw(spriteBatch, gameTime);

      spriteBatch.Begin();

      foreach (string name in gameObjects) {
        spriteBatch.DrawString(font, name, new Vector2(textPositionX, textPositionY), Color.Black, 0, Vector2.Zero, .75f, SpriteEffects.None, 0f);
        textPositionY += font.LineSpacing + 20;
      }

      textPositionX = 400;
      textPositionY = 150;

      foreach (Texture2D name in objectContent) {
        spriteBatch.Draw(name, new Rectangle(textPositionX, textPositionY, name.Width, name.Height), Color.White);
        textPositionY += font.LineSpacing + name.Height / 2;
      }

      spriteBatch.End();
    }


    /// <summary>
    /// Event handler for when the Next menu entry is selected
    /// </summary>
    void NextMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ExitScreen();
      ScreenList.AddScreen(new HowToPlayScreen3(), e.PlayerIndex);
    }

    /// <summary>
    /// Event handler for when the Back menu entry is selected
    /// </summary>
    void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ExitScreen();
      ScreenList.AddScreen(new HowToPlayScreen1(), e.PlayerIndex);
    }

    protected void OnCancel(object sender, PlayerIndexEventArgs e) {
      ExitScreen();
    }
  }
}
