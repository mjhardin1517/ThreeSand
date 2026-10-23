using System.Runtime.InteropServices;

namespace ThreeSand.Interop;

public sealed unsafe class FloatArr : IDisposable
{
    public int Count { get; private set; }
        
    private float* _ns;
    
    public FloatArr(IReadOnlyList<float> ns)
    {
        _ns = (float*)NativeMemory.Alloc((nuint)ns.Count, (nuint)sizeof(float*));
        for (var i = 0; i < ns.Count; i++)
        {
            _ns[i] = ns[i];
        }
        Count = ns.Count;
    }
    
    public static implicit operator float*(FloatArr floatArr) => floatArr._ns;
    
    public void Dispose()
    {
        NativeMemory.Free(_ns);
        _ns = null;
        Count = 0;
    }
    
}