using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser_1.Services.Menu
{
    internal class MenuStatictics : MenuElements
    {
        public override void Action(List<Dictionary<string, object>> record, out string statusString)///////prideti galimybe saugoti irasusi i db ir i json
        {
            statusString = "";
            List<Dictionary<string, object>> filteredResultRecords = new List<Dictionary<string, object>>();
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
                        ParsedDataManager.PrintToConsoleDictionary("Existing collumns", record);
                        filteredResultRecords = ParsedDataManager.GetAndFilterRecords(record);
                        if (!filteredResultRecords.Any())
                            break;
                        break;
                    case ConsoleKey.F2:
                        if (!filteredResultRecords.Any())
                            break;
                        Console.Clear();

                        break;
                    case ConsoleKey.F3:
                        if (!filteredResultRecords.Any())
                            break;
                        Console.Clear();

                        break;
                    case ConsoleKey.F4:
                        if (!filteredResultRecords.Any())
                            break;
                        Console.Clear();

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
            Console.WriteLine("'F2' - "); // sort by date   // ivesim ranka collumn kuris atsakingas uz data
            Console.WriteLine("'F3' - "); // severity statistic   // ivesim severity lauka ir gausim kiek yra tarkim su 5 su 4
            Console.WriteLine("'F4' - "); // messages out with date and severity   // ivesim ranka message lauka ir severity lauka ir gausim visus msg su severity rikiuotu nuo didziausio
            //kazka su dublikatais daryt ? kur tarkim msg laukai juos sutraukt bet irgi ivesim msg lauka


            Console.WriteLine("\r\n'ESC' - To exit\r\n");
            if (status != "")
                Console.WriteLine($"---+ {status} +---\r\n");
        }

    }
}
