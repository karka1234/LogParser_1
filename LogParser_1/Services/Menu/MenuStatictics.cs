using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogParser_1.Services.Managers;

namespace LogParser_1.Services.Menu
{
    internal class MenuStatictics : MenuElements
    {
        public override void Action(List<Dictionary<string, object>> record, out string statusString)
        {
            statusString = "";
            List<Dictionary<string, object>> filteredResultRecords = new List<Dictionary<string, object>>(record);
            ConsoleKeyInfo consoleKey;
            do
            {
                statusString = "\r\nTotal amount of filtered results : " + filteredResultRecords.Count;
                PrintMenu(statusString);
                consoleKey = Console.ReadKey();
                switch (consoleKey.Key)
                {
                    case ConsoleKey.F1:
                        Console.Clear();
                        Console.WriteLine("'ESC' - To exit\r\n");
                        ParsedDataManager.PrintToConsoleListKeys("Existing collumns", record);
                        filteredResultRecords = ParsedDataManager.GetAndFilterRecords(record);
                        if (!filteredResultRecords.Any())
                            break;
                        break;
                    case ConsoleKey.F2:
                        Console.Clear();
                        Console.WriteLine("'ESC' - To exit\r\n");
                        ParsedDataManager.PrintToConsoleListKeys("Existing collumns", record);
                        ParsedDataManager.PrintStatiscticsToConsole(ParsedDataManager.GetStatiscticsBasedOnCol(filteredResultRecords));
                        Console.WriteLine("Press Any Key To Continue");
                        Console.ReadKey(true);
                        break;
                    default:
                        break;
                }
            }
            while (consoleKey.Key != ConsoleKey.Escape);
        }

        public override void PrintMenu(string status)
        {
            Console.Clear();
            Console.WriteLine("Menu > ");
            Console.WriteLine("\r\nPress your option\r\n");
            Console.WriteLine("'F1' - Filter records");
            Console.WriteLine();
            Console.WriteLine("'F2' - Statistics based on col");

            Console.WriteLine("\r\n'ESC' - To exit\r\n");
            if (status != "")
                Console.WriteLine($"---+ {status} +---\r\n");
        }

    }
}
