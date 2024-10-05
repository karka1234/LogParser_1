using LogParser_1.OPTIONS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogParser_1.Services.Menu
{
    internal class MenuManipulateData : MenuElements
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
                        string jsonString = FileManager.ObjToJsonString(filteredResultRecords);
                        Console.WriteLine(jsonString);
                        Console.WriteLine("Press Any Key To Continue");
                        Console.ReadKey(true);
                    break;
                    case ConsoleKey.F3:
                        if (!filteredResultRecords.Any())
                            break;
                        Console.Clear();
                        string newFilePath = Options.outputJsonFilesPath + "FilteredParsedData.json";
                        FileManager.StreamWriter(filteredResultRecords, newFilePath);///await reikia
                        statusString = "Json file created : " + newFilePath;
                        break;
                    case ConsoleKey.F4:
                        if (!filteredResultRecords.Any())
                            break;
                        Console.Clear();
                        DataBaseManager.CreateAndInsertLogsIntoDatabase(record);
                        statusString = "Database updated";
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
            Console.WriteLine("'F2' - Print to console");
            Console.WriteLine("'F3' - Write To Json File");
            Console.WriteLine("'F4' - Write To DataBase");

            Console.WriteLine("\r\n'ESC' - To exit\r\n");
            if (status != "")
                Console.WriteLine($"---+ {status} +---\r\n");
        }


    }
}
