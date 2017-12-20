using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace OpenTKGame.Components
{
    public struct Vertex    // Denna klass/struct är helt kopierad från en tutorial
    {
        public const int Size = (4 + 2) * 4;    // Storleken på vertexen, konstant 24 bytes

        private readonly Vector4 position;
        private readonly Vector2 texCoords;

        public Vertex(Vector4 pos, Vector2 tex)
        {
            position = pos;
            texCoords = tex;
        }
    }
}
