﻿/*
* Study the code of this application to calculate the sum of integers from 0 to N, and then
* change the application code so that the following requirements are met:
* 1. The calculation must be performed asynchronously.
* 2. N is set by the user from the console. The user has the right to make a new boundary in the calculation process,
* which should lead to the restart of the calculation.
* 3. When restarting the calculation, the application should continue working without any failures.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal class Program
{
    /// <summary>
    /// The Main method should not be changed at all.
    /// </summary>
    /// <param name="args"></param>
    /// 

    private static void Main(string[] args)
    {
        Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
        Console.WriteLine("Calculating the sum of integers from 0 to N.");
        Console.WriteLine("Use 'q' key to exit...");
        Console.WriteLine();

        Console.WriteLine("Enter N: ");

        var input = Console.ReadLine();

        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        while (input.Trim().ToUpper() != "Q")
        {
            if (int.TryParse(input, out var n))
            {
                tokenSource.Cancel();
                tokenSource = new CancellationTokenSource();
                token = tokenSource.Token;
                CalculateSum(n, token);
            }
            else
            {
                Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                Console.WriteLine("Enter N: ");
            }

            input = Console.ReadLine();

        }
        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private static async Task CalculateSum(int n, CancellationToken cancellationToken)
    {

        long result = 0; 
        Task sum = Task.Run(() => 
        {
            result = Calculator.Calculate(n,cancellationToken);
        }
        );
        try
        {
            await sum;
        }
        catch (OperationCanceledException ex) { return; }

        Console.WriteLine($"Sum for {n} = {result}.");
        Console.WriteLine();
        Console.WriteLine("Enter N: ");
        Console.WriteLine($"The task for {n} started... Enter N to cancel the request:");
    }
}
