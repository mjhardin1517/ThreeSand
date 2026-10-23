using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using ThreeSand.Interop;

namespace ThreeSand.Vulkan;

/// <summary>
/// Where the physical device is only information that can be read, the logical device describes the connection to that
/// physical device.
///
/// What queues do I want?
/// What features will I use?
/// What extensions do I need?
///
/// Sticking with plain `Device` for now, `DeviceEx` will matter when we get to buffers.
///
/// You record many commands into a command buffer. Those commands are then sent to a queue.
/// </summary>
public static class LogicalDeviceHelper
{
    public static unsafe Device Create(PhysicalDevice physicalDevice, int queueFamilyIndex)
    {
        float[] queuePriorities = [1.0f];

        HashSet<string> extensions = [];
        VulkanHelpers.AddMandatoryIfSupportedDeviceExtensions(extensions, physicalDevice);

        // ptr only needs to exist through call to CreateDevice
        fixed (float* queuePrioritiesPtr = queuePriorities)
        {
            var deviceQueueCreateInfo = new VkDeviceQueueCreateInfo
            {
                QueueFamilyIndex = queueFamilyIndex,
                // Setting to 1 for now, this might change if we want to do more compute for the sim in parallel. However, queue
                // families only have so many queues, so you need to find that out before you change it.
                QueueCount = queuePriorities.Length,
                QueuePriorities = queuePrioritiesPtr,
            };

            var device = physicalDevice.CreateDevice(new Device.CreateInfo
            {
                Extensions = extensions,
                // This is in a collection because there can be an info per QUEUE FAMILY
                QueueCreateInfos = [deviceQueueCreateInfo],
            });

            return device;
        }
    }
}