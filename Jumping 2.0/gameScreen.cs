using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;

using Jumping;
using GUI.elements;

namespace Jumping_2._0
{
    class gameScreen
    {
        public delegate void OnClickEventHandler(Object sender);
        public event OnClickEventHandler OnClick;

        KeyboardState prevKB;

        public Player Balle;
        List<Block> Blocks;
        List<Coin> Coins;
        public List<string> name = new List<string>();
        public List<int> scores = new List<int>();
        public List<float> time = new List<float>();


        public Camera camera;
        Vector2 backgroundPosition;

        Vector2 timerPos = new Vector2(10, 10);

        SpriteFont font;

        List<char[,]> Levels = new List<char[,]>();
        List<int> startPos;

        int tileWidth, tileHeight;

        int score;
        public int currentLevel;

        Random rnd = new Random();
        bool doodleJump = false;

        Dictionary<string, SoundEffect> sfx;

        GraphicsDeviceManager graphics;

        Texture2D blockSpriteA;
        Texture2D blockSpriteB;
        Texture2D coin;
        public gameScreen(ContentManager Content, Dictionary<string, SoundEffect> Sfx, GraphicsDevice GD, GraphicsDeviceManager Graphics)
        {
            sfx = Sfx;
            graphics = Graphics;

            camera = new Camera(GD.Viewport);

            Blocks = new List<Block>();
            Coins = new List<Coin>();

            if (!doodleJump)
            {
                List<object> holder = MapLibrary.Load();
                Levels = (List<char[,]>)holder[0];
                startPos = (List<int>)holder[1];
                currentLevel = 0;
            }

            font = Content.Load<SpriteFont>("Font");
            Texture2D ballSprite = Content.Load<Texture2D>("ball");
            Balle = new Player(ballSprite, Vector2.Zero, 6.0f, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            backgroundPosition = new Vector2(-400, 0);

            blockSpriteA = Content.Load<Texture2D>("block-1");
            blockSpriteB = Content.Load<Texture2D>("block-2");
            coin = Content.Load<Texture2D>("coin-0");

            if (!doodleJump)
                LoadLevel(currentLevel);
            else
                spawnPlatform(Balle.Position.X, Balle.Position.Y + 100);
        }

        public void LoadLevel(int level)
        {
            Blocks.Clear();
            Coins.Clear();

            Balle.Position = Vector2.Zero;
            Balle.Velocity = Vector2.Zero;

            score = 0;

            tileWidth = Levels[level].GetLength(1);
            tileHeight = Levels[level].GetLength(0);

            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    //Inpassable Blocks
                    if (Levels[level][y, x] == '#')
                    {
                        Blocks.Add(new Block(blockSpriteA, new Vector2(x * 50, y * 50), 1));
                    }
                    //Blocks that are only passable if going up them
                    if (Levels[level][y, x] == '-')
                    {
                        Blocks.Add(new Block(blockSpriteB, new Vector2(x * 50, y * 50), 2));
                    }
                    //Coin
                    if (Levels[level][y, x] == 'C')
                    {
                        Coins.Add(new Coin(coin, new Vector2(x * 50, y * 50), 50));
                    }
                    //Spawn
                    if (Levels[level][y, x] == 'P' && Balle.Position == Vector2.Zero)
                    {
                        Balle.Position = new Vector2(x * 50, (y + 1) * 50 - Balle.Texture.Height);
                        Balle.startPos.Y = startPos[level];
                    }
                    else if (Levels[level][y, x] == 'P' && Balle.Position != Vector2.Zero)
                    {
                        throw new Exception("Only one 'P' is needed for each level");
                    }
                }
            }

            if (Balle.Position == Vector2.Zero)
            {
                throw new Exception("Player Position needs to be set with 'P'");
            }
        }

        void spawnPlatform()
        {
            Blocks.Add(new Block(blockSpriteB, new Vector2(rnd.Next((int)Balle.Position.X, (int)Balle.Position.X + 10), rnd.Next(100, graphics.PreferredBackBufferWidth - 100)),
                2));
        }
        void spawnPlatform(float x, float y)
        {
            Blocks.Add(new Block(blockSpriteB, new Vector2(x, y), 2));
        }

