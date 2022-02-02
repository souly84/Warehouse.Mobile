using System;
using System.Threading;
using System.Threading.Tasks;

namespace Warehouse.Mobile.IntegrationTests
{
    public static class TaskExtensions
    {
        public static Task WithTimeout(this Task task, int milliseconds)
        {
            return task.WithTimeout(
                TimeSpan.FromMilliseconds(milliseconds)
            );
        }

        public static async Task WithTimeout(this Task task, TimeSpan timeout)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            var tcs = new TaskCompletionSource<bool>();
            using (var cts = new CancellationTokenSource())
            {
                using (var registration = cts.Token.Register(() => tcs.TrySetCanceled()))
                {
                    cts.CancelAfter(timeout);
                    _ = task
                        .ContinueWith(
                            t =>
                            {
                                if (t.Exception != null)
                                {
                                    tcs.TrySetException(t.Exception.InnerException);
                                }
                                else if (t.IsCanceled)
                                {
                                    tcs.TrySetCanceled();
                                }
                                else
                                {
                                    tcs.TrySetResult(true);
                                }
                            },
                            TaskScheduler.Current
                        );

                    await tcs.Task.ConfigureAwait(false);
                }
            }
        }
    }
}
