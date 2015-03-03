using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace GUI.elements
{
    class Button
    {
        public delegate void OnClickEventHandler(Object sender);
        public event OnClickEventHandler OnClick;

        #region createValiues
        public Texture2D unPressed_texture;
        public Color unPressed_color = Color.White;
        public Color unPressed_textColor = Color.Black;
        public float unPressed_textScale = 0.7f;
        public int unPressed_textSpacing = 3;
        public bool unPressed_textCentered = true;
        public Rectangle unPressed_dimentions;
        public Vector2 unPressed_textOffset = new Vector2(0, 0);

        public Texture2D pressed_texture;
        public Color pressed_color = Color.White;
        public Color pressed_textColor = Color.Blue;
        public float pressed_textScale = 0.7f;
        public int pressed_textSpacing = 3;
        public bool pressed_textCentered = true;
        public Rectangle pressed_dimentions;
        public Vector2 pressed_textOffset = new Vector2(0, 0);

        public Texture2D onHover_texture;
        public Color onHover_color = Color.White;
        public Color onHover_textColor = Color.Lerp(Color.Black, Color.Blue, 0.4f);
        public float onHover_textScale = 0.7f;
        public int onHover_textSpacing = 3;
        public bool onHover_textCentered = true;
        public Rectangle onHover_dimentions;
        public Vector2 onHover_textOffset = new Vector2(0, 0);

        public Texture2D currentTexture;
        public Color currentColor;
        public Color currentTextColor;
        public Rectangle currentDimentions;
        public string text;
        public Vector2 currentTextOffset;

        Font textFont;
        FontRenderer textDraw;
        #endregion

        public List<object> memory = new List<object>(); //Used to store valiues inside the button.

        public bool isPressed = false;
        public bool isToggle = false;
        public bool canUnpress = true;
        public bool canHoldDown = false;

        public Button(ContentManager Content, Rectangle Dimentions, string FontPath)
        {
            Content.RootDirectory = "Content/GUI-Elements/";
            unPressed_dimentions = pressed_dimentions = onHover_dimentions = Dimentions;
            unPressed_texture = Content.Load<Texture2D>("Button/blueButton_unPressed.png");
            pressed_texture = Content.Load<Texture2D>("Button/blueButton_Pressed.png");
            onHover_texture = Content.Load<Texture2D>("Button/blueButton_onHover.png");
            text = "I-I'M-TEXT!";

            textDraw = new FontRenderer();

            textFont = new Font(FontPath, Content);
            textFont.Scale = unPressed_textScale;
            textFont.Spacing = unPressed_textSpacing;

            currentColor = Color.White;
            currentTextColor = Color.White;
            currentDimentions = unPressed_dimentions;
            currentTexture = unPressed_texture;
            currentTextOffset = unPressed_textOffset;

            Content.RootDirectory = "Content";
        }

        public void Update(Rectangle cursorPossition)
        {
            isPressed = UpdateButtonState(cursorPossition);

            #region changeValiues
            if (isPressed)
            {
                currentTexture = pressed_texture;
                textFont.Scale = pressed_textScale;
                textFont.Spacing = pressed_textSpacing;
                textDraw.centerText = pressed_textCentered;
                currentColor = pressed_color;
                currentTextColor = pressed_textColor;
                currentDimentions = pressed_dimentions;
                currentTextOffset = pressed_textOffset;
            }
            else if (cursorHover && !isPressed)
            {
                currentTexture = onHover_texture;
                textFont.Scale = onHover_textScale;
                textFont.Spacing = onHover_textSpacing;
                textDraw.centerText = onHover_textCentered;
                currentColor = onHover_color;
                currentTextColor = onHover_textColor;
                currentDimentions = onHover_dimentions;
                currentTextOffset = onHover_textOffset;
            }
            else if (!isPressed)
            {
                currentTexture = unPressed_texture;
                textFont.Scale = unPressed_textScale;
                textFont.Spacing = unPressed_textSpacing;
                textDraw.centerText = unPressed_textCentered;
                currentColor = unPressed_color;
                currentTextColor = unPressed_textColor;
                currentDimentions = unPressed_dimentions;
                currentTextOffset = unPressed_textOffset;
            }
            #endregion
        }

        #region UpdateButtonState
        MouseState oldState_mouse;
        MouseState mouseState;
        bool cursorHover;
        bool wasPressed;
        private bool UpdateButtonState(Rectangle cursorPossition)
        {
            mouseState = Mouse.GetState();

            cursorHover = cursorPossition.Intersects(currentDimentions);
            if (!canHoldDown)
            {
                if (isToggle)
                    if (cursorHover && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (oldState_mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                        {
                            if (!isPressed)
                            {
                                OnClick(this);
                                isPressed = true;
                            }
                            else if (canUnpress)
                                isPressed = false;
                        }
                    }
                    else
                    { }
                else
                {
                    isPressed = cursorHover && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;

                    bool pressedState = isPressed;

                    if (!wasPressed && isPressed)
                        OnClick(this);

                    wasPressed = pressedState;
                }
            }
            else
            {
                if (cursorHover && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (!isPressed)
                    {
                        OnClick(this);
                        isPressed = true;
                    }
                    else if (canUnpress)
                        isPressed = false;
                }
            }

            oldState_mouse = mouseState;
            return isPressed;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            spriteBatch.Draw(currentTexture, currentDimentions, currentColor);
            textDraw.DrawText(spriteBatch, new Vector2(currentDimentions.X + currentDimentions.Width / 2 + 3, currentDimentions.Y + currentDimentions.Height / 2) + currentTextOffset, text, textFont, currentTextColor);

            spriteBatch.End();

        }


    }

    class CheckBox
    {
        public delegate void OnClickEventHandler(Object sender);
        public event OnClickEventHandler OnClick;

        public Texture2D item_texture;
        public Color item_color = Color.White;
        public Rectangle item_dimentions;
        public bool item_doDraw = true;
        public Vector2 item_scale = new Vector2(1, 1);
        public Vector2 item_offsetFix;

        public Texture2D unPressed_texture;
        public Color unPressed_color = Color.White;
        public Rectangle unPressed_dimentions;
        public bool unPressed_doDraw = false;

        public Texture2D pressed_texture;
        public Color pressed_color = Color.White;
        public Rectangle pressed_dimentions;
        public bool pressed_doDraw = false;

        public Texture2D onHover_texture;
        public Color onHover_color = Color.White;
        public Rectangle onHover_dimentions;
        public bool onHover_doDraw = false;


        public List<object> memory = new List<object>(); //Used to store valiues inside the CheckBox.

        public bool isPressed = false;
        public bool canUnpress = true;
        public bool item_doCenter = true;

        public CheckBox(ContentManager Content, Rectangle Dimentions)
        {
            Content.RootDirectory = "Content/GUI-Elements/";
            unPressed_dimentions = pressed_dimentions = onHover_dimentions = item_dimentions = Dimentions;
            item_texture = Content.Load<Texture2D>("CheckBox/CheckBox_item_placeholder.png");
            unPressed_texture = Content.Load<Texture2D>("Null.png");
            pressed_texture = Content.Load<Texture2D>("CheckBox/CheckBox_pressed_Right_placeholder.png");
            onHover_texture = Content.Load<Texture2D>("CheckBox/CheckBox_onHover_placeholder.png");

            item_offsetFix = new Vector2(0, 0);

            Content.RootDirectory = "Content";
        }

        public void Update(Rectangle cursorPossition)
        {
            isPressed = UpdateButtonState(cursorPossition);

            if (isPressed)
                pressed_doDraw = true;
            else
                pressed_doDraw = false;

            if (!isPressed)
                unPressed_doDraw = true;
            else
                unPressed_doDraw = false;

            if (cursorHover)
                onHover_doDraw = true;
            else
                onHover_doDraw = false;

        }

        #region UpdateButtonState
        MouseState oldState_mouse;
        MouseState mouseState;
        bool cursorHover;
        private bool UpdateButtonState(Rectangle cursorPossition)
        {
            mouseState = Mouse.GetState();

            cursorHover = cursorPossition.Intersects(onHover_dimentions);
            if (canUnpress)
                if (cursorHover && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (oldState_mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (!isPressed)
                        {
                            OnClick(this);
                            isPressed = true;
                        }
                        else
                        {
                            OnClick(this);
                            isPressed = false;
                        }
                    }
                }
                else
                {
                }
            else
            {
                if (cursorHover && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (oldState_mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (!isPressed)
                        {
                            OnClick(this);
                            isPressed = true;
                        }
                    }
                }
            }
            oldState_mouse = mouseState;
            return isPressed;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            if (item_doDraw && !item_doCenter)
                spriteBatch.Draw(texture: item_texture,
                    position: new Vector2(item_dimentions.X - item_offsetFix.X, item_dimentions.Y - item_offsetFix.Y),
                    color: item_color,
                    scale: item_scale);
            else if (item_doDraw && item_doCenter)
                spriteBatch.Draw(item_texture, item_dimentions, item_color);
            if (unPressed_doDraw)
                spriteBatch.Draw(unPressed_texture, unPressed_dimentions, unPressed_color);
            if (onHover_doDraw)
                spriteBatch.Draw(onHover_texture, onHover_dimentions, onHover_color);
            if (pressed_doDraw)
                spriteBatch.Draw(pressed_texture, pressed_dimentions, pressed_color);

            spriteBatch.End();

        }

    }

    class Slider2D
    {
        public delegate void OnClickEventHandler(Object sender);
        public event OnClickEventHandler OnMove;

        public List<object> memory = new List<object>(); //Used to store valiues inside the button.

        #region Peramiters
        public Texture2D backgroundTexture;
        public Color color;
        public Rectangle dimentions;
        public Button firstArrow; //Up || Left
        public Button seccondArrow; // Down || Right
        public bool vertical;

        public Texture2D sliderTexture; //Vertical || Horisontal
        public Rectangle sliderDimentions;
        public Color sliderColor;
        public Rectangle slideBounds;

        public float segment;
        public int segmentCount = 0;
        #endregion

        public Slider2D(ContentManager Content, Rectangle Dimensions, bool Vertical)
        {
            Content.RootDirectory = "Content/GUI-Elements/";
            dimentions = Dimensions;
            slideBounds = dimentions;

            color = Color.White;
            sliderColor = Color.White;

            vertical = Vertical;

            if (!vertical)
            {
                #region Horizontal
                sliderTexture = Content.Load<Texture2D>("Slider/Slider.png");
                sliderDimentions = new Rectangle(dimentions.X + dimentions.Width / 10, slideBounds.Y + 25, dimentions.Width * 8 / 10, dimentions.Height / 6);


                backgroundTexture = Content.Load<Texture2D>("Slider/SliderBar2D.png");

                firstArrow = new Button(Content, new Rectangle(dimentions.X, dimentions.Y, dimentions.Width, dimentions.Height / 10), "Font/Calibri");
                seccondArrow = new Button(Content, new Rectangle(dimentions.X, dimentions.Y + dimentions.Height * 9 / 10, dimentions.Width, dimentions.Height / 10), "Font/Calibri");

                Content.RootDirectory = "Content/GUI-Elements/";
                firstArrow.onHover_texture = firstArrow.pressed_texture = firstArrow.unPressed_texture = Content.Load<Texture2D>("Slider/Arrow_Up.png");
                seccondArrow.onHover_texture = seccondArrow.pressed_texture = seccondArrow.unPressed_texture = Content.Load<Texture2D>("Slider/Arrow_Down.png");

                slideBounds.Y += firstArrow.currentDimentions.Height;
                slideBounds.Height -= seccondArrow.currentDimentions.Height + sliderDimentions.Height * 8 / 5;
                segment = ((float)slideBounds.Height / 100);
                #endregion
            }
            else if (vertical)
            {
                #region Vertical
                sliderTexture = Content.Load<Texture2D>("Slider/Slider_Vertical.png");
                backgroundTexture = Content.Load<Texture2D>("Slider/SliderBar2D_Vertical.png");

                sliderDimentions = new Rectangle(dimentions.X + dimentions.Height / 2, dimentions.Y + dimentions.Height / 10, dimentions.Width / 6, dimentions.Height * 8 / 10);
                firstArrow = new Button(Content, new Rectangle(dimentions.X, dimentions.Y, dimentions.Width / 10, dimentions.Height), "Font/Calibri");
                seccondArrow = new Button(Content, new Rectangle(dimentions.X + dimentions.Width * 9 / 10, dimentions.Y + 10, dimentions.Width / 10, dimentions.Height), "Font/Calibri");

                Content.RootDirectory = "Content/GUI-Elements/";
                seccondArrow.onHover_texture = seccondArrow.pressed_texture = seccondArrow.unPressed_texture = Content.Load<Texture2D>("Slider/Arrow_Right.png");
                firstArrow.onHover_texture = firstArrow.pressed_texture = firstArrow.unPressed_texture = Content.Load<Texture2D>("Slider/Arrow_Left.png");

                segmentCount = 100;

                slideBounds.X += firstArrow.currentDimentions.Width;
                slideBounds.Width -= seccondArrow.currentDimentions.Width;
                segment = ((float)slideBounds.Width / 100);
                #endregion
            }
            #region ArrowSettings
            firstArrow.text = "";
            firstArrow.isToggle = false;
            firstArrow.OnClick += firstArrow_OnClick;
            firstArrow.canHoldDown = true;
            seccondArrow.text = "";
            seccondArrow.isToggle = false;
            seccondArrow.OnClick += seccondArrow_OnClick;
            seccondArrow.canHoldDown = true;
            #endregion

            Content.RootDirectory = "Content";
        }

        void seccondArrow_OnClick(object sender)
        {
            Button obj = (Button)sender;
            if (segmentCount < 100)
                segmentCount++;

        }

        void firstArrow_OnClick(object sender)
        {
            Button obj = (Button)sender;
            if (segmentCount > 0)
                segmentCount--;
        }

        private Rectangle oldSliderDimentions = new Rectangle();
        public void Update(Rectangle cursor, GameTime gameTime)
        {
            firstArrow.Update(cursor);
            seccondArrow.Update(cursor);

            if (!vertical)
            {
                if (dimentions.Intersects(cursor))
                {
                    if (segmentCount > 0 && Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                        segmentCount--;
                    if (segmentCount < 100 && Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                        segmentCount++;
                }
                sliderDimentions.Y = (int)(slideBounds.Y + segment * segmentCount);
            }
            else
            {
                if (dimentions.Intersects(cursor))
                {
                    if (segmentCount > 0 && Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                        segmentCount--;
                    if (segmentCount < 100 && Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                        segmentCount++;
                }
                sliderDimentions.X = (int)(slideBounds.X + segment * segmentCount);
            }

            if (sliderDimentions != oldSliderDimentions)
                OnMove(this);

            oldSliderDimentions = sliderDimentions;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            spriteBatch.Draw(backgroundTexture, dimentions, color);

            spriteBatch.Draw(sliderTexture, sliderDimentions, sliderColor);

            spriteBatch.End();

            firstArrow.Draw(spriteBatch);
            seccondArrow.Draw(spriteBatch);
        }

    }

    class CustomCursor
    {
        public Vector2 scale;
        public Microsoft.Xna.Framework.Color colorOverlay;
        public Microsoft.Xna.Framework.Rectangle hitbox;

        public List<Texture2D> texture;
        public List<string> texturePath;
        public int texturePickIndex;

        public static System.Drawing.Point Position { get; set; }


        public CustomCursor(ContentManager Content, Texture2D Texture, Vector2 Scale, Color ColorOverlay)
        {
            Content.RootDirectory = "Content/GUI-Elements/";
            texturePickIndex = 0;
            texture = new List<Texture2D>();
            texture.Add(Texture);
            texturePath = new List<string>();
            texturePath.Add(texture[0].ToString());
            scale = Scale;
            colorOverlay = ColorOverlay;

            hitbox = new Microsoft.Xna.Framework.Rectangle();
            hitbox.Height = hitbox.Width = 1;

            Content.RootDirectory = "Content";
        }

        public void Update(GameTime gameTime)
        {
            hitboxUppdate();
        }

        public void PickCursor(int CursorIndex)
        {
            texturePickIndex = CursorIndex;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(texture[texturePickIndex], new Microsoft.Xna.Framework.Rectangle(Mouse.GetState().X, Mouse.GetState().Y, (int)(texture[texturePickIndex].Width * scale.X), (int)(texture[texturePickIndex].Height * scale.Y)), colorOverlay);
            spriteBatch.End();
        }

        public void hitboxUppdate()
        {
            hitbox.X = (int)Mouse.GetState().X;
            hitbox.Y = (int)Mouse.GetState().Y;
        }

    }

    class TextField
    {
        public delegate void OnClickEventHandler(Object sender);
        public event OnClickEventHandler OnClick;

        public Texture2D texture;
        public Color color = Color.White;
        public bool drawBackground = true;
        public Rectangle dimentions;
        private bool inputField;

        Font textFont;
        FontRenderer textDraw;

        public string text;
        public Color text_Color = Color.Black;
        public string preText = "Type here...";
        public Color preText_Color = Color.Gray;

        public List<string> availabelNonLetterInput { private set; get; }
        public List<string> availabelLetterInput { private set; get; }

        public TextField(ContentManager Content, bool InputField, Rectangle Dimentions, string FontPath, float TextScale, int TextSpcaing)
        {
            Content.RootDirectory = "Content/GUI-Elements/";
            inputField = InputField;
            dimentions = Dimentions;
            texture = Content.Load<Texture2D>("TextArea/background.png");

            availabelNonLetterInput = fixAvailabelNonLetterInput();
            availabelLetterInput = fixAvailabelLetterInput();

            textDraw = new FontRenderer();
            textFont = new Font(FontPath, Content);
            textFont.Scale = TextScale;
            textFont.Spacing = TextSpcaing;
            textDraw.drawMargin.Y = 1;
            textDraw.drawMargin.X = 3;

            Content.RootDirectory = "Content";
        }

        private List<string> fixAvailabelNonLetterInput()
        {
            List<string> Output = new List<string>();
            Output.Add("D1");
            Output.Add("D2");
            Output.Add("D3");
            Output.Add("D4");
            Output.Add("D5");
            Output.Add("D6");
            Output.Add("D7");
            Output.Add("D8");
            Output.Add("D9");
            Output.Add("D0");
            Output.Add("OemComma");
            Output.Add("OemPeriod");
            Output.Add("OemMinus");
            Output.Add("OemPlus");
            Output.Add("OemQuestion");
            //avilabelInput.Add("LeftShift");
            //avilabelInput.Add("RightShift");
            //avilabelInput.Add("CapsLock");

            return Output;
        }

        private List<string> fixAvailabelLetterInput()
        {
            List<string> Output = new List<string>();
            Output.Add("Q");
            Output.Add("W");
            Output.Add("E");
            Output.Add("R");
            Output.Add("T");
            Output.Add("Y");
            Output.Add("U");
            Output.Add("I");
            Output.Add("O");
            Output.Add("P");
            Output.Add("A");
            Output.Add("S");
            Output.Add("D");
            Output.Add("F");
            Output.Add("G");
            Output.Add("H");
            Output.Add("J");
            Output.Add("K");
            Output.Add("L");
            Output.Add("Z");
            Output.Add("X");
            Output.Add("C");
            Output.Add("V");
            Output.Add("B");
            Output.Add("N");
            Output.Add("M");

            return Output;
        }

        #region editText
        private List<string> textHolder = new List<string>();
        public void editText(string keyChanged, bool capitalLetter, bool isWithinBox)
        {
            text = "";

            if (keyChanged == "Back" && textHolder.Count > 0)
                textHolder.RemoveAt(textHolder.Count - 1);
            else if (isWithinBox)
            {
                if (keyChanged == "Space")
                    textHolder.Add(" ");
                else if (availabelNonLetterInput.Contains(keyChanged))
                {
                    #region replace nonLetter Input
                    if (keyChanged == "D1")
                        textHolder.Add("1");
                    else if (keyChanged == "D2")
                        textHolder.Add("2");
                    else if (keyChanged == "D3")
                        textHolder.Add("3");
                    else if (keyChanged == "D4")
                        textHolder.Add("4");
                    else if (keyChanged == "D5")
                        textHolder.Add("5");
                    else if (keyChanged == "D6")
                        textHolder.Add("6");
                    else if (keyChanged == "D7")
                        textHolder.Add("7");
                    else if (keyChanged == "D8")
                        textHolder.Add("8");
                    else if (keyChanged == "D9")
                        textHolder.Add("9");
                    else if (keyChanged == "D0")
                        textHolder.Add("0");
                    else if (keyChanged == "OemComma")
                        textHolder.Add(",");
                    else if (keyChanged == "OemPeriod")
                        textHolder.Add(".");
                    else if (keyChanged == "OemMinus")
                        textHolder.Add("-");
                    else if (keyChanged == "OemPlus")
                        textHolder.Add("+");
                    else if (keyChanged == "OemQuestion")
                        textHolder.Add("'");
                    #endregion
                }
                else if (availabelLetterInput.Contains(keyChanged))
                {
                    if (!capitalLetter)
                        textHolder.Add(keyChanged.ToLower());
                    else
                        textHolder.Add(keyChanged);
                }
            }

            for (int i = 0; i < textHolder.Count; i++)
                text += textHolder[i];
        }
        #endregion

        public void Update(Rectangle cursorPossition)
        {
            ChangeState(cursorPossition);
        }

        #region ChangeState
        KeyboardState oldKeyState;
        KeyboardState keyState;
        bool isTargeted = false;
        MouseState oldMouseState;
        MouseState mouseState;
        bool cursorHover;
        bool canWrite;
        Microsoft.Xna.Framework.Input.Keys[] keysChange;
        bool capsLock = false;
        bool isWithinBox = true;
        //string oldKeyChanged;
        private void ChangeState(Rectangle cursorPossition)
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (isTargeted)
                color = Color.Violet;
            else
                color = Color.White;

            cursorHover = cursorPossition.Intersects(dimentions);
            if (cursorHover && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    isTargeted = !isTargeted;
                    canWrite = !canWrite;
                }
            }
            else if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                if (oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    isTargeted = false;
                    canWrite = false;
                }

            if (isTargeted && keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                if (isTargeted && oldKeyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    isTargeted = false;
                    canWrite = false;
                    OnClick(this);
                }

            if (isTargeted && keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.CapsLock))
                if (isTargeted && oldKeyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.CapsLock))
                    capsLock = !capsLock;


            //ReadKey monogame edition!
            keysChange = keyState.GetPressedKeys();
            string holder = "";
            bool capitalLetter = true;
            if (keysChange.Length >= 1 && keyState != oldKeyState && canWrite)
            {
                holder = keysChange[0].ToString();

                if (textDraw.width + textFont.RectangleList[0].Width > dimentions.Width)
                    isWithinBox = false;
                else if (!isWithinBox && textDraw.width + textFont.RectangleList[0].Width < dimentions.Width)
                    isWithinBox = true;

                #region TODO: Add string with ALL imputs not just the first one!
                //TODO: Add string with ALL imputs not just the first one!

                //if (scores != oldKeyChanged)
                //{
                //    oldKeyChanged = scores;
                //}
                //else if (keysChange.Length >= 2)
                //{
                //    scores = keysChange[1].ToString();
                //}
                //else
                //    scores = "";
                #endregion

                if (!capsLock)
                {
                    if (text == null)
                        capitalLetter = true;
                    else if (keysChange.Contains<Microsoft.Xna.Framework.Input.Keys>(Microsoft.Xna.Framework.Input.Keys.LeftShift) || keysChange.Contains<Microsoft.Xna.Framework.Input.Keys>(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        capitalLetter = true;
                    else if (capitalLetter)
                        capitalLetter = false;
                }
                else if (!capitalLetter)
                    capitalLetter = true;

                editText(holder, capitalLetter, isWithinBox);
            }

            oldKeyState = keyState;
            oldMouseState = mouseState;
        }
        #endregion

        public Vector2 textPos = Vector2.Zero;
        public bool overRideTextPos = false;
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!overRideTextPos)
                textPos = new Vector2(dimentions.X, dimentions.Y);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            if (drawBackground)
                spriteBatch.Draw(texture, dimentions, new Rectangle(100, 100, 100, 100), color);

            if (text == "" || text == null)
                textDraw.DrawText(spriteBatch, textPos, preText, textFont, preText_Color);
            else
                textDraw.DrawText(spriteBatch, textPos, text, textFont, text_Color);

            spriteBatch.End();
        }

    }

    static class Write
    {
        static ThreadStart trigger1 = new ThreadStart(graphicalIndicator);
        static Thread thread1 = new Thread(trigger1);

        public static void Start()
        {
            thread1.Start();
        }

        static private void graphicalIndicator()
        {
        }
    }

    public class FontRenderer
    {

        public FontRenderer()
        {
        }

        Vector2 GetOffset(string text, Font font)
        {
            Vector2 offset = new Vector2();
            foreach (char c in text)
            {
                int key = (int)c;

                Rectangle rect = font.RectangleDictionary[key];

                offset.X += rect.Width * font.Scale + font.Spacing;
                offset.Y = rect.Height * font.Scale;
            }
            offset.X -= font.Spacing;



            return offset;
        }

        public bool centerText = false;
        public bool centerText_Y = false;
        public int width { private set; get; }
        public Vector2 drawMargin = Vector2.Zero;

        /// <summary>
        /// Draws the given text with the given font.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="position">The upper left position of the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to draw with.</param>
        /// <param name="color">The color to draw with.</param>
        public void DrawText(SpriteBatch spriteBatch, Vector2 position, string text, Font font, Color color)
        {
            int oldkey = -1;


            Vector2 offset = GetOffset(text, font);
            offset /= 2;

            if (centerText)
            {
                position.X -= offset.X;
                position.Y -= offset.Y;
            }
            else if (centerText_Y)
            {
                position.Y -= offset.Y;
            }

            position += drawMargin;

            width = 0;
            for (int i = 0; i < text.Length; i++)
            {
                int key = (int)text[i];

                spriteBatch.Draw(font.Texture, new Rectangle((int)position.X + width, (int)position.Y, (int)(font.RectangleDictionary[key].Width * font.Scale), (int)(font.RectangleDictionary[key].Height * font.Scale)), font.RectangleDictionary[key], color);
                width += (int)(font.RectangleDictionary[key].Width * font.Scale + font.Spacing);

                oldkey = key;
            }

        }
    }

    public class Font
    {
        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture2D Texture { get; private set; }
        /// <summary>
        /// Gets the rectangle dictionary.
        /// </summary>
        /// <value>
        /// The rectangle dictionary.
        /// </value>
        public Dictionary<int, Rectangle> RectangleDictionary { get; private set; }
        public List<Rectangle> RectangleList { get; private set; }
        private XDocument doc;
        private string path;

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public float Scale { get; set; }
        /// <summary>
        /// Gets or sets the spacing in pixels.
        /// </summary>
        /// <value>
        /// The spacing in pixels.
        /// </value>
        public int Spacing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Font"/> class.
        /// </summary>
        /// <param name="font">The file name of the font.</param>
        /// <param name="Content">Content manager.</param>
        public Font(string font, ContentManager Content)
        {
            Scale = 1f;
            Spacing = 3;
            Texture = Content.Load<Texture2D>(font);
            RectangleDictionary = new Dictionary<int, Rectangle>();
            RectangleList = new List<Rectangle>();

            path = Content.RootDirectory;
            doc = XDocument.Load(path + @"\" + font + ".xml");

            Parse();
        }

        private void Parse()
        {
            XElement element = doc.Element("fontMetrics");
            var characters = element.Elements("character");

            RectangleDictionary.Add(-1, new Rectangle(0, 0, 0, 0));

            foreach (XElement character in characters)
            {
                Rectangle rect = new Rectangle();
                rect.X = int.Parse(character.Element("x").Value);
                rect.Y = int.Parse(character.Element("y").Value);
                rect.Width = int.Parse(character.Element("width").Value);
                rect.Height = int.Parse(character.Element("height").Value);
                RectangleDictionary.Add(int.Parse(character.Attribute("key").Value), rect);
                RectangleList.Add(rect);
            }
        }
    }
}
