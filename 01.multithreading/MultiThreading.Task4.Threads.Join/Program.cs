/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore _semaphore = new Semaphore(1, 1);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            TaskA(10);
            Console.WriteLine();
            TaskB(10);

            Console.ReadLine();
        }

        private static void TaskA(int number)
        {
            if (number == 0) return;
            var thread = new Thread(() =>
            {
                number--;
                Console.WriteLine(number);
                TaskA(number);

            });
            thread.Start();
            thread.Join();
        }

        private static void TaskB(int number)
        {
            if (number == 0) return;
            ThreadPool.QueueUserWorkItem(a =>
            {
                _semaphore.WaitOne();

                number--;
                Console.WriteLine(number);
                TaskB(number);

                _semaphore.Release();
            });
        }
    }
}
