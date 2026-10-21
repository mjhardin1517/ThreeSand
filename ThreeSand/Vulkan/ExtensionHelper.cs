using System.Runtime.InteropServices;
using Brutal.VulkanApi;

namespace ThreeSand.Vulkan;

public static class ExtensionHelper
{
    public static HashSet<string> GetExtensions(IEnumerable<string> requiredExtensions, HashSet<string> layers)
    {
        HashSet<string> extensions = [..requiredExtensions];
        var possibleExtensions = GetExtensionNames();
        
        if (CheckPortability(possibleExtensions))
            extensions.Add(VK.KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME);

        // Enable debug if the validation layer is present
        if (layers.Contains(VK.LAYER_KHRONOS_VALIDATION))
        {
            if(!possibleExtensions.Contains(VK.EXT_DEBUG_UTILS_EXTENSION_NAME))
                throw new Exception("Validation layer present but debug utils extension not available");
            
            extensions.Add(VK.EXT_DEBUG_UTILS_EXTENSION_NAME);
        }

        return extensions;
    }
    
    public static bool CheckPortability(HashSet<string> extensionNames)
    {
        return extensionNames.Contains(VK.KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME);
    }

    private static HashSet<string> GetExtensionNames()
    {
        var count = Brutal.VulkanApi.Vulkan.EnumerateInstanceExtensionProperties_Count(default);
        // Using array for config setup as I don't think this is a hot path
        var props = new VkExtensionProperties[count];
        var result = Brutal.VulkanApi.Vulkan.EnumerateInstanceExtensionProperties_Write(default, props);
        var names = result == VkResult.Success
            // Now convert
            ? ConvertPropsToStrings(props, count)
            : throw new Exception("Failed to enumerate instance extension properties");

        return [..names];
    }
    
    private static unsafe string[] ConvertPropsToStrings(VkExtensionProperties[] props, int count)
    {
        var names = new string[count];
        for (var i = 0; i < count; i++)
        {
            fixed (byte* p = props[i].ExtensionName)
            {
                names[i] = Marshal.PtrToStringUTF8((IntPtr)p)!;
            }
        }

        return names;
    }
}