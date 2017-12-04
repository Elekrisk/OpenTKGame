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
        public MainWindow() : base(1280, 720, GraphicsMode.Default, "Example Window", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        private int program;
        private int vertexArray;

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = true;
            program = CompileShaders();
            GL.GenVertexArrays(1, out vertexArray);
            GL.BindVertexArray(vertexArray);
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
        }

        public override void Exit()
        {
            GL.DeleteVertexArrays(1, ref vertexArray);
            GL.DeleteProgram(program);
            base.Exit();
        }

        private double currentTime;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            currentTime += e.Time;
            Title = $"(VSync: {VSync}) FPS: {1f / e.Time:0}";

            Color4 backColor = new Color4
            {
                A = 1.0f,
                R = 0.1f,
                G = 0.1f,
                B = 0.3f
            };

            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(program);

            GL.Uniform1(GL.GetUniformLocation(program, "time"), currentTime);

            GL.DrawArrays(PrimitiveType.Points, 1, 1);
            GL.PointSize(100);

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
