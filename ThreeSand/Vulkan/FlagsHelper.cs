using Brutal.VulkanApi;

namespace ThreeSand.Vulkan;

public static class FlagsHelper
{
    public static VkInstanceCreateFlags CreateFlags(bool isPortability)
    {
        const VkInstanceCreateFlags flags = VkInstanceCreateFlags.None;

        return isPortability
            ? flags | VkInstanceCreateFlags.EnumeratePortabilityBitKHR
            : flags;
    }
}