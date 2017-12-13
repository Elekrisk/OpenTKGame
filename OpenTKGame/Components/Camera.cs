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
    }
}
