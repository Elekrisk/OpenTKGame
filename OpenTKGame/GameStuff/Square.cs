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
    class Square
    {
        private RenderObject renderObject = new RenderObject(new Vertex[0]);
        public Vector3 Position { get; set; }
        private Color4 col1;
        private Color4 col2;

        public Square(Vector3 position)
        {
            Position = position;
            col1 = Color4.AliceBlue;
            col2 = Color4.Red;
        }
        public Square(Vector3 position, Color4 color)
        {
            Position = position;
            col1 = color;
            col2 = color;
        }
        public Square(Vector3 position, Color4 color1, Color4 color2)
        {
            Position = position;
            col1 = color1;
            col2 = color2;
        }

        public void Render(Camera cam)
        {
            renderObject.Dispose();
            Vertex[] newVertices = new Vertex[6]
            {
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(Position.X - 0.5f, Position.Y - 0.5f, 0)), 1), col1),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(Position.X + 0.5f, Position.Y - 0.5f, 0)), 1), col1),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(Position.X - 0.5f, Position.Y + 0.5f, 0)), 1), col1),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(Position.X + 0.5f, Position.Y + 0.5f, 0)), 1), col2),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(Position.X + 0.5f, Position.Y - 0.5f, 0)), 1), col2),
                new Vertex(new Vector4(WorldToScreen(cam, new Vector3(Position.X - 0.5f, Position.Y + 0.5f, 0)), 1), col2)
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
        public void Dispose()
        {
            renderObject.Dispose();
        }
    }
}
