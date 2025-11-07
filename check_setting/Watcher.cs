#region

using System;
using System.Threading;
using static System.Threading.Tasks.Task;

#endregion

namespace check_setting;

public class Watcher : IDisposable
{
    /// How often to check for changes
    private static readonly TimeSpan CheckFrequency = TimeSpan.FromSeconds(0.5);

    /// When the last Check was
    private static readonly DateTime LastCheck = DateTime.MinValue;

    /// The Cancellation Token Controller
    private static readonly CancellationTokenSource Cancellation =
        new();

    public static readonly Action Task = () =>
    {
        if (Cancellation.IsCancellationRequested)
            return;

        if ((DateTime.Now - LastCheck) < CheckFrequency)
            return;

        Run(Checker.Check, Cancellation.Token);

        Framework.RunOnTick(Task!,
            delay: CheckFrequency,
            cancellationToken: Cancellation.Token);
    };

    public void Dispose()
    {
        SettingChanges.Clear();
        Cancellation.Cancel();
    }
}