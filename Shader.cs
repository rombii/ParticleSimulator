using OpenTK.Graphics.OpenGL4;

namespace ParticleSimulator;

public class Shader : IDisposable
{
    public int handle;
    
    public Shader(string vertexPath, string fragmentPath)
    {
        var vertexShaderSource = File.ReadAllText(vertexPath);
        var fragmentShaderSource = File.ReadAllText(fragmentPath);

        var vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
        var fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
        
        GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
        GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);
        
        GL.CompileShader(vertexShaderHandle);
        GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out var status);
        if (status == 0)
        {
            var infoLog = GL.GetShaderInfoLog(vertexShaderHandle);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(fragmentShaderHandle);
        GL.GetShader(fragmentShaderHandle, ShaderParameter.CompileStatus, out status);
        if (status == 0)
        {
            var infoLog = GL.GetShaderInfoLog(fragmentShaderHandle);
            Console.WriteLine(infoLog);
        }

        handle = GL.CreateProgram();
        
        GL.AttachShader(handle, vertexShaderHandle);
        GL.AttachShader(handle, fragmentShaderHandle); 
        
        GL.LinkProgram(handle);
        
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out status);

        if (status == 0)
        {
            var infoLog = GL.GetProgramInfoLog(handle);
            Console.WriteLine(infoLog);
        }
        
        GL.DetachShader(handle, vertexShaderHandle);
        GL.DetachShader(handle, fragmentShaderHandle);
        
        GL.DeleteShader(vertexShaderHandle);
        GL.DeleteShader(fragmentShaderHandle);
    }

    public void Use()
    {
        GL.UseProgram(handle);
    }


    private bool _disposableValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposableValue)
        {
            GL.DeleteProgram(handle);
            _disposableValue = true;
        }
    }
    
    ~Shader() {
        if (_disposableValue == false)
        {
            Console.WriteLine("Memory leak");
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}