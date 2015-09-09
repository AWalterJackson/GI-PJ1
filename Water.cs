using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Project1
{
    using SharpDX.Toolkit.Graphics;
    class Water : ColoredGameObject
    {
        public Water(Project1Game game){

            int max = (int)Math.Pow(2,game.scale)+1;

            Vector3 surfacenormal = new Vector3(0, 1, 0);

            vertices = Buffer.Vertex.New(
                game.GraphicsDevice,
                new[]
                {
                    new VertexPositionNormalColor(new Vector3(0f, 0f, 0f), surfacenormal, Color.LightBlue), // Front FBLN
                    new VertexPositionNormalColor(new Vector3(0f, 0f, max), surfacenormal, Color.LightBlue), //FTLN
                    new VertexPositionNormalColor(new Vector3(max, 0f, max), surfacenormal, Color.LightBlue), //FTRN
                    new VertexPositionNormalColor(new Vector3(max, 0f, max), surfacenormal, Color.LightBlue), //FBLN
                    new VertexPositionNormalColor(new Vector3(max, 0f, 0f), surfacenormal, Color.LightBlue), //FTRN
                    new VertexPositionNormalColor(new Vector3(0f, 0f, 0f), surfacenormal, Color.LightBlue), //FBRN)
                });

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                VertexColorEnabled = true,
                LightingEnabled = true,
                View = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY),
                Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f),
                World = Matrix.Identity
            };

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            this.game = game;
        }

        public override void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime, Vector3 light)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);


            basicEffect.Alpha = 0.5f;
            basicEffect.DirectionalLight0.Enabled = true;
            basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.5f, 0);
            basicEffect.DirectionalLight0.Direction = light;
            basicEffect.DirectionalLight0.SpecularColor = new Vector3(0, 0, 1);
        }

        public override void Draw(GameTime gametime)
        {
            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the sun
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }
    }
}
