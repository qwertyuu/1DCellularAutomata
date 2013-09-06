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

namespace _1DCA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool[] oldLine;
        bool[] currentLine;
        int lessOne;
        double ratio = 20d/40d;
        Texture2D toDraw;
        List<bool[]> toConvert;
        Camera cam;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = (int)(graphics.PreferredBackBufferWidth * ratio);
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
            IsMouseVisible = true;
            cam = new Camera();
            toConvert = new List<bool[]>();
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Calculate));
            System.Threading.Thread calculator = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Algo));
            t.IsBackground = true;
            calculator.IsBackground = true;
            oldLine = new bool[GraphicsDevice.Viewport.Width];
            Random swag = new Random();
            for (int i = 0; i < oldLine.Length; i++)
            {
                //oldLine[i] = (swag.Next(2) == 0) ? true : false;
                oldLine[i] = false;
            }
            oldLine[oldLine.Length / 2] = true;
            toConvert.Add(oldLine);
            lessOne = oldLine.Length - 1;
            calculator.Start();
            t.Start();
            base.Initialize();
        }

        private void Algo(object obj)
        {
            for (int j = 0; j < oldLine.Length; j++)
            {
                currentLine = new bool[oldLine.Length];
                for (int i = 0; i < oldLine.Length; i++)
                {
                    bool P, Q, R;
                    Q = oldLine[i];
                    if (i == 0)
                    {
                        P = oldLine[lessOne];
                        R = oldLine[i + 1];
                    }
                    else if (i == lessOne)
                    {
                        P = oldLine[i - 1];
                        R = oldLine[0];
                    }
                    else
                    {
                        P = oldLine[i - 1];
                        R = oldLine[i + 1];
                    }
                    // rule 73: !((P && R) || (P ^ Q ^ R))
                    //rule ?
                    currentLine[i] = P ^ Q ^ R;
                }
                toConvert.Add(currentLine);
                oldLine = currentLine;
            }
        }

        private void Calculate(object obj)
        {
            bool exit = false;
            while (!exit)
            {
                int count1 = toConvert.Count;
                if (count1 != 0)
                {
                    while (toConvert.Count != count1)
                    {
                        count1 = toConvert.Count;
                        int count2 = oldLine.Length;
                        Color[] buf = new Color[count1 * count2];
                        Texture2D toAdd = new Texture2D(graphics.GraphicsDevice, oldLine.Length, count1);
                        for (int i = 0; i < count1; i++)
                        {
                            for (int j = 0; j < count2; j++)
                            {
                                buf[i * toConvert[i].Length + j] = toConvert[i][j] ? Color.White : Color.Transparent;
                            }
                        }
                        toAdd.SetData(buf);
                        toDraw = toAdd;
                        exit = true;
                    }
                }
            }
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
            // TODO: Add your update logic here
            cam.Update(oldLine.Length, graphics);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.viewMatrix);
            spriteBatch.Draw(toDraw, Vector2.Zero, Color.Black);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
