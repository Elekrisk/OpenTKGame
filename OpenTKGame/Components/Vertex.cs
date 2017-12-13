﻿using System;
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
        public const int Size = (4 + 4) * 4;    // Storleken på vertexen, konstant 32 bytes

        private readonly Vector4 position;
        private readonly Color4 color;

        public Vertex(Vector4 pos, Color4 col)
        {
            position = pos;
            color = col;
        }
    }
}
