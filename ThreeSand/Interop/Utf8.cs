using System.Runtime.InteropServices;

namespace ThreeSand.Interop;

public sealed unsafe class Utf8(string str) : IDisposable
{
    private byte* _ptr = (byte*)Marshal.StringToCoTaskMemUTF8(str);

    public static implicit operator byte*(Utf8 utf8) => utf8._ptr;

    public void Dispose()
    {
        // Freeing memory requires explicitly casting to nint
        Marshal.FreeCoTaskMem((nint)_ptr);
        _ptr = null;
    }
}