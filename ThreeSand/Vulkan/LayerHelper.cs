using System.Runtime.InteropServices;
using Brutal.VulkanApi;

namespace ThreeSand.Vulkan;

public static class LayerHelper
{
    public static HashSet<string> GetLayers(bool isDebug)
    {
        var count = Brutal.VulkanApi.Vulkan.EnumerateInstanceLayerProperties_Count();
        var props = new VkLayerProperties[count];
        var result = Brutal.VulkanApi.Vulkan.EnumerateInstanceLayerProperties_Write(props);
        HashSet<string> existingLayers = result == VkResult.Success
            ? [..ConvertPropsToStrings(props, count)]
            : throw new Exception("Failed to enumerate instance layer properties");
        HashSet<string> layers = [];


        if (isDebug)
        {
            if (!existingLayers.Contains(VK.LAYER_KHRONOS_VALIDATION))
                throw new Exception("Validation layer not found");

            layers.Add(VK.LAYER_KHRONOS_VALIDATION);
        }

        return layers;
    }

    private static unsafe string[] ConvertPropsToStrings(VkLayerProperties[] props, int count)
    {
        var names = new string[count];
        for (var i = 0; i < count; i++)
        {
            fixed (byte* p = props[i].LayerName)
            {
                names[i] = Marshal.PtrToStringUTF8((IntPtr)p)!;
            }
        }

        return names;
    }
}