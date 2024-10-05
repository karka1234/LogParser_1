using LogParser_1.OPTIONS;
using LogParser_1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LogParser_1.Services.Menu
{
    internal static class Menu 
    {
        public static void Launch()
        {
            MenuReadFile menuReadFile = new MenuReadFile();
            MenuManipulateData menuManipulateData = new MenuManipulateData();

            List<Dictionary<string, object>> record = new List<Dictionary<string, object>>();
            string statusString = "";

            Console.Clear();
            ConsoleKeyInfo consoleKey;
            do
            {
                statusString = "Total amount of parsed results : " + record.Count;
                PrintChooseMenuWithStatus(statusString);
                consoleKey = Console.ReadKey();
                switch (consoleKey.Key)
                {
                    case ConsoleKey.F1:
                        Console.Clear();
                        menuReadFile.Action(record, out statusString);
                        break;
                    case ConsoleKey.F2:
                        Console.Clear();
                        if (!record.Any())
                            break;
                        menuManipulateData.Action(record, out statusString);
                        break;
                    default:
                        break;
                }
            }
            while (consoleKey.Key != ConsoleKey.Escape);
        }

        private static void PrintChooseMenuWithStatus(string status)
        {
            Console.Clear();
            Console.WriteLine("LOG PARSER");
            Console.WriteLine("\r\nPress your option\r\n");
            Console.WriteLine("'F1' - Select and parse .CSV file"); //MenuReadFile
            Console.WriteLine();
            Console.WriteLine("'F2' - Manipulate data");  //MenuManipulateData

            Console.WriteLine("\r\n'ESC' - To exit\r\n");
            if (status != "")
                Console.WriteLine($"---+ {status} +---\r\n");
        }


    }
}
