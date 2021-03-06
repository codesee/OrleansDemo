using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemoCluster.Api
{
    public class ActionDispatcher : IActionDispatcher
    {
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();
        private TaskScheduler scheduler;

        public ActionDispatcher(TaskScheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        public bool CanDispatch()
        {
            return scheduler != null;
        }

        public Task DispatchAsync(Func<Task> action)
        {
            if (scheduler == null)
            {
                throw new InvalidOperationException("The dispatcher has already been closed.");
            }

            return Task<Task>.Factory.StartNew(action, cts.Token, TaskCreationOptions.None, scheduler).Unwrap();
        }

        public Task<T> DispatchAsync<T>(Func<Task<T>> action)
        {
            if (scheduler == null)
            {
                throw new InvalidOperationException("The dispatcher has already been closed.");
            }

            return Task<Task<T>>.Factory.StartNew(action, cts.Token, TaskCreationOptions.DenyChildAttach, scheduler).Unwrap();
        }

        public void Dispose()
        {
            cts.Cancel();

            scheduler = null;
        }
    }
}
