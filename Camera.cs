using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Project1
{
    class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public Project1Game game;
        private float speed = 0.005f;
        private float rotation_speed = 0.005f;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(Project1Game game)
        {
            View = Matrix.LookAtLH(new Vector3(0, 0, -10), new Vector3(0, 0, 0), Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            this.game = game;
        }

        // If the screen is resized, the projection matrix will change
        public void Update(GameTime gameTime)
        {
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            //Console.WriteLine(View.ToString());
            //Console.WriteLine(game.mouseState.Y.ToString());
            int rotation = 0;
            if (game.keyboardState.IsKeyDown(Keys.E)) { rotation += 1; }
            if (game.keyboardState.IsKeyDown(Keys.Q)) { rotation -= 1; }
            float x_pos = game.mouseState.X - 0.5f;
            float y_pos = game.mouseState.Y - 0.5f;
            int time = gameTime.ElapsedGameTime.Milliseconds;
            View = Matrix.Multiply(View, Matrix.RotationYawPitchRoll(rotation_speed * time * y_pos, rotation_speed * time * x_pos, rotation * rotation_speed * time));
        }
    }
}
