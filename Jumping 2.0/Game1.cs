#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;

using Jumping;
using GUI.elements;
#endregion

namespace Jumping_2._0
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        gameScreen gameScreen;
        menuScreen menuScreen;
        string acctiveScreen = "menuScreen";

        CustomCursor Cursor;

        Dictionary<string, SoundEffect> sfx;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 500;

            this.graphics.ApplyChanges();
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

            Cursor = new CustomCursor(Content, Content.Load<Texture2D>("GUI-Elements/Cursor/Cursor_Pointer_Placeholder.png"), new Vector2(0.15f, 0.2f), Color.White);

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

            sfx = new Dictionary<string, SoundEffect>();
            sfx.Add("Coin_Pickup", Content.Load<SoundEffect>("Pickup_Coin"));
            sfx.Add("Jump", Content.Load<SoundEffect>("Jump.wav"));

            gameScreen = new gameScreen(Content, sfx, GraphicsDevice, graphics);
            gameScreen.OnClick += gameScreen_OnClick;
            menuScreen = new menuScreen(Content);
            menuScreen.OnClick += menuScreen_OnClick;

            gameScreen.Load(gameScreen.scores);

        }

        void gameScreen_OnClick(object sender)
        {
            privScreen = "gameScreen";
            acctiveScreen = "menuScreen";
        }

        void menuScreen_OnClick(object sender)
        {
            privScreen = "menuScreen";
            acctiveScreen = "gameScreen";
            if (menuScreen.resumeButton.isPressed == true)
                gameScreen.LoadLevel(gameScreen.currentLevel);
            else
                gameScreen.LoadLevel(0);
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
        KeyboardState oldKeyState;
        KeyboardState keyState;
        protected override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            if (keyState != oldKeyState)
            { 
                if (keyState.IsKeyDown(Keys.Escape) && acctiveScreen == "gameScreen")
                {
                    privScreen = "gameScreen";
                    acctiveScreen = "menuScreen";
                }
                else if (keyState.IsKeyDown(Keys.Escape) && acctiveScreen == "menuScreen")
                {
                    privScreen = "menuScreen";
                    acctiveScreen = "gameScreen";
                }
            }

            if (acctiveScreen == "gameScreen")
                gameScreen.Update(gameTime);
            else if (acctiveScreen == "menuScreen")
            {
                Cursor.Update(gameTime);
                menuScreen.Update(Cursor.hitbox, privScreen);
            }

            if (keyState.IsKeyDown(Keys.Q))
                acctiveScreen = "gameScreen";
            else if (keyState.IsKeyDown(Keys.E))
                acctiveScreen = "menuScreen";

            oldKeyState = keyState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        string privScreen = null;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            if (acctiveScreen == "gameScreen")
                gameScreen.Draw(spriteBatch);
            else if (acctiveScreen == "menuScreen" && privScreen == "gameScreen")
            {
                gameScreen.Draw(spriteBatch);
                menuScreen.Draw(spriteBatch, privScreen);
                Cursor.Draw(spriteBatch);
            }
            else if (acctiveScreen == "menuScreen")
            { 
                menuScreen.Draw(spriteBatch, privScreen);
                Cursor.Draw(spriteBatch);
            }

            base.Draw(gameTime);
        }
    }
}
