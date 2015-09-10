using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
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
        private float speed = 0.05f;
        private float rotation_speed = 0.005f;
        private float roll_speed = 0.001f;
        private Vector3 position, target, up;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(Project1Game game)
        {
            position = new Vector3(0, game.getHeight(0,0)+5, -10);
            target = new Vector3(0, 0, 0);
            up = Vector3.UnitY;
            View = Matrix.LookAtLH(position, target, up);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, (float)Math.Pow(2, game.scale) + 1);
            this.game = game;
        }

        // If the screen is resized, the projection matrix will change
        public void Update(GameTime gameTime)
        {
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, game.drawDistance());
            int rotation = direction(Keys.E, Keys.Q);
            float x_pos = 0;
            float y_pos = 0;
            if (!game.getCursorState())
            {
                x_pos = (game.mouseState.X - 0.5f) * 50;
                y_pos = (game.mouseState.Y - 0.5f) * 50;
            }
            int time = gameTime.ElapsedGameTime.Milliseconds;
            int forward = direction(Keys.W, Keys.S);
            int sideways = direction(Keys.A, Keys.D);

            Vector3 NormTP = Vector3.Normalize(target - position);
            Vector3 NormTPxU = Vector3.Normalize(Vector3.Cross(target - position, up));
            Vector3 NormU = Vector3.Normalize(up);
            //moving forward and back
            position += time * speed * forward * NormTP;
            //Moving side to side
            position += time * speed * sideways * NormTPxU;
            //Moving the view target forward and back
            target += time * speed * forward * NormTP;
            //Moving the view target side to side
            target += time * speed * sideways * NormTPxU;
            //Moving the view target as we pitch
            target -= rotation_speed * time * y_pos * NormU;
            //Making sure the target stays one unit distance away from us
            target = position + Vector3.Normalize(target - position);
            //Moving the 'up' vector forwards and backwards as we pitch
            up += rotation_speed * time * y_pos * Vector3.Normalize(target - position);
            up.Normalize();
            //Moving the 'up' vector side to side as we roll
            up -= roll_speed * time * rotation * Vector3.Normalize(Vector3.Cross(target - position, up));
            up.Normalize();
            //Moving the view target as we yaw
            target -= rotation_speed * time * x_pos * Vector3.Normalize(Vector3.Cross(target - position, up));
            //Making sure the target stays one unit distance away from us
            target = position + Vector3.Normalize(target - position);

            //Setting the new view matrix if there's no collision and it's in bounds
            Vector3 edge_bounding = game.edge_bounding(position);
            if(!edge_bounding.Equals(position))
            {
                target -= position - edge_bounding;
                position = edge_bounding;
            }
            float collide_height = game.terraincollide(position);
            target.Y -= position.Y - Math.Max(position.Y, collide_height);
            position.Y = Math.Max(position.Y, collide_height);
            View = Matrix.LookAtLH(position, target, up);
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
