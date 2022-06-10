/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var cancellationTokenSource = new CancellationTokenSource();

            var task = Task.Run(() => { Console.WriteLine($"Thread id: {Environment.CurrentManagedThreadId}"); })

                .ContinueWith(
                    taskA =>
                    {
                        Console.WriteLine($"a. Thread id: {Environment.CurrentManagedThreadId}");
                        throw new Exception();
                    })

                .ContinueWith(
                    taskB =>
                    {
                        Console.WriteLine($"b. Thread id: {Environment.CurrentManagedThreadId}");
                        throw new Exception();
                    },
                    TaskContinuationOptions.OnlyOnFaulted)

                .ContinueWith(
                    taskC =>
                    {
                        Console.WriteLine($"c. Thread id: {Environment.CurrentManagedThreadId}");


                        cancellationTokenSource.Cancel();
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    },
                   cancellationTokenSource.Token,
                    TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current)

                .ContinueWith(
                    taskD =>
                    {
                        Console.WriteLine($"d. Thread id: {Environment.CurrentManagedThreadId}");
                    }, new CancellationToken(),
                    TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning, TaskScheduler.Default);

            task.Wait();
            Console.ReadLine();
        }
    }
}
