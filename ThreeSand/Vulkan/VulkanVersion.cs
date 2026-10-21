namespace ThreeSand.Vulkan;

public static class VulkanVersion
{
    // Something that should approximate the VK_MAKE_VERSION macro from the tutorial
    public static int Make(int major, int minor, int patch)
        => (major << 22) | (minor << 12) | patch; // Packing bits
}