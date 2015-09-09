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
        private Vector3 position, target, up;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(Project1Game game)
        {
            position = new Vector3(0, 0, -10);
            target = new Vector3(0, 0, 0);
            up = Vector3.UnitY;
            View = Matrix.LookAtLH(position, target, up);
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
            //View = Matrix.Multiply(View, Matrix.RotationYawPitchRoll(rotation_speed * time * -x_pos, rotation_speed * time * -y_pos, rotation * rotation_speed * time));
            int forward = direction(Keys.W, Keys.S);
            int sideways = direction(Keys.A, Keys.D);

            Vector3 NormTP = Vector3.Normalize(target - position);
            Vector3 NormTPxU = Vector3.Normalize(Vector3.Cross(target - position, up));
            Vector3 NormU = Vector3.Normalize(up);
            //moving forward and back
            position += time * speed * forward * NormTP;
            //Moving side to side
            position += time * speed * sideways * NormTPxU;
            //Moving the target of our view forward and back
            target += time * speed * forward * NormTP;
            //Moving the target of our view side to side
            target += time * speed * sideways * NormTPxU;
            //Moving the target of our view as we pitch
            target -= rotation_speed * time * y_pos * NormU;
            //Making sure the target stays one unit distance away from us
            target = position + Vector3.Normalize(target - position);
            //Moving the 'up' vector forwards and backwards as we pitch
            up += rotation_speed * time * y_pos * Vector3.Normalize(target - position);
            up.Normalize();
            //Moving the 'up' vector side to side as we roll
            up -= rotation_speed * time * rotation * Vector3.Normalize(Vector3.Cross(target - position, up));
            up.Normalize();
            //Moving the target of our view as we yaw
            target -= rotation_speed * time * x_pos * Vector3.Normalize(Vector3.Cross(target - position, up));
            //Making sure the target stays one unit distance away from us
            target = position + Vector3.Normalize(target - position);

            //Setting the new view matrix if there's no collision
            //if(game.model.is_within_landscape(position))
            View = Matrix.LookAtLH(position, target, up);
            /* if (rotation == 0 && forward == 0 && sideways == 0)
             {
                 return;
             }*/
            //Console.WriteLine(View.ToString());
            //View.Forward = speed * time * forward * View.Forward;
            //Vector3 movement = speed * time * new Vector3(forward * View.Forward.X + sideways * View.Right.X, forward * View.Forward.Y + sideways * View.Right.Y, forward * View.Forward.Z + sideways * View.Right.Z);
            //Vector3 movement = speed * time * forward * View.Forward;
            //View = /*Matrix.Subtract(Matrix.Add*/Matrix.Multiply(View, Matrix.Translation(movement))/*, Matrix.Translation(new Vector3()))*/;
            //Console.WriteLine(Matrix.Multiply(View, Matrix.RotationYawPitchRoll(rotation_speed * time * -x_pos, rotation_speed * time * -y_pos, rotation * rotation_speed * time)).ToString());
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
