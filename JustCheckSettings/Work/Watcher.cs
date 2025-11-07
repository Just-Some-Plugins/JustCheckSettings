#region

using System;
using System.Threading;
using System.Threading.Tasks;
using JustCheckSettings.Logic;
using JustCheckSettings.Struct;

#endregion

namespace JustCheckSettings.Work;

public class Watcher : IDisposable
{
    /// How often to check for changes
    private static readonly TimeSpan CheckFrequency = TimeSpan.FromSeconds(0.5);

    /// When the last Check was
    private static readonly DateTime LastCheck = DateTime.MinValue;

    /// The Cancellation Token Controller
    private static readonly CancellationTokenSource Cancellation =
        new();

    public static readonly Action Worker = () =>
    {
        if (Cancellation.IsCancellationRequested)
            return;

        if ((DateTime.Now - LastCheck) < CheckFrequency)
            return;

        Task.Run(Check.Task, Cancellation.Token);

        Framework.RunOnTick(Worker!,
            delay: CheckFrequency,
            cancellationToken: Cancellation.Token);
    };

    public void Dispose()
    {
        SettingChanges.Clear();
        Cancellation.Cancel();
    }
}