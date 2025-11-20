#region

using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using JustCheckSettings.Struct;

#endregion

namespace JustCheckSettings.UI;

public class ChangesDisplay : Window
{
    public ChangesDisplay() : base("Just Check Settings##JustCheckSettings")
    {
        Flags = ImGuiWindowFlags.AlwaysAutoResize;
    }

    public override void Draw()
    {
        ImGui.Dummy(new Vector2(0, 10));

        if (SettingChanges.Count < 1)
        {
            ImGui.Dummy(new Vector2(175, 10));
            ImGui.Text("No Setting Changes Detected Yet.");
            ImGui.Dummy(new Vector2(0, 10));
            return;
        }

        ImGui.Text("Most Recent Setting Changes:");

        SetupTable(SettingChanges.Draw);

        ImGui.Dummy(new Vector2(0, 10));

        var size = ImGui.GetContentRegionAvail();
        // ReSharper disable once UseWithExpressionToCopyStruct
        if (ImGui.Button("Clear Recorded Changes", new Vector2(size.X, 20)))
            SettingChanges.Clear();

        ImGui.Dummy(new Vector2(0, 10));
    }

    private void SetupTable(Action tableContents)
    {
        using var table = ImRaii.Table("##changesTable", 5,
            ImGuiTableFlags.SizingStretchProp);
        if (!table)
        {
            ImGui.Text("COULD NOT DRAW TABLE");
            return;
        }

        ImGui.TableSetupColumn("Time", initWidthOrWeight: 1);
        ImGui.TableSetupColumn("Setting", initWidthOrWeight: 4);
        ImGui.TableSetupColumn("Old Value", initWidthOrWeight: 1);
        ImGui.TableSetupColumn("New Value", initWidthOrWeight: 1);
        ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 100);

        ImGui.TableHeadersRow();

        tableContents();
    }

    public void Open()
    {
        IsOpen = true;
    }
}