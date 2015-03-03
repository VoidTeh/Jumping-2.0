using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUI.elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;

namespace Jumping_2._0
{
    class menuScreen
    {
        public delegate void OnClickEventHandler(Object sender);
        public event OnClickEventHandler OnClick;

        public Button playButton;
        public Button resumeButton;

        public menuScreen(ContentManager Content)
        {
            playButton = new Button(Content, new Rectangle(200, 200, 170, 50), "Font/Calibri");
            playButton.OnClick += playButton_OnClick;
            playButton.text = "Start Game";
            playButton.pressed_color = Color.Red;

            resumeButton = new Button(Content, new Rectangle(200, 250, 170, 50), "Font/Calibri");
            resumeButton.OnClick += resumeButton_OnClick;
            resumeButton.text = "Restart Level";
            resumeButton.pressed_color = Color.Red;
        }

        void resumeButton_OnClick(object sender)
        {
            OnClick(this);
        }
        void playButton_OnClick(object sender)
        {
            OnClick(this);
        }

        public void Update(Rectangle cursorPos, string privScreen)
        {
            playButton.Update(cursorPos);
            if (privScreen != null)
                resumeButton.Update(cursorPos);
        }

        public void Draw(SpriteBatch spriteBatch, string privScreen)
        {
            playButton.Draw(spriteBatch);
            if (privScreen != null)
                resumeButton.Draw(spriteBatch);
        }
    }
}
