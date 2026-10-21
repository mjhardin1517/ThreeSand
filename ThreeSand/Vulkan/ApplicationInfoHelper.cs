using Brutal.VulkanApi;
using ThreeSand.Interop;

namespace ThreeSand.Vulkan;

internal sealed class ApplicationInfoHelper : IDisposable
{
    private readonly Utf8Arena _utf8Arena = new();

    public VkApplicationInfo Create()
    {
        var applicationName = _utf8Arena.Create("ThreeSand");
        var engineName = _utf8Arena.Create("BRUTAL");

        return new VkApplicationInfo
        {
            ApplicationName = applicationName,
            ApplicationVersion = VulkanVersion.Make(26, 10, 0),
            EngineName = engineName,
            EngineVersion = VulkanVersion.Make(26, 10, 2),
            // Vulkan ignores patch version so we just pass in 0
            ApiVersion = (int)MyConstants.RequiredApiVersion,
        };
    }

    public void Dispose()
    {
        _utf8Arena.Dispose();
    }
}