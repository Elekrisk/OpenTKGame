using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKGame.Components
{
    class MainWindow : GameWindow
    {
        private int program;
        private int vertexArray;
        private List<Square> toRender = new List<Square>();
        private Camera camera;
        bool pressedMinus = false;
        bool pressedPlus = false;

        public MainWindow() : base(1280, 720, GraphicsMode.Default, "Example Window", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.ForwardCompatible)
        {
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            camera.Size = new Vector2(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            camera = new Camera();
            camera.Size = new Vector2(Width, Height);
            camera.Zoom = 1.0f;

            Square sq = new Square(new Vector3(-5, -5, 0));
            toRender.Add(sq);

            CursorVisible = true;
            program = CompileShaders();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            Closed += OnClosed;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Exit();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard();
        }

        private void HandleKeyboard()
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            if (ks.IsKeyDown(Key.KeypadAdd) || ks.IsKeyDown(Key.Plus))
            {
                if (!pressedPlus && camera.Zoom < 10)
                {
                    camera.Zoom *= 1.25f;
                    pressedPlus = true;
                }
            }
            else
            {
                pressedPlus = false;
            }
            if (ks.IsKeyDown(Key.KeypadSubtract) || ks.IsKeyDown(Key.Minus))
            {
                if (!pressedMinus && camera.Zoom > 0.1f)
                {
                    camera.Zoom *= 0.75f;
                    pressedMinus = true;
                }
            }
            else
            {
                pressedMinus = false;
            }

            if (ks.IsKeyDown(Key.Left))
            {
                camera.Offset = new Vector2(camera.Offset.X - 1, camera.Offset.Y);
            }
            if (ks.IsKeyDown(Key.Right))
            {
                camera.Offset = new Vector2(camera.Offset.X + 1, camera.Offset.Y);
            }
            if (ks.IsKeyDown(Key.Up))
            {
                camera.Offset = new Vector2(camera.Offset.X, camera.Offset.Y + 1);
            }
            if (ks.IsKeyDown(Key.Down))
            {
                camera.Offset = new Vector2(camera.Offset.X, camera.Offset.Y - 1);
            }
        }

        public override void Exit()
        {
            foreach (Square r in toRender)
            {
                r.renderObject.Dispose();
            }
            GL.DeleteProgram(program);
            base.Exit();
        }

        //private double currentTime;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //currentTime += e.Time;
            //Title = $"(VSync: {VSync}) FPS: {1f / e.Time:0}";

            Color4 backColor = new Color4
            {
                A = 1.0f,
                R = 0.1f,
                G = 0.1f,
                B = 0.3f
            };

            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4.CreateOrthographic(1.0f, 1.0f, -1.0f, 1.0f);
            

            GL.UseProgram(program);

            foreach (Square ro in toRender)
            {
                ro.Render(camera);
            }
            
            SwapBuffers();
        }

        private int CompileShaders()
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, File.ReadAllText(@"..\..\Components\Shaders\vertexShader.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText(@"..\..\Components\Shaders\fragmentShader.frag"));
            GL.CompileShader(fragmentShader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            return program;
        }
    }
}
