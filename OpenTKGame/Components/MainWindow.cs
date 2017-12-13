using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTKGame.GameStuff;

namespace OpenTKGame.Components
{
    // Fönstret som spelet körs i, ärver OpenTKs klass GameWindow
    class MainWindow : GameWindow
    {
        private int program;    // Det OpenGL-program som används vid rendering
        private List<Square> toRender = new List<Square>(); // En lista med Square-klasser, vilket är det alla saker på själva spelkartan ärver
        private Camera camera;  // En klass med information om hur spelkartan ska renderas
        bool pressedMinus = false;  // Ifall användaren tryckte på minus-knappen förra bilden
        bool pressedPlus = false;   // Ifall användaren tryckte på plus-knappen förra bilden

        
        // Konstruktör, kallar basklassen konstruktor vilken gör en massa saker.
        public MainWindow() : base(1280, 720, GraphicsMode.Default, "Example Window", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.ForwardCompatible)
        {

        }

        // Anropas när fönstret ändrar storlek
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);   // Sätter viewporten till hela fönstret
            camera.Size = new Vector2(Width, Height);   // Uppdaterar kameran med den nya upplösningen
        }

        // Körs en gång då fönstret har laddats
        protected override void OnLoad(EventArgs e)
        {
            camera = new Camera     // Skapar kameran med en default zoom på 4
            {
                Size = new Vector2(Width, Height),
                Zoom = 4.0f
            };

            Floor f1 = new Floor(10, 10);   // Skapar temporär våning för att testa saker
            foreach (Square sq in f1.Map)
            {
                toRender.Add(sq);   // Lägger till alla Squares på våningen i en lista för rendering
            }

            CursorVisible = true;   // Rätt så självbeskrivande, gör muspekaren synlig
            program = CompileShaders(); // En funktion som jag kopierade från en tutorial, kompilerar två shaders och länkar dem till ett program och returnerar sedan det.
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);    // Är ej säker, gissar på att den sätter polygoner till att vara tvåsidiga och att de ska vara ifyllda
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);  // Har ingen aning
            Closed += OnClosed;     // Lägger till OnClosed att anropas då fönstret stängs
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Exit();
        }

        // Anropas ett fast antal gånger i sekunden, antalet bestäms av GameWindows Run()-funktion
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard();
        }

        // Hanterar tangentbordet
        private void HandleKeyboard()
        {
            KeyboardState ks = Keyboard.GetState(); // Sparar vilka tangenter som är nedtryckta i variabeln ks

            if (ks.IsKeyDown(Key.Escape) || (ks.IsKeyDown(Key.AltLeft) && ks.IsKeyDown(Key.F4)))    // Om Escape trycks ner eller vänstra alt och F4 trycks ner samtidigt stängs programmet av.
            {
                Exit();
            }

            // Zoomar in ifall plus-knappen är nedtryckt och inte var det förra bilden. (Amerikanskt tangentbord)
            if (ks.IsKeyDown(Key.KeypadAdd) || ks.IsKeyDown(Key.Plus))
            {
                if (!pressedPlus && camera.Zoom < 20)
                {
                    camera.Zoom *= 1.25f;
                    pressedPlus = true;
                }
            }
            else
            {
                pressedPlus = false;
            }
            // Samma sak för minus-knappen, zoomar ut
            if (ks.IsKeyDown(Key.KeypadSubtract) || ks.IsKeyDown(Key.Minus))
            {
                if (!pressedMinus && camera.Zoom > 0.5f)
                {
                    camera.Zoom *= 0.75f;
                    pressedMinus = true;
                }
            }
            else
            {
                pressedMinus = false;
            }

            // Flyttar kameran med piltangenterna, hastigheten beror på zoomnivån
            if (ks.IsKeyDown(Key.Left))
            {
                camera.Offset = new Vector2(camera.Offset.X - (1 / camera.Zoom), camera.Offset.Y);
            }
            if (ks.IsKeyDown(Key.Right))
            {
                camera.Offset = new Vector2(camera.Offset.X + (1 / camera.Zoom), camera.Offset.Y);
            }
            if (ks.IsKeyDown(Key.Up))
            {
                camera.Offset = new Vector2(camera.Offset.X, camera.Offset.Y + (1 / camera.Zoom));
            }
            if (ks.IsKeyDown(Key.Down))
            {
                camera.Offset = new Vector2(camera.Offset.X, camera.Offset.Y - (1 / camera.Zoom));
            }
        }

        // Rensar Square-arnas renderingsobjekt från minnet
        public override void Exit()
        {
            foreach (Square r in toRender)
            {
                r.Dispose();
            }
            GL.DeleteProgram(program);
            base.Exit();
        }
        
        // Anropas så många gånger som möjligt
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Color4 backColor = new Color4   // Bakgrundsfärg
            {
                A = 1.0f,   // Alfa
                R = 0.1f,
                G = 0.1f,
                B = 0.3f
            };

            GL.ClearColor(backColor);   // Rensar skärmen med bakgrundsfärgen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);  // Är ej säker på vad detta gör
            
            GL.UseProgram(program);     // Ej heller säker på denna, tror den säger till OpenGL att det är detta program som ska användas vid rendering

            foreach (Square ro in toRender)
            {
                ro.Render(camera);  // Renderar alla Squares med informationen i kamera-objektet till en buffer.
            }
            
            SwapBuffers();  // Byter skärmen och buffern, ritar ut på skärmen.
        }

        // Funktion som kopierats från tutorial, kompilerar två shaders och lägger till dem i ett program som sedan returneras
        private int CompileShaders()
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);    // Skapar en tom VertexShader
            GL.ShaderSource(vertexShader, File.ReadAllText(@"..\..\Components\Shaders\vertexShader.vert")); // Laddar in shader-koden i shadern
            GL.CompileShader(vertexShader); // Kompilerar shadern

            // Samma som ovan men för en FragmentShader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText(@"..\..\Components\Shaders\fragmentShader.frag"));
            GL.CompileShader(fragmentShader);

            int program = GL.CreateProgram();   // Skapar ett tint program
            GL.AttachShader(program, vertexShader);     // Fäster shadern till programmet, är ej helt säker hur det fungerar
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);    // Länkar programmet?

            GL.DetachShader(program, vertexShader); // Tar bort shadern från programmet
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);  // Raderar shadern
            GL.DeleteShader(fragmentShader);
            return program; // Returnerar programmet
        }
    }
}
