#region

using System;
using System.Collections.Generic;
using JustCheckSettings.Struct;

#endregion

namespace JustCheckSettings.Logic;

public static class Diff
{
    /// <summary>
    ///     Produce a list of setting changes between two saved game configurations.
    /// </summary>
    public static bool TryGet(
        Dictionary<string, Dictionary<object, object>> oldCfg,
        Dictionary<string, Dictionary<object, object>> currentCfg,
        out SettingChange[] differences)
    {
        var changes = new List<SettingChange>();

        foreach (var (enumFullName, enumValues) in currentCfg)
        {
            // Enum didn't exist in old
            if (!oldCfg.TryGetValue(enumFullName, out var oldValues))
                continue;

            foreach (var (key, newValue) in enumValues)
            {
                // Enum Value didn't exist in old
                if (!oldValues.TryGetValue(key, out var oldValue))
                    continue;

                if (!newValue.Equals(oldValue))
                {
                    changes.Add(new SettingChange
                    {
                        When         = DateTime.Now,
                        EnumFullName = enumFullName,
                        EnumKey      = (Enum)key,
                        OldValue     = oldValue,
                        NewValue     = newValue,
                    });
                }
            }
        }

        differences = changes.ToArray();
        return differences.Length > 0;
    }
}