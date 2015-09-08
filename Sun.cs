using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Project1
{
    using SharpDX.Toolkit.Graphics;
    public class Sun : ColoredGameObject
    {
        int worldsize;
        Vector3 ambientcolour;
        Vector3 directionalcolour;
        Vector3 specularcolour;
        Vector3 lightdirection;
        public Sun(Project1Game game)
        {

            worldsize = (int)Math.Pow(2, game.scale) + 1;

            ambientcolour = new Vector3(0.2f, 0.2f, 0.2f);
            directionalcolour = new Vector3(1.0f, 1.0f, 0.4f);
            specularcolour = new Vector3(0,0,1);
            lightdirection = new Vector3(0, 0, 0);

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 backNormal = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

            VertexPositionNormalColor[] points = new[]
                    {
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), frontNormal, Color.Yellow), // Front FBLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, -1.0f), frontNormal, Color.Yellow), //FTLN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, -1.0f), frontNormal, Color.Yellow), //FTRN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), frontNormal, Color.Yellow), //FBLN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, -1.0f), frontNormal, Color.Yellow), //FTRN
                    new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, -1.0f), frontNormal, Color.Yellow), //FBRN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, 1.0f), backNormal, Color.Yellow), // BACK BBLN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), backNormal, Color.Yellow), //BTRN
                    new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, 1.0f), backNormal, Color.Yellow), //BTLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, 1.0f), backNormal, Color.Yellow), //BBLN
                    new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, 1.0f), backNormal, Color.Yellow), //BTRN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), backNormal, Color.Yellow), //BTRN
                    new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, -1.0f), topNormal, Color.Yellow), // Top FTLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, 1.0f), topNormal, Color.Yellow), //BTLN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), topNormal, Color.Yellow), //BTRN
                    new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, -1.0f), topNormal, Color.Yellow), //FTLN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), topNormal, Color.Yellow), //BTRN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, -1.0f), topNormal, Color.Yellow), //FTRN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), bottomNormal, Color.Yellow), // Bottom FBLN
                    new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, 1.0f), bottomNormal, Color.Yellow), //BBRN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, 1.0f), bottomNormal, Color.Yellow), //BBLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f),bottomNormal, Color.Yellow), //FBLN
                    new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, -1.0f), bottomNormal, Color.Yellow), //FBRN
                    new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, 1.0f), bottomNormal, Color.Yellow), //BBRN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), leftNormal, Color.Yellow), // Left FBLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, 1.0f), leftNormal, Color.Yellow), //BBLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, 1.0f), leftNormal, Color.Yellow), //BTLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, -1.0f, -1.0f), leftNormal, Color.Yellow), //FBLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, 1.0f), leftNormal, Color.Yellow), //BTLN
                    new VertexPositionNormalColor(new Vector3(-1.0f, 1.0f, -1.0f), leftNormal, Color.Yellow), //FTLN
                    new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, -1.0f), rightNormal, Color.Yellow), // Right FBRN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), rightNormal, Color.Yellow), //BTRN
                    new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, 1.0f), rightNormal, Color.Yellow), //BBRN
                    new VertexPositionNormalColor(new Vector3(1.0f, -1.0f, -1.0f), rightNormal, Color.Yellow), //FBRN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, -1.0f), rightNormal, Color.Yellow), //FTRN
                    new VertexPositionNormalColor(new Vector3(1.0f, 1.0f, 1.0f), rightNormal, Color.Yellow), //BTRN
                };

            vertices = Buffer.Vertex.New(game.GraphicsDevice, points);

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                VertexColorEnabled = true,
                LightingEnabled = true,
                View = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY),
                Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f),
                World = Matrix.Identity
            };

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
        }

        public Vector3 getLightDirection(){
            return this.lightdirection;
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            basicEffect.World = Matrix.Translation(-worldsize * 2, 0, 0) * Matrix.RotationX(time);
            lightdirection.X = (float)Math.Cos(time);
            lightdirection.Y = (float)Math.Sin(time);
        }

        public override void Draw(GameTime gameTime)
        {
            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(this.vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the sun
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }
    }
}
