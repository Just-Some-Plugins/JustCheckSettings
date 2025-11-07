#region

using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Config;
using Dalamud.Utility;
using JustCheckSettings.Struct;

#endregion

namespace JustCheckSettings.Logic;

public static class Check
{
    private static Dictionary<string, Dictionary<object, object>> _oldCfg     = [];
    private static Dictionary<string, Dictionary<object, object>> _currentCfg = [];

    public static readonly Action Task = () =>
    {
        _oldCfg     = _currentCfg;
        _currentCfg = [];
        CheckEnum<SystemConfigOption>();
        CheckEnum<UiConfigOption>();
        CheckEnum<UiControlOption>();

        if (Diff.TryGet(_oldCfg, _currentCfg, out var changes))
        {
            SettingChanges.Add(changes);

            Log.Debug(changes.Aggregate("", (_, change) =>
                $"{change.EnumFullName}.{change.EnumKey} " +
                $"`{change.OldValue ?? "?"}` -> `{change.NewValue ?? "?"}`, "));
        }
    };

    /// Gets all values of all GameConfigOptions within a specified Enum.
    private static void CheckEnum<TEnumToCheck>() where TEnumToCheck : Enum
    {
        var @enum        = typeof(TEnumToCheck);
        var enumName     = @enum.Name;
        var enumFullName = @enum.FullName;

        if (enumFullName is null)
            return;

        _currentCfg[enumFullName] = [];

        foreach (Enum value in Enum.GetValues(@enum))
        {
            // ??
            if (value is null)
                continue;

            var type = value.GetAttribute<GameConfigOptionAttribute>()?
                .Type;

            // Ignore non-valued settings
            if (type is ConfigType.Unused or ConfigType.Category)
                continue;

            if (value.TryGetGameConfigOption(enumName, out var optionValue))
                _currentCfg[enumFullName][value] = optionValue!;
        }
    }
}