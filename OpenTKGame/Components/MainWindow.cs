using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        private List<RenderObject> toRender = new List<RenderObject>(); // En lista med Square-klasser, vilket är det alla saker på själva spelkartan ärver
        private Camera camera;  // En klass med information om hur spelkartan ska renderas
        bool pressedMinus = false;  // Ifall användaren tryckte på minus-knappen förra bilden
        bool pressedPlus = false;   // Ifall användaren tryckte på plus-knappen förra bilden

        private Bitmap floorTile;
        private Bitmap wallTile;

        private int mapTexture;

        private Map map;

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
            floorTile = OpenTKGame.Properties.Resources.FloorTile;
            wallTile = OpenTKGame.Properties.Resources.WallTile;

            GL.Enable(EnableCap.Texture2D);

            camera = new Camera     // Skapar kameran med en default zoom på 4
            {
                Size = new Vector2(Width, Height),
                Zoom = 4.0f
            };

            map = new Map();
            Floor f1 = new Floor(1000, 1000);   // Skapar temporär våning för att testa saker
            map.floors.Add(f1);
            mapTexture = CompileStaticMap(f1.Map);

            CursorVisible = true;   // Rätt så självbeskrivande, gör muspekaren synlig
            program = CompileShaders(); // En funktion som jag kopierade från en tutorial, kompilerar två shaders och länkar dem till ett program och returnerar sedan det.
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);    // Är ej säker, gissar på att den sätter polygoner till att vara tvåsidiga och att de ska vara ifyllda
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);  // Har ingen aning
            Closed += OnClosed;     // Lägger till OnClosed att anropas då fönstret stängs
        }

        private int CompileStaticMap(Square[,] squares)
        {
            Bitmap map = new Bitmap(squares.GetLength(0) * 16, squares.GetLength(1) * 16);
            Graphics g = Graphics.FromImage(map);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, map.Width, map.Height));
            foreach (Square sq in squares)
            {
                switch (sq.TileType)
                {
                    case StaticTileType.Wall:
                        g.DrawImage(wallTile, new Point((int)sq.Position.X * 16, (int)sq.Position.Y * 16));
                        break;
                    case StaticTileType.Floor:
                        g.DrawImage(floorTile, new Point((int) sq.Position.X * 16, (int) sq.Position.Y * 16));
                        break;
                    case StaticTileType.Empty:
                    default:
                        break;
                }
            }

            GL.GenTextures(1, out int mapTexture);

            BitmapData bmpData = map.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.BindTexture(TextureTarget.Texture2D, mapTexture);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, map.Width, map.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Rgb, PixelType.UnsignedByte, bmpData.Scan0);
            map.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            RenderObject obj = new RenderObject(new Vertex[6]
            {
                new Vertex(new Vector4(-0.5f, 0.5f, 0, 1), new Vector2(0, 1)),
                new Vertex(new Vector4(0.5f, 0.5f, 0, 1), new Vector2(1, 1)),
                new Vertex(new Vector4(-0.5f, -0.5f, 0, 1), new Vector2(0, 0)),
                new Vertex(new Vector4(0.5f, 0.5f, 0, 1), new Vector2(1, 1)),
                new Vertex(new Vector4(0.5f, -0.5f, 0, 1), new Vector2(1, 0)),
                new Vertex(new Vector4(-0.5f, -0.5f, 0, 1), new Vector2(0, 0))
            }, program);
            toRender.Add(obj);

            return mapTexture;
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

            //GL.ActiveTexture(TextureUnit.Texture0);
            //GL.BindTexture(TextureTarget.Texture2D, mapTexture);

            foreach (Floor f in map.floors)
            {
                f.Render(camera, program);
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
