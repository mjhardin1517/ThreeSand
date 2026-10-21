using System.Runtime.InteropServices;

namespace ThreeSand.Interop;

public sealed unsafe class Utf8Arr : IDisposable
{
    /// <summary>
    /// _strs is not strictly necessary, but makes it convenient to dispose of the Utf8 instances.
    /// </summary>
    private readonly Utf8[] _strs;

    private byte** _ptrs;

    public Utf8Arr(IReadOnlyList<string> strs)
    {
        _strs = new Utf8[strs.Count];
        _ptrs = (byte**)NativeMemory.Alloc((nuint)strs.Count, (nuint)sizeof(byte*));
        for (var i = 0; i < strs.Count; i++)
        {
            _strs[i] = new Utf8(strs[i]);
            _ptrs[i] = _strs[i];
        }
    }
    
    public int Count => _strs.Length;

    public void Dispose()
    {
        foreach (var str in _strs)
            str.Dispose();
        NativeMemory.Free(_ptrs);
        _ptrs = null;
    }
}