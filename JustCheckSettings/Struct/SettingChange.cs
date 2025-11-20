#region

using System;
using Dalamud.Bindings.ImGui;
using TextCopy;

#endregion

namespace JustCheckSettings.Struct;

public record SettingChange
{
    public required DateTime When { get; init; }
    public required string EnumFullName { get; init; }
    public required Enum EnumKey { get; init; }
    public required object? OldValue { get; init; }
    public required object? NewValue { get; init; }

    public void Draw()
    {
        ImGui.TableNextRow();
        ImGui.TableNextColumn();
        ImGui.Text($"{When:HH:mm:ss}");

        ImGui.TableNextColumn();
        ImGui.Text(
            $"{EnumFullName.GetEnumName()}.{EnumKey.ToString()}");

        ImGui.TableNextColumn();
        ImGui.Text(OldValue?.ToString() ?? "?");

        ImGui.TableNextColumn();
        ImGui.Text(NewValue?.ToString() ?? "?");

        ImGui.TableNextColumn();
        if (ImGui.Button("Copy Reference"))
            ClipboardService.SetText($"{EnumFullName}.{EnumKey}");
    }
}