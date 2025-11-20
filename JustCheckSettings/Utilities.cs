#region

using System;
using Dalamud.Game.Config;
using Dalamud.Utility;

#endregion

namespace JustCheckSettings;

public static class Utilities
{
    /// Gets just the enum name from a Fully Qualified Name of an enum.
    public static string GetEnumName(this string @string) =>
        @string.Split('.')[^1];

    /// Tries to get the associated GameConfigOption value for the specified setting
    public static bool TryGetGameConfigOption(this Enum enumValue, string enumName,
        out object? value)
    {
        value = null;

        var option = enumValue.GetAttribute<GameConfigOptionAttribute>();
        if (option is null)
            return false;

        return enumValue switch
        {
            SystemConfigOption sys => TryGetForSys(sys, option.Type, out value),
            UiConfigOption ui      => TryGetForCfg(ui, option.Type, out value),
            UiControlOption ctl    => TryGetForCtl(ctl, option.Type, out value),
            _                      => false,
        };
    }

    #region TryGetGameConfigOption Helpers

    private static bool TryGetForSys
        (SystemConfigOption opt, ConfigType type, out object? value)
    {
        value = null;
        switch (type)
        {
            case ConfigType.UInt:
                if (!GameConfig.TryGet(opt, out uint u)) return false;
                value = u;
                return true;
            case ConfigType.Float:
                if (!GameConfig.TryGet(opt, out float f)) return false;
                value = f;
                return true;
            case ConfigType.String:
                if (!GameConfig.TryGet(opt, out string s)) return false;
                value = s;
                return true;
            default:
                return false;
        }
    }

    private static bool TryGetForCfg
        (UiConfigOption opt, ConfigType type, out object? value)
    {
        value = null;
        switch (type)
        {
            case ConfigType.UInt:
                if (!GameConfig.TryGet(opt, out uint u)) return false;
                value = u;
                return true;
            case ConfigType.Float:
                if (!GameConfig.TryGet(opt, out float f)) return false;
                value = f;
                return true;
            case ConfigType.String:
                if (!GameConfig.TryGet(opt, out string s)) return false;
                value = s;
                return true;
            default:
                return false;
        }
    }

    private static bool TryGetForCtl
        (UiControlOption opt, ConfigType type, out object? value)
    {
        value = null;
        switch (type)
        {
            case ConfigType.UInt:
                if (!GameConfig.TryGet(opt, out uint u)) return false;
                value = u;
                return true;
            case ConfigType.Float:
                if (!GameConfig.TryGet(opt, out float f)) return false;
                value = f;
                return true;
            case ConfigType.String:
                if (!GameConfig.TryGet(opt, out string s)) return false;
                value = s;
                return true;
            default:
                return false;
        }
    }

    #endregion
}