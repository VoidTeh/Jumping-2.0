using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Jumping;

namespace Jumping_2._0
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector2 center;

        public Camera(Viewport newView)
        {
            view = newView;
        }
        public void Update(GameTime gameTime, Player Ball, Vector2 obj)
        {
            center = new Vector2(obj.X + (Ball.screenBound.Width / 2) -750, obj.Y + (Ball.screenBound.Height / 4)- 400);
            transform = Matrix.CreateScale(new Vector3(1,1,0)) *
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y,0));
        }
    }
}
