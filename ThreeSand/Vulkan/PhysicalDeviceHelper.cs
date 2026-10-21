using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace ThreeSand.Vulkan;

public static class PhysicalDeviceHelper
{
    private const VkQueueFlags QueueFlags = VkQueueFlags.GraphicsBit | VkQueueFlags.ComputeBit;

    public static (PhysicalDevice, int) Get(Instance instance)
    {
        (PhysicalDevice?, int) left = (null, -1);

        foreach (var physicalDevice in instance.PhysicalDevices)
        {
            var rightDeviceProps = physicalDevice.GetProperties();
            var rightQueueFamily = physicalDevice.FindQueueFamily(QueueFlags);
            var isRightMinimallyViable = rightDeviceProps.ApiVersion >= (int)MyConstants.RequiredApiVersion && rightQueueFamily != -1;

            left = (left.Item1, physicalDevice) switch
            {
                // Important initial check, if right is not minimally viable there is not point in continuing
                var (_, _) when !isRightMinimallyViable => left,
                // If left is null we take right
                (null, var right) => (right, rightQueueFamily),
                (not null, var right) when AreRightPropsBetter(left.Item1.GetProperties(), rightDeviceProps) => (right, rightQueueFamily),
                _ => left
            };
        }
        
        return (left.Item1 == null ? throw new Exception("no compatible physical device found") : left)!;
    }
    
    private static bool AreRightPropsBetter(VkPhysicalDeviceProperties left, VkPhysicalDeviceProperties right)
    {
        return true switch
        {
            // First and formost we want a discrete GPU to win
            _ when right.DeviceType  == VkPhysicalDeviceType.DiscreteGpu && left.DeviceType != VkPhysicalDeviceType.DiscreteGpu => true,
            // Since discrete is more important, we take left even if right has a higher api version
            _ when left.DeviceType == VkPhysicalDeviceType.DiscreteGpu => true,
            // Presumably both are discrete, so we pick the one with the greater API version
            _ when right.ApiVersion > left.ApiVersion => true,
            // Fall back to the left
            _ => false
        };
    }
}