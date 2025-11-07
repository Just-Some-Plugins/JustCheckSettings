#region

using System;
using Dalamud.Bindings.ImGui;
using TextCopy;

#endregion

namespace JustCheckSettings.Struct;

public record SettingChange
{
    public required string EnumFullName { get; init; }
    public required Enum EnumKey { get; init; }
    public required object? OldValue { get; init; }
    public required object? NewValue { get; init; }

    public void Draw()
    {
        ImGui.Text($"{EnumFullName.GetEnumName()}." +
                   $"{EnumKey.ToString().PadRight(40)} " +
                   $"`{OldValue ?? "?"}` > `{NewValue ?? "?"}`");
        ImGui.SameLine();
        if (ImGui.Button("Copy FQDN"))
            ClipboardService.SetText($"{EnumFullName}.{EnumKey}");
    }
}