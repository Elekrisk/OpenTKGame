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
    class RenderObject : IDisposable
    {
        private bool initialized;
        private readonly int vertexArray;
        private readonly int buffer;
        private readonly int verticeCount;

        public RenderObject(Vertex[] vertices)
        {
            verticeCount = vertices.Length;

            vertexArray = GL.GenVertexArray();
            buffer = GL.GenBuffer();

            GL.BindVertexArray(vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);

            GL.NamedBufferStorage(buffer, Vertex.Size * vertices.Length, vertices, BufferStorageFlags.MapWriteBit);

            GL.VertexArrayAttribBinding(vertexArray, 1, 0);
            GL.EnableVertexArrayAttrib(vertexArray, 1);
            GL.VertexArrayAttribFormat(vertexArray, 1, 4, VertexAttribType.Float, false, 0);

            GL.VertexArrayAttribBinding(vertexArray, 2, 0);
            GL.EnableVertexArrayAttrib(vertexArray, 2);
            GL.VertexArrayAttribFormat(vertexArray, 2, 4, VertexAttribType.Float, false, 16);

            GL.VertexArrayVertexBuffer(vertexArray, 0, buffer, IntPtr.Zero, Vertex.Size);
            initialized = true;
        }

        public void Render()
        {
            GL.BindVertexArray(vertexArray);
            GL.DrawArrays(PrimitiveType.Triangles, 0, verticeCount);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (initialized)
                {
                    GL.DeleteVertexArray(vertexArray);
                    GL.DeleteBuffer(buffer);
                    initialized = false;
                }
            }
        }
    }
}
