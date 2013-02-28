using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PuzzlePathDimension
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //The screens and the current screen
        ControllerDetectScreen mControllerScreen;
        TitleScreen mTitleScreen;
        GameScreen mGameScreen;
        Screen mCurrentScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Initialize the various screens in the game
            mControllerScreen = new ControllerDetectScreen(this.Content, new EventHandler(ControllerDetectScreenEvent));
            mTitleScreen = new TitleScreen(this.Content, new EventHandler(TitleScreenEvent));
            mGameScreen = new GameScreen(this.Content, GraphicsDevice.Viewport, new EventHandler(GameScreenEvent));

            //Set the current screen
            mCurrentScreen = mControllerScreen;


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //By taking advantage of Polymorphism, we can call update on the current screen class,
            //but the Update in the subclass is the one that will be executed.

            mCurrentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //Again, using Polymorphism, we can call draw on the current screen class
            //and the Draw in the subclass is the one that will be executed.
            mCurrentScreen.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //This event fires when the Controller detect screen is returing control back to the main game class
        public void ControllerDetectScreenEvent(Object obj, EventArgs e)
        {
            //Switch to the title screen, the Controller detect screen is finished being displayed
            mCurrentScreen = mTitleScreen;
        }

        //Thid event is fired when the Title screen is returning control back to the main game class
        public void TitleScreenEvent(Object obj, EventArgs e)
        {
            //Switch to the controller detect screen, the Title screen is finished being displayed
            mCurrentScreen = mGameScreen;
        }

        public void GameScreenEvent(Object obj, EventArgs e)
        {
            //Switch to the title screen, the Title screen is finished being displayed
            mCurrentScreen = mTitleScreen;
        }
    }
}
