using System;
using Lib;

namespace Console.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            bool canContinue = false;
            do
            {
                try
                {
                    System.Console.WriteLine("Enter expression to calculate. e.g - 1 + 2 * 4 - 1");
                    string input = System.Console.ReadLine();

                    Calculator calculator = new Calculator(input);
                    var result = calculator.Calculate();

                    System.Console.WriteLine();
                    System.Console.WriteLine($"Your result is {result}");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                }
                finally
                {
                    System.Console.WriteLine("Do you want to continue Y/N ?");
                    var input = System.Console.ReadLine()?.ToString().ToLower();
                    canContinue = input == "y";
                }

            }
            while (canContinue);
        }
    }
}
