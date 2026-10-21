using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace ThreeSand.Vulkan;

public sealed class DebugHelper : IDisposable
{
    private readonly Instance _instance;

    private readonly VkDebugUtilsMessengerEXT? _messenger;

    public DebugHelper(Instance instance, VkDebugUtilsMessengerCreateInfoEXT? info)
    {
        _instance = instance;
        if (info is { } someInfo)
            _messenger = CreateMessenger(someInfo);
    }

    public static unsafe VkDebugUtilsMessengerCreateInfoEXT CreateInfo()
    {
        return VulkanHelpers.BuildDefaultDebugMessengerCreateInfo(
            new PfnVkDebugUtilsMessengerCallbackEXT(&VulkanHelpers.DebugUtilsMessengerCallbacks.Default));
    }

    private unsafe VkDebugUtilsMessengerEXT? CreateMessenger(VkDebugUtilsMessengerCreateInfoEXT info)
    {
        if (!_instance.IsExtensionEnabled(VK.EXT_DEBUG_UTILS_EXTENSION_NAME))
            return null;

        // We could add an allocator instead of null. That is for a specific use case for if you need to track the memory
        return _instance.CreateDebugUtilsMessengerEXT(info, null);
    }

    public void Dispose()
    {
        // Check if messenger is null and pattern match into local variable
        if (_messenger is { } messenger)
        {
            _instance.DestroyDebugUtilsMessengerEXT(messenger, null);
        }
    }
}