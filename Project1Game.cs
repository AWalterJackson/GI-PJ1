// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

using SharpDX;
using SharpDX.Toolkit;

namespace Project1
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class Project1Game : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private Landscape model;
        private Water water;
        private Camera camera;
        private KeyboardManager keyboardManager;
        public KeyboardState keyboardState;
        private MouseManager mouseManager;
        public MouseState mouseState;
        private Sun lightsource;
        public int scale;

        /// <summary>
        /// Initializes a new instance of the <see cref="Project1Game" /> class.
        /// </summary>
        public Project1Game()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            scale = 7;
            keyboardManager = new KeyboardManager(this);
            mouseManager = new MouseManager(this);
        }

        protected override void LoadContent()
        {
            model = new Landscape(this, this.scale);
            water = new Water(this);
            lightsource = new Sun(this);

            // Create an input layout from the vertices

            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Project 1";
            this.camera = new Camera(this);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = keyboardManager.GetState();
            mouseState = mouseManager.GetState();

            // Handle base.Update
            base.Update(gameTime);
            lightsource.Update(gameTime);
            model.Update(gameTime, lightsource.getLightDirection());
            water.Update(gameTime, lightsource.getLightDirection());
            camera.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);
            model.basicEffect.Projection = camera.Projection;
            lightsource.basicEffect.Projection = camera.Projection;
            water.basicEffect.Projection = camera.Projection;
            model.basicEffect.View = camera.View;
            lightsource.basicEffect.View = camera.View;
            water.basicEffect.View = camera.View;
            model.Draw(gameTime);
            lightsource.Draw(gameTime);
            water.Draw(gameTime);

            // Handle base.Draw
            base.Draw(gameTime);
        }

        public float terraincollide(Vector3 pos){
            return this.model.isColliding(pos);
        }

        public Vector3 edge_bounding(Vector3 pos)
        {
            float size = (float)Math.Pow(2, scale) + 1;
            if (pos.X < 0)
            {
                pos.X = 0;
            }
            else if(pos.X > size)
            {
                pos.X = size;
            }
            if (pos.Z < 0)
            {
                pos.Z = 0;
            }
            else if (pos.Z > size)
            {
                pos.Z = size;
            }
            return pos;
        }
    }
}
