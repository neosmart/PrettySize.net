using NeoSmart.PrettySize;
using System;
using System.Text;

namespace UserTests
{
    class Program
    {
        static bool aborted = false;

        static void Main(string[] args)
        {
            //Console.TreatControlCAsInput = true;
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                aborted = true;
                Environment.Exit(0);
            };

            while (!aborted)
            {
                //Console.TreatControlCAsInput = true;
                Console.Write("> ");

                var input = Console.ReadLine();

                if (input == null)
                {
                    //Ctrl+C
                    break;
                }
                if (!long.TryParse(input, out var value))
                {
                    Console.WriteLine("Unable to parse user input!");
                    continue;
                }

                Console.WriteLine(PrettySize.Format(value, UnitBase.Base2));
                Console.WriteLine(PrettySize.Format(value, UnitBase.Base10));
            }
        }
    }
}