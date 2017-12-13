using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OpenTKGame.Components
{
    class Square
    {
        public RenderObject renderObject = new RenderObject(new Vertex[6]
        {
            new Vertex(new Vector4(-1, -1, 0, 1), Color4.AliceBlue),
            new Vertex(new Vector4(1, -1, 0, 1), Color4.AliceBlue),
            new Vertex(new Vector4(-1, 1, 0, 1), Color4.AliceBlue),
            new Vertex(new Vector4(1, 1, 0, 1), Color4.Red),
            new Vertex(new Vector4(1, -1, 0, 1), Color4.Red),
            new Vertex(new Vector4(-1, 1, 0, 1), Color4.Red)
        });
        public Vector3 Position { get; set; }

        public Square(Vector3 position)
        {
            Position = position;
        }

        public void Render(Camera cam)
        {
            Vertex[] newVertices = new Vertex[6]
            {
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(-1, -1, 0)), 1), Color4.AliceBlue),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(1, -1, 0)), 1), Color4.AliceBlue),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(-1, 1, 0)), 1), Color4.AliceBlue),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(1, 1, 0)), 1), Color4.Red),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(1, -1, 0)), 1), Color4.Red),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(-1, 1, 0)), 1), Color4.Red)
            };
            renderObject = new RenderObject(newVertices);

            renderObject.Render();
        }

        private Vector3 WorldToScreen(Camera cam, Vector3 position)
        {
            Vector2 pos = new Vector2(position.X, position.Y);
            pos -= cam.Offset;
            pos.X /= cam.Size.X;
            pos.Y /= cam.Size.Y;
            pos *= 25 * cam.Zoom;

            return new Vector3(pos.X, pos.Y, position.Z);
        }
        private Vector3[] WorldToScreen(Camera cam, Vector3[] pos)
        {
            Vector3[] positions = new Vector3[pos.Length];
            for (int i = 0; i < pos.Length; i++)
            {
                positions[i] = WorldToScreen(cam, pos[i]);
            }
            return positions;
        }
    }
}
