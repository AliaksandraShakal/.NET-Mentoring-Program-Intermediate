/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Threading.Tasks;
using System.Linq;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private static Random random = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var random = new Random();

            Task.Run(Task1)
                    .ContinueWith(task => Task2(task.Result))
                    .ContinueWith(task => Task3(task.Result))
                    .ContinueWith(task => Task4(task.Result));

            Console.ReadLine();
        }

        private static void Task4(int[] array)
        {
            var mean = array.Average();
            Console.WriteLine($"{mean} - sorted array");
        }

        private static int[] Task3(int[] array)
        {
            Array.Sort(array);
            for (var i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine(" - sorted array");
            return array;
        }

        private static int[] Task2(int[] array)
        {
            var randomNumber = random.Next(0, 10);

            for (var i = 0; i < array.Length; i++)
            {
                array[i] *= randomNumber;
                Console.Write(array[i] + " ");
            }
            Console.WriteLine($" - my array * {randomNumber}");
            return array;
        }


        private static int[] Task1()
        {
            var array = new int[10];

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(0, 10);
                Console.Write(array[i] + " ");
            }
            Console.WriteLine(" - my array");
            return array;
        }
    }
}
