using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTKGame.Components;

namespace OpenTKGame.GameStuff
{
    class Floor
    {
        public Square[,] Map { get; set; }
        private RenderObject renderObject = new RenderObject(new Vertex[0], 0);
        public Vector2 Size { get; set; }

        public Floor(int width, int height)
        {
            Size = new Vector2(width, height);
            Map = new Square[width, height];
            Generate(width, height);
        }

        private void Generate(int width, int height)
        {
            Random r = new Random();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    StaticTileType tileType;
                    if (r.Next(100) > 20)
                    {
                        tileType = StaticTileType.Floor;
                    }
                    else
                    {
                        tileType = StaticTileType.Wall;
                    }
                    Map[i, j] = new Square(new Vector3(i, j, 0), tileType);
                }
            }
        }

        public void Render(Camera cam, int program)
        {
            renderObject.Dispose();
            renderObject = new RenderObject(new Vertex[6]
                {
                    new Vertex(cam.WorldToScreenSpace(new Vector4(-Size.X/2, Size.Y/2, 0, 1)), new Vector2(0, 1)),
                    new Vertex(cam.WorldToScreenSpace(new Vector4(Size.X/2, Size.Y/2, 0, 1)), new Vector2(1, 1)),
                    new Vertex(cam.WorldToScreenSpace(new Vector4(-Size.X/2, -Size.Y/2, 0, 1)), new Vector2(0, 0)),
                    new Vertex(cam.WorldToScreenSpace(new Vector4(Size.X/2, Size.Y/2, 0, 1)), new Vector2(1, 1)),
                    new Vertex(cam.WorldToScreenSpace(new Vector4(Size.X/2, -Size.Y/2, 0, 1)), new Vector2(1, 0)),
                    new Vertex(cam.WorldToScreenSpace(new Vector4(-Size.X/2, -Size.Y/2, 0, 1)), new Vector2(0, 0))
                }, program);
            renderObject.Render();
        }
    }
}
