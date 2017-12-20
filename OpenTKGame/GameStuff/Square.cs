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
    enum StaticTileType { Empty, Floor, Wall }

    class Square
    {
        public Vector3 Position { get; set; }
        public StaticTileType TileType { get; set; }

        public Square(Vector3 position, StaticTileType tileType)
        {
            Position = position;
            TileType = tileType;
        }
    }
}
