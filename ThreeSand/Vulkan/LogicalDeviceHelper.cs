using Brutal.VulkanApi;
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
public sealed class LogicalDeviceHelper : IDisposable
{
    // TODO: We only need this past the CreateDevice call, so we don't really need this native memory float arr... get rid of it
    private readonly FloatArr _queuePriorities = new ([1.0f]);
    
    public LogicalDeviceHelper(PhysicalDevice physicalDevice, int queueFamilyIndex)
    {
        var deviceQueueCreateInfo = new VkDeviceQueueCreateInfo
        {
            QueueFamilyIndex = queueFamilyIndex,
            // Setting to 1 for now, this might change if we want to do more compute for the sim in parallel. However, queue
            // families only have so many queues, so you need to find that out before you change it.
            QueueCount = _queuePriorities.Count,
            QueuePriorities = _queuePriorities,
        };

        physicalDevice.CreateDevice(deviceQueueCreateInfo);
    }

    public void Dispose()
    {
        _queuePriorities.Dispose();
    }
}