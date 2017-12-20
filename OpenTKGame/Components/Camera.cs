using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKGame.Components
{
    class Camera    // Klass som innehåller information om hur spelkartan ska renderas
    {
        public Vector2 Size { get; set; }
        public Vector2 Offset { get; set; }
        public float Zoom { get; set; }

        public Vector2 WorldToScreenSpace(Vector2 position)
        {
            position -= Offset;
            position.X /= Size.X;
            position.Y /= Size.Y;
            position *= 25 * Zoom;

            return position;
        }
        public Vector4 WorldToScreenSpace(Vector4 position)
        {
            Vector2 pos = new Vector2(position.X, position.Y);
            pos = WorldToScreenSpace(pos);
            position = new Vector4(pos.X, pos.Y, position.Z, position.W);
            return position;
        }

        public IEnumerable<Vector2> WorldToScreenSpace(IEnumerable<Vector2> positions)
        {
            List<Vector2> newPositions = new List<Vector2>();
            foreach (Vector2 position in positions)
            {
                newPositions.Add(WorldToScreenSpace(position));
            }
            return newPositions;
        }
    }
}
