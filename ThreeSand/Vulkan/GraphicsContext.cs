using Brutal.Logging;
using Brutal.VulkanApi;

namespace ThreeSand.Vulkan;

/// <summary>
/// NOTE: There are string utilities in Brutal.String, this may not be necessary
/// </summary>
public sealed class GraphicsContext : IDisposable
{
    private readonly Instance _instance;
    private readonly DebugHelper _debugHelper;
    private readonly Stack<IDisposable> _disposables = new();
    /// <summary>
    /// Where `int` is the queue family
    /// </summary>
    private readonly (PhysicalDevice, int) _physicalDevice;

    public unsafe GraphicsContext(IEnumerable<string> requiredExtensions)
    {
        var isDebug = false;
#if DEBUG
        DefaultCategory.Log.Info("Debug mode enabled");
        isDebug = true;
#endif



        try
        {
            using var applicationInfoHelper = new ApplicationInfoHelper();
            var layerSet = LayerHelper.GetLayers(isDebug);
            var extensionSet = ExtensionHelper.GetExtensions(requiredExtensions, layerSet);
            var flags = FlagsHelper.CreateFlags(ExtensionHelper.CheckPortability(extensionSet));
            VkDebugUtilsMessengerCreateInfoEXT debugInfo = DebugHelper.CreateInfo();

            var deviceQueue = new VkDeviceQueueCreateInfo
            {

            };
            
            _instance = Brutal.VulkanApi.Vulkan.CreateInstance(new Instance.CreateInfo
            {
                AppInfo = applicationInfoHelper.Create(),
                // Portability is required for macos, we need to set the flag and enable the extension
                // Flags are bits; to combine them we use the bitwise OR operator
                Flags = flags,
                EnabledExtensions = extensionSet,
                EnabledLayers = layerSet,
                // Right now we only have one extension. Vulkan calls these in a chain. If we wanted to dynamically add
                // more, we would add the latest one here and point the head of the chain to the latest.
                // We have to do the ternary here, if you make a variable nullable it changes its type
                Next = isDebug ? &debugInfo : null, 
                
                
            });
            _disposables.Push(_instance);

            _debugHelper = new DebugHelper(_instance, debugInfo);
            _disposables.Push(_debugHelper);
            
            // Get the physical device along with its queue family
            _physicalDevice = PhysicalDeviceHelper.Get(_instance);
        }
        catch (Exception e)
        {
            Dispose();
            DefaultCategory.Log.Error(e);
            throw;
        }
    }


    public void Dispose()
    {
        // Use while with pop so we don't run into issues on a double dispose
        while (_disposables.Count > 0)
        {
            var item = _disposables.Pop();
            item.Dispose();
        }
    }
}