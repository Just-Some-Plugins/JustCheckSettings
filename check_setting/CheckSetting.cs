#region

global using static check_setting.CheckSetting;
using System;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using JetBrains.Annotations;
using IPluginInterface = Dalamud.Plugin.IDalamudPluginInterface;
using ICommands = Dalamud.Plugin.Services.ICommandManager;

#endregion

namespace check_setting;

[UsedImplicitly]
public class CheckSetting : IDalamudPlugin
{
    private readonly Watcher      _watcher      = new();
    private readonly Window       _window       = new();
    private readonly WindowSystem _windowSystem = new();

    public CheckSetting(IPluginInterface pluginInterface)
    {
        _windowSystem.AddWindow(_window);

        Framework.RunOnTick(Watcher.Task, delay: TimeSpan.FromSeconds(1));

        Interface.UiBuilder.OpenMainUi += _window.Open;
        Interface.UiBuilder.Draw       += _windowSystem.Draw;

        Commands.AddHandler($"/{pluginInterface.InternalName.ToLower()}",
            new CommandInfo(OpenCommand)
                { HelpMessage = "Open the Window to check for Setting Changes" });
    }

    public void Dispose()
    {
        Interface.UiBuilder.OpenMainUi -= _window.Open;
        Interface.UiBuilder.Draw       -= _windowSystem.Draw;
        _watcher.Dispose();
    }

    public void OpenCommand(string _, string __)
    {
        _window.Open();
    }

    #region Dalamud Properties

    [PluginService]
    public static IFramework Framework { get; [UsedImplicitly] private set; }
        = null!;

    [PluginService]
    public static IGameConfig GameConfig { get; [UsedImplicitly] private set; }
        = null!;

    [PluginService]
    public static IPluginLog Log { get; [UsedImplicitly] private set; }
        = null!;

    [PluginService]
    private static IPluginInterface Interface { get; [UsedImplicitly] set; }
        = null!;

    [PluginService]
    public static ICommands Commands { get; [UsedImplicitly] set; }
        = null!;

    #endregion
}