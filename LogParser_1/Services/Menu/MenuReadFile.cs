using LogParser_1.OPTIONS;
using LogParser_1.Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogParser_1.Services.Menu
{
    internal class MenuReadFile : MenuElements
    {
        public override void Action(List<Dictionary<string, object>> record, out string statusString)//async ??
        {
            Console.Clear();
            statusString = "";
            string filePath = "", fileName = "", fileDirectory = "";
            ConsoleKeyInfo consoleKey;
            do 
            {
                PrintMenu(statusString);
                consoleKey = Console.ReadKey();
                switch (consoleKey.Key)
                {
                    case ConsoleKey.F1 :
                        Console.Clear();
                        filePath = GetGoodFilePath();
                        record.AddRange(FileManager.ParseLogCsv(filePath));//multi files
                        statusString = $"File succesfully parsed : {record.Count} current data rows in memory";
                        break;
                    case ConsoleKey.F2 :
                        if (!record.Any())                        
                            break;                        
                        Console.Clear();
                        string newFilePath = Options.outputJsonFilesPath + "FullParsedData.json";
                        FileManager.StreamWriter(record, newFilePath);///await reikia
                        statusString = "Json file created : " + newFilePath;
                        break;                               
                    case ConsoleKey.F3 :
                        if (!record.Any())
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
            Console.WriteLine("'F1' - Read And Parse Files");
            Console.WriteLine();
            Console.WriteLine("'F2' - Write To Json File");
            Console.WriteLine("'F3' - Write To DataBase");

            Console.WriteLine("\r\n'ESC' - To exit\r\n");
            if (status != "")
                Console.WriteLine($"---+ {status} +---\r\n");
        }
        private static string GetGoodFilePath()
        {
            string filePath = "";
            Console.Clear();
            do
            {
                Console.WriteLine("Please enter path to .CSV file");
                filePath = Console.ReadLine();
                if (filePath == "" || !File.Exists(filePath) || Path.GetExtension(filePath) != Options.availableFileExtension)
                    Console.WriteLine("File path is incorect");
                else
                    break;
            }
            while (true);
            return filePath;
        }


    }
}
