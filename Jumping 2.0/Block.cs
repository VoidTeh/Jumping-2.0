using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jumping
{
    class Block
    {
        Texture2D Texture;
        Vector2 Position;
        int BlockState;

        public bool haveJumped = false;

        public Block(Texture2D Texture, Vector2 Position, int BlockState)
        {
            this.Texture = Texture;
            this.Position = Position;
            this.BlockState = BlockState;
        }

        public Player BlockCollision(Player player)
        {
            Rectangle top = new Rectangle((int)Position.X + 5, (int)Position.Y - 10, Texture.Width - 10, 10);
            Rectangle bottom = new Rectangle((int)Position.X + 5, (int)Position.Y + Texture.Height, Texture.Width - 10, 10);
            Rectangle left = new Rectangle((int)Position.X - 10, (int)Position.Y + 5, 10, Texture.Height - 10);
            Rectangle right = new Rectangle((int)Position.X + Texture.Width, (int)Position.Y + 5, 10, Texture.Height - 10);

            if (BlockState != 2 || (BlockState == 2 && !player.goingUp))
            {
                if (top.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Texture.Width, player.Texture.Height)))
                {
                    if (player.Position.Y + player.Texture.Height > Position.Y && player.Position.Y + player.Texture.Height < Position.Y + Texture.Height/2)
                    {
                        player.Position.Y = player.ground = Position.Y - player.Texture.Height;
                        player.Velocity.Y = 0;
                        player.isJumping = false;
                        player.t = 0;

                        haveJumped = true;
                    }

                }
                else if (player.isJumping == false && player.ground == Position.Y - player.Texture.Height)
                {
                    player.isJumping = true;
                    player.u = 0;
                }

                if (BlockState != 2)
                {
                    if (bottom.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Texture.Width, player.Texture.Height)))
                    {
                        if (player.Position.Y < Position.Y + Texture.Height)
                        {
                            player.Position.Y = Position.Y + Texture.Height;
                            player.t = 0;
                            player.u = 0;
                        }

                    }

                    if (left.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Texture.Width, player.Texture.Height)))
                    {
                        if (player.Position.X + player.Texture.Width > Position.X)
                        {
                            player.Position.X = Position.X - player.Texture.Width;
                            player.Velocity.X = 0;
                        }
                    }

                    if (right.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Texture.Width, player.Texture.Height)))
                    {
                        if (player.Position.X < Position.X + Texture.Width)
                        {
                            player.Position.X = Position.X + Texture.Width;
                            player.Velocity.X = 0;
                        }
                    }
                }
            }
            return player;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height), Color.White);
        }
    }
}
