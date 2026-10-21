using System.Runtime.InteropServices;
using Brutal.GlfwApi;

namespace ThreeSand.Window;

public class WindowHelper
{
    public static unsafe string[] GetRequiredInstanceExtensions()
    {
        var names = Glfw.GetRequiredInstanceExtensions(out var count);
        var result = new string[count];
        
        for (var i = 0; i < count; i++)
        {
            result[i] = Marshal.PtrToStringUTF8((nint)names[i])!;
        }
        return result;
    }
}