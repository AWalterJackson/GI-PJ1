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
        private float speed = 0.5f;
        private float rotation_speed = 0.0005f;

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
            int rotation = direction(Keys.E, Keys.Q);
            float x_pos = game.mouseState.X - 0.5f;
            float y_pos = game.mouseState.Y - 0.5f;
            int time = gameTime.ElapsedGameTime.Milliseconds;
            View = Matrix.Multiply(View, Matrix.RotationYawPitchRoll(rotation_speed * time * -x_pos *0, rotation_speed * time * -y_pos*0, rotation * rotation_speed * time));
            int forward = direction(Keys.W, Keys.S);
            int sideways = direction(Keys.A, Keys.D);
            if (rotation == 0 && forward == 0 && sideways == 0)
            {
                return;
            }
            Vector3 movement = speed * new Vector3(forward * View.Forward.X + sideways * View.Right.X, forward * View.Forward.Y + sideways * View.Right.Y, forward * View.Forward.Z + sideways * View.Right.Z);
            View = Matrix.Add(View, Matrix.Translation(movement));
        }

        //Chooses what direction to go or rotate etc, based one keyboard input
        private int direction(Keys key1, Keys key2)
        {
            int dir = 0;
            if (game.keyboardState.IsKeyDown(key1)) { dir += 1; }
            if (game.keyboardState.IsKeyDown(key2)) { dir -= 1; }
            return dir;
        }

    }
}
