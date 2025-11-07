#region

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

#endregion

namespace check_setting;

[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
public static class SettingChanges
{
    private const int MaximumValues = 100;

    private static readonly Queue<KeyValuePair<DateTime, SettingChange[]>>
        ChangesQueue = new(MaximumValues);

    public static int Count => ChangesQueue.Count;

    public static void Clear()
    {
        ChangesQueue.Clear();
    }

    public static void Add(SettingChange[] changes, DateTime? when = null)
    {
        if (changes.Length == 0)
            return;

        var realWhen = when ?? DateTime.Now;

        while (ChangesQueue.Count >= MaximumValues)
            ChangesQueue.Dequeue();
        ChangesQueue.Enqueue(
            new KeyValuePair<DateTime, SettingChange[]>(realWhen, changes));
    }

    /// Gets the most recent set of changes
    public static SettingChange[]? GetLatestChanges() =>
        ChangesQueue.Count == 0 ? null : ChangesQueue.Last().Value;

    /// Gets the most recent sets of changes, excluding the most recent.
    public static List<KeyValuePair<DateTime, SettingChange[]>>
        GetPreviousChanges(int count = 5)
    {
        if (count <= 0 || ChangesQueue.Count <= 1)
            return [];

        var arr = ChangesQueue.ToArray();
        var result =
            new List<KeyValuePair<DateTime, SettingChange[]>>();

        // Start from the second-last entry, walk backwards
        for (var i = arr.Length - 2; i >= 0 && result.Count < count; i--)
            result.Add(arr[i]);

        return result;
    }
}