        public void Update(GameTime gameTime)
        {
            HandleInput(Keyboard.GetState());
            Balle.Update(gameTime);

            foreach (Block b in Blocks)
            {
                Balle = b.BlockCollision(Balle);
                if (b.haveJumped && doodleJump)
                {
                    Blocks.Remove(b);
                    spawnPlatform(Balle.Position.X, Balle.Position.Y + 100);
                    Balle.isJumping = true;
                    break;
                }
            }

            for (int i = 0; i < Coins.Count; i++)
            {
                if (Coins[i].isColliding(new Rectangle((int)Balle.Position.X, (int)Balle.Position.Y, Balle.Texture.Width, Balle.Texture.Height)))
                {
                    score += Coins[i].score;
                    Coins.RemoveAt(i);
                    sfx["Coin_Pickup"].Play();
                }
            }

            Balle.screenBound = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            prevKB = Keyboard.GetState();


            if (Balle.Position.Y > startPos[currentLevel] + 100)
            {
                scores.Add(score);
                Save(scores, name, time);
                LoadLevel(currentLevel);
                Balle.t = 0;
                OnClick(this);
            }


            timerPos = GUIposFix(2, 5, 3, 7);
            camera.Update(gameTime, Balle, Balle.Position);
        }
        public bool reset = false;


        private Vector2 GUIposFix(int offsetX_times, int offsetX_divide, int offsetY_times, int offsetY_divide)
        {
            return new Vector2(Balle.Position.X - (graphics.PreferredBackBufferWidth * offsetX_times) / offsetX_divide, Balle.Position.Y - (graphics.PreferredBackBufferHeight * offsetY_times) / offsetY_divide);
        }

        void HandleInput(KeyboardState keyState)
        {
            Balle.Input(keyState);
            if (prevKB.IsKeyUp(Keys.F) && keyState.IsKeyDown(Keys.F))
            {
                this.graphics.ToggleFullScreen();
                this.graphics.ApplyChanges();
            }

            if ((prevKB.IsKeyUp(Keys.L) && keyState.IsKeyDown(Keys.L) && !doodleJump) || (Coins.Count == 0 && !doodleJump))
                NextLevel();
        }

        private void NextLevel()
        {
            currentLevel = (currentLevel + 1) % Levels.Count;
            LoadLevel(currentLevel);
        }

        #region Save & Load
        StreamWriter SW;
        public void Save(List<int> score, List<string> name, List<float> time)
        {
            using (SW = new StreamWriter("save.txt"))
            {
                for (int i = 0; i < score.Count; i++)
                {
                    if (name.Count == i)
                        name.Add("");
                    if (time.Count == i)
                        time.Add(0000);
                    SW.WriteLine("S:{0}S; N:{1}N; T:{2}T;", score[i], name[i], time[i]);
                }
                SW.Close();
            }
        }

        #region FindArg
        static string _holder;
        private int ArgPos(string arg)
        {
            return _holder.IndexOf(arg) + arg.Length;
        }
        private string FindArg(string arg)
        {
            int start = ArgPos(arg + ":");
            int end = ArgPos(arg + ";") - (arg.Length + 1);
            return _holder.Substring(start, end - start);
        }
        #endregion

        StreamReader SR;
        public void Load(List<int> score, List<string> name = null, List<float> time = null)
        {
            using (SR = new StreamReader("save.txt"))
            {
                while (true)
                {
                    _holder = SR.ReadLine();
                    if (_holder != null && _holder != "")
                    {
                        if (name != null)
                            name.Add(FindArg("N"));
                        if (time != null)
                            time.Add(float.Parse(FindArg("T")));
                        if (score != null)
                            score.Add(int.Parse(FindArg("S")));
                    }
                    else
                        break;
                }
                SR.Close();
            }
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                camera.transform);
            foreach (Block b in Blocks)
            {
                b.Draw(spriteBatch);
            }
            foreach (Coin c in Coins)
            {
                c.Draw(spriteBatch);
            }
            Balle.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Score: " + score, timerPos, Color.Black);
            spriteBatch.End();
        }

    }
}
