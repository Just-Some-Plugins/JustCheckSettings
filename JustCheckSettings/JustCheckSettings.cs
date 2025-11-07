#region

global using static JustCheckSettings.JustCheckSettings;
using System;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using JetBrains.Annotations;
using JustCheckSettings.UI;
using JustCheckSettings.Work;
using IPluginInterface = Dalamud.Plugin.IDalamudPluginInterface;
using ICommands = Dalamud.Plugin.Services.ICommandManager;

#endregion

namespace JustCheckSettings;

[UsedImplicitly]
public class JustCheckSettings : IDalamudPlugin
{
    private readonly ChangesDisplay _changesDisplay = new();
    private readonly Watcher        _watcher        = new();
    private readonly WindowSystem   _windowSystem   = new();

    public JustCheckSettings(IPluginInterface pluginInterface)
    {
        _windowSystem.AddWindow(_changesDisplay);

        Framework.RunOnTick(Watcher.Worker, delay: TimeSpan.FromSeconds(1));

        Interface.UiBuilder.OpenMainUi += _changesDisplay.Open;
        Interface.UiBuilder.Draw       += _windowSystem.Draw;

        Commands.AddHandler($"/{pluginInterface.InternalName.ToLower()}",
            new CommandInfo(OpenMainCommand)
                { HelpMessage = "Open the Window to check for Setting Changes" });
    }

    public void Dispose()
    {
        Interface.UiBuilder.OpenMainUi -= _changesDisplay.Open;
        Interface.UiBuilder.Draw       -= _windowSystem.Draw;
        _watcher.Dispose();
    }

    private void OpenMainCommand(string _, string __)
    {
        _changesDisplay.Open();
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