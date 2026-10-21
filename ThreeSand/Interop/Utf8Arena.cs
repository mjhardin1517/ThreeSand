namespace ThreeSand.Interop;

/// <summary>
/// 
/// </summary>
public sealed class Utf8Arena : IDisposable
{
    // I need to do some research on whether using List matters for hot paths. I'm only really using this for config right
    // now so I don't think I care.
    private readonly List<Utf8> _allocations = [];

    public Utf8 Create(string str)
    {
        var utf8 = new Utf8(str);
        _allocations.Add(utf8);

        return utf8;
    }

    public void Dispose()
    {
        foreach (var allocation in _allocations)
        {
            allocation.Dispose();
        }

        _allocations.Clear();
    }
}