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
        }

        public override void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gametime)
        {
            throw new NotImplementedException();
        }
    }
}
