using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ParticleSimulator;

public class Window(int width, int height, string title) : GameWindow(GameWindowSettings.Default,
    new NativeWindowSettings()
    {
        ClientSize = (width, height),
        Title = title
    })
{
    private readonly float[] _vertices =
    [
        -0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f,
         0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f,
         0.0f,  0.5f, 0.0f, 0.0f, 0.0f, 1.0f
    ];

    private Shader _shader;
    private int _vertexArrayObject;
    private readonly Stopwatch _timer = new();

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        
        _timer.Start();
        
        GL.ClearColor(38/255.0f, 35/255.0f, 32/255.0f, 1.0f);
        
        var vertexBufferObject = GL.GenBuffer();
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);


        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
       
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        
        _shader = new Shader(Path.Combine("Shaders", "shader.vert"), Path.Combine("Shaders", "shader.frag"));
        _shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        _shader.Use();
        
        var timeValue = _timer.Elapsed.TotalSeconds;
        var opacity = (float)Math.Abs(Math.Sin(timeValue));
        var vertexColorLocation = GL.GetUniformLocation(_shader.handle, "uniOpacity");
        GL.Uniform1(vertexColorLocation, opacity);
        
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);
        
        GL.DeleteBuffer(_vertexArrayObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        
        GL.DeleteProgram(_shader.handle);
        
        _shader.Dispose();
    }
}
