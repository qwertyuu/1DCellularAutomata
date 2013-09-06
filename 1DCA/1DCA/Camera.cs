using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _1DCA
{
    class Camera
    {
        public Matrix viewMatrix;
        int oldScrollValue = 0;
        int old;
        int diff = 0;

        public void Update(int lol, GraphicsDeviceManager graphics)
        {
            var mouse = Mouse.GetState();
            diff += mouse.ScrollWheelValue - old;

            if (diff >= 0)
            {
                viewMatrix = Matrix.CreateTranslation(-lol / 2, 0, 0) * Matrix.CreateScale((diff + 1000) / 1000f) * Matrix.CreateTranslation(graphics.PreferredBackBufferWidth / 2, 0, 0);
            }
            else
            {
                diff = 0;
            }
            old = mouse.ScrollWheelValue;
        }
    }
}
