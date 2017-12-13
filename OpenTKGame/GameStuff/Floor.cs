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

        public Floor(int width, int height)
        {
            Map = new Square[10, 10];
            Generate(width, height);
        }

        private void Generate(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color4 color;
                    if ((j % 2 + i) % 2 == 0)
                    {
                        color = Color4.White;
                    }
                    else
                    {
                        color = Color4.Gray;
                    }

                    Map[i, j] = new Square(new Vector3(i, j, 0), color);
                }
            }
        }
    }
}
