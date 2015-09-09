using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Project1{
    using SharpDX.Toolkit.Graphics;
    class Landscape : ColoredGameObject{

        public int size;
        private int polycount;
        private int degree;
        private float[,] coords;
        private VertexPositionNormalColor[] terrain;
        private Random rngesus;
        public Landscape(Game game, int degree){
            this.degree = degree;
            this.size = (int)Math.Pow(2,this.degree)+1;
            this.polycount = (int)Math.Pow(this.size - 1, 2) * 2;
            this.rngesus = new Random();
            this.coords = new float[size, size];

            Generate(0,this.size,0,this.size,100,65);
            this.terrain = TerrainModel(this.coords);
            vertices = Buffer.Vertex.New(game.GraphicsDevice, TerrainModel(this.coords));

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

        //Recursive Diamond Square
        private void Generate(int xmin, int xmax, int ymin, int ymax, int maxnoise, int remaining){

            //Termination condition
            if(remaining <= 0){
                return;
            }

            //Local variable declarations, performed after termination check for efficiency
            float bl, br, tl, tr, c;
            float t, b, l, r;
            float noise;
            float avg;

            //Algorithm Steps
            //Diamond Step
            for(int i = xmin + remaining; i < xmax; i += remaining){
                for(int j = ymin + remaining; j < ymax; j += remaining){
                    bl = this.coords[i-remaining,j-remaining];
                    br = this.coords[i,j-remaining];
                    tl = this.coords[i-remaining,j];
                    tr = this.coords[i,j];
                    avg = fouravg(bl,br,tl,tr);
                    if(rngesus.Next(0,2) > 0){
                        noise = rngesus.Next(0,maxnoise);
                    }
                    else{
                        noise = -rngesus.Next(0,maxnoise);
                    }
                    this.coords[i - (remaining / 2), j - (remaining / 2)] = (avg + noise);
                }
            }

            //Square Step
            for (int i = xmin + 2 * remaining; i < xmax; i += remaining){
                for (int j = ymin + (2 * remaining); j < ymax; j += remaining){
                    b = this.coords[i - remaining, j - remaining];
                    br = this.coords[i, j - remaining];
                    tl = this.coords[i - remaining, j];
                    t = this.coords[i, j];
                    c = coords[i - remaining / 2, j - remaining / 2];

                    l = this.coords[i-(3*remaining/2),j-(remaining/2)];
                    r = this.coords[i-remaining/2,j-3*remaining/2];

                    if(rngesus.Next(0,2) > 0){
                        noise = rngesus.Next(0,maxnoise);
                    }
                    else{
                        noise = -rngesus.Next(0,maxnoise);
                    }

                    this.coords[i - remaining, j - remaining / 2] = fouravg(b, tl, c, l) + noise;
                    this.coords[i - remaining/2, j - remaining] = fouravg(b, tl, c, r) + noise;
                    
                }
            }

            Generate(xmin,xmax,ymin,ymax,maxnoise/2,remaining/2);
        }

        private float fouravg(float a, float b, float c, float d){
            return (a + b + c + d) / (float)4.0;
        }

        private VertexPositionNormalColor[] TerrainModel(float[,] map){
            VertexPositionNormalColor[] VList = new VertexPositionNormalColor[this.polycount*3];
            int index=0;
            
            Vector3 p1,p2,p3;
            Vector3 normal;

            //Upper Triangles in Mesh
            for (int i = 0; i < this.size-1; i++){
                for (int j = 0; j < this.size - 1;j++ ){

                    p1 = new Vector3(i, map[i, j], j);
                    p2 = new Vector3(i, map[i, j + 1], j + 1);
                    p3 = new Vector3(i + 1, map[i + 1, j + 1], j + 1);

                    normal = genNormal(p1,p2,p3);

                    VList[index] = new VertexPositionNormalColor(p1,normal,getColor(map[i,j]));
                    VList[index + 1] = new VertexPositionNormalColor(p2, normal, getColor(map[i,j+1]));
                    VList[index + 2] = new VertexPositionNormalColor(p3, normal, getColor(map[i+1,j+1]));
                    index += 3;
                }
            }

            //Lower Triangles in Mesh
            for (int i = 1; i < this.size; i++){
                for (int j = 1; j < this.size; j++){

                    p1 = new Vector3(i, map[i, j], j);
                    p2 = new Vector3(i, map[i, j - 1], j - 1);
                    p3 = new Vector3(i - 1, map[i - 1, j - 1], j - 1);

                    normal = genNormal(p1, p2, p3);

                    VList[index] = new VertexPositionNormalColor(p1, normal, getColor(map[i,j]));
                    VList[index + 1] = new VertexPositionNormalColor(p2, normal, getColor(map[i,j-1]));
                    VList[index+2] = new VertexPositionNormalColor(p3, normal, getColor(map[i-1,j-1]));
                    index += 3;
                }
            }
            return VList;
        }

        private Color getColor(float vert){
            if (vert >= 0 && vert < 10){
                return Color.Green;
            }
            if (vert >= 10){
                return Color.Gray;
            }
            else{
                return Color.SandyBrown;
            }
        }

        private Vector3 genNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 normal = Vector3.Cross(b - a, c - a);
            normal = Vector3.Normalize(normal);
            return normal;
        }

        public void Update(GameTime gameTime, Vector3 light)
        {
            // Rotate the cube.
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);

            basicEffect.DirectionalLight0.Enabled = true;
            basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.5f, 0);
            basicEffect.DirectionalLight0.Direction = light;
            basicEffect.DirectionalLight0.SpecularColor = new Vector3(0, 0, 1);
        }

        public override void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the terrain.
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }

        public bool isColliding(Vector3 pos)
        {
            int i = 0;

            int closestx = (int)Math.Round(pos.X, MidpointRounding.AwayFromZero);
            int closestz = (int)Math.Round(pos.Z, MidpointRounding.AwayFromZero);

            while (terrain[i].Position.X != closestx && terrain[i].Position.Z != closestz){
                if (i > (polycount*3)/2 + 1){
                    return false;
                }
                i++;
            }

            if (terrain[i].Position.Y >= pos.Y){
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
