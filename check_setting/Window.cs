#region

using System.Numerics;
using Dalamud.Bindings.ImGui;

#endregion

namespace check_setting;

public class Window : Dalamud.Interface.Windowing.Window
{
    public Window() : base("Just Check Setting")
    {
        Flags = ImGuiWindowFlags.AlwaysAutoResize;
    }

    public override void Draw()
    {
        if (SettingChanges.Count < 1)
        {
            ImGui.Text("No Setting Changes Detected");
            return;
        }

        ImGui.Dummy(new Vector2(0, 10));

        ImGui.Text("Most Recent Setting Changes:");
        SettingChanges.GetLatestChanges().Draw();

        if (SettingChanges.Count > 1)
        {
            ImGui.Dummy(new Vector2(0, 10));
            ImGui.Text("Previous Setting Changes:");

            foreach (var (key, changes) in SettingChanges.GetPreviousChanges())
                changes.Draw(key);
        }

        ImGui.Dummy(new Vector2(0, 10));

        var size = ImGui.GetContentRegionAvail();
        // ReSharper disable once UseWithExpressionToCopyStruct
        if (ImGui.Button("Clear Recorded Changes", new Vector2(size.X, 20)))
            SettingChanges.Clear();

        ImGui.Dummy(new Vector2(0, 10));
    }

    public void Open()
    {
        IsOpen = true;
    }
}