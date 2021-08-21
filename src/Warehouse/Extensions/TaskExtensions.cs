namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Fires the <see cref="Task" /> and safely forget.
        /// </summary>
        /// <param name="task">The task.</param>
        public static void FireAndForget(this Task task)
        {
            task.FireAndForget(null);
        }

        /// <summary>
        /// Fires the <see cref="Task"/> and safely forget and in case of exception call <paramref name="onError"/> handler if any.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="onError">The on error action handler.</param>
        public static async void FireAndForget(this Task task, Action<Exception> onError)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }

        public static async Task Then(this Task task, Action<Task> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            await task.ConfigureAwait(false);
            next(task);
        }

        public static async Task Then(this Task task, Action next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            await task.ConfigureAwait(false);
            next();
        }

        public static async Task Then<T>(this Task<T> task, Action<T, Task> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            var result = await task.ConfigureAwait(false);
            next(result, task);
        }

        public static async Task Then<T>(this Task<T> task, Action<T> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            var result = await task.ConfigureAwait(false);
            next(result);
        }

        public static async Task Then(this Task task, Func<Task, Task> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            await task.ConfigureAwait(false);
            await next(task).ConfigureAwait(false);
        }

        public static async Task Then(this Task task, Func<Task> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            await task.ConfigureAwait(false);
            await next().ConfigureAwait(false);
        }

        public static async Task<T> Then<T>(this Task task, Func<Task<T>> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            await task.ConfigureAwait(false);
            return await next().ConfigureAwait(false);
        }

        public static async Task Then<T>(this Task<T> task, Func<T, Task, Task> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            var result = await task.ConfigureAwait(false);
            await next(result, task).ConfigureAwait(false);
        }

        public static async Task<T2> Then<T1, T2>(this Task<T1> task, Func<T1, Task<T2>> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            var result = await task.ConfigureAwait(false);
            return await next(result).ConfigureAwait(false);
        }

        public static async Task Then<T>(this Task<T> task, Func<T, Task> next)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));
            _ = next ?? throw new ArgumentNullException(nameof(next));
            var result = await task.ConfigureAwait(false);
            await next(result).ConfigureAwait(false);
        }
    }
}
