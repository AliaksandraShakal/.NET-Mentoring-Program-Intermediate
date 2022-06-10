/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> _collection = new List<int>();
        private static object _locker = new object();
        private static bool _isReady = false;
        private static bool _isReadyTask2 = false;
        private static Barrier _barrier = new Barrier(2);
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var task1 = Task.Run(Task1);
            var task2 = Task.Run(Task2);

            Task.WaitAll(task1, task2);

            Console.ReadLine();
        }

        private static void Task1()
        {
            for (var i = 0; i < 10; i++)
            {
                lock (_locker)
                {
                    _collection.Add(i);
                    Console.WriteLine($"Create {i}");
                    _isReadyTask2 = false;
                }
                _barrier.SignalAndWait();
            }
            _isReady = true;
        }

        private static void Task2()
        {
            while (!_isReady)
            {
                if (!_isReadyTask2)
                {
                    lock (_locker)
                    {
                        _collection.ForEach(i => Console.Write(i + " "));
                        Console.WriteLine();
                        _isReadyTask2 = true;
                    }
                    _barrier.SignalAndWait();
                }
            }
        }
    }
}
