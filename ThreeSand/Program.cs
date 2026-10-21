using Brutal.GlfwApi;
using Brutal.Logging;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Microsoft.Extensions.Logging;
using ThreeSand.Vulkan;
using ThreeSand.Window;

// VulkanHelpers.VerboseLoggingEnabled = true;
LogSystem.Initialize((builder) => builder.AddConsole());
DefaultCategory.Log.Info("Hello there");

if (!Glfw.Init())
    throw new Exception("Failed to initialize GLFW");

Glfw.WindowHint(GlfwWindowHint.ClientApi, 0);
var window = Glfw.CreateWindow(new GlfwWindow.CreateInfo
{
    Title = "Hello BRUTAL",
    Size = new int2(1280, 720),
});

// Get the extensions that Glfw requires and pass it to Vulkan
var extensions = WindowHelper.GetRequiredInstanceExtensions();
using var graphicsInstance = new GraphicsContext(extensions);


while (!window.ShouldClose)
    Glfw.PollEvents();

Glfw.Terminate();