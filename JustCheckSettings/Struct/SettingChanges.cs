#region

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#endregion

namespace JustCheckSettings.Struct;

[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
public static class SettingChanges
{
    private const int MaximumValues = 100;

    private static readonly Queue<SettingChange>
        ChangesQueue = new(MaximumValues);

    public static readonly Action Draw = () =>
    {
        foreach (var change in GetPreviousChanges())
            change.Draw();
    };

    public static int Count => ChangesQueue.Count;

    public static void Clear()
    {
        ChangesQueue.Clear();
    }

    public static void Add(SettingChange[] changes)
    {
        if (changes.Length == 0)
            return;

        // Remove older entries
        while (ChangesQueue.Count >= MaximumValues - changes.Length)
            ChangesQueue.Dequeue();

        // Add the new ones
        foreach (var change in changes)
            ChangesQueue.Enqueue(change);
    }

    /// Gets the most recent set of changes
    public static SettingChange? GetLatestChanges() =>
        ChangesQueue.Count == 0 ? null : ChangesQueue.Last();

    /// Gets the most recent sets of changes, excluding the most recent.
    public static List<SettingChange> GetPreviousChanges(int count = 5)
    {
        if (count <= 0 || ChangesQueue.Count < 1)
            return [];

        var arr    = ChangesQueue.ToArray();
        var result = new List<SettingChange>();

        // Start from the most recent entry, walk backwards
        for (var i = arr.Length - 1; i >= 0 && result.Count < count; i--)
            result.Add(arr[i]);

        return result;
    }
}