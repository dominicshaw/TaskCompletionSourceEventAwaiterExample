using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCompletionSourceEventAwaiterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();

            Console.WriteLine("I have completed all operations - both sync and async.");
            Console.WriteLine("Press ENTER to quit.");
            Console.ReadLine();
        }

        private static async Task Run()
        {
            Console.WriteLine("Welcome to the application");
            Console.WriteLine("Performing sync operations...");
            Thread.Sleep(1000);
            Console.WriteLine("Completed sync operations, starting async operations...");
            
            var tcs = new TaskCompletionSource<int>();

            var eventBasedAsyncJob = new EventBasedAsynchronousJob(TimeSpan.FromSeconds(2), 2, 4);
            eventBasedAsyncJob.WorkComplete += z => tcs.SetResult(z);

            var result = await tcs.Task;

            Console.WriteLine("Result of async operation: " + result);
        }
    }

    public class EventBasedAsynchronousJob
    {
        public delegate void WorkCompleteEventHandler(int result);
        public WorkCompleteEventHandler WorkComplete;

        public EventBasedAsynchronousJob(TimeSpan timeout, int x, int y)
        {
            new Timer(_ => WorkComplete?.Invoke(x + y), null, timeout, Timeout.InfiniteTimeSpan);
        }
    }
}
