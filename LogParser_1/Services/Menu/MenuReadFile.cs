using LogParser_1.OPTIONS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogParser_1.Services.Menu
{
    internal class MenuReadFile : MenuActions
    {
        public override void Action(List<Dictionary<string, object>> record, out string statusString)//async reiktu bet ar to reikia nzn
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
                        filePath = @"C:\Users\karsi\source\repos\LogParser_1\LogParser_1\DATA\20220601182758.csv";// GetGoodFilePath();
                        fileName = Path.GetFileNameWithoutExtension(filePath);
                        fileDirectory = Path.GetDirectoryName(filePath);

                        record.AddRange(FileManager.ParseLogCsv(filePath));
                        statusString = $"File succesfully parsed : {record.Count} current data rows in memory";

                        break;
                    case ConsoleKey.F2 :
                        Console.Clear();
                        string newFilePath = $"{fileDirectory}\\{fileName}_parsed.json";
                        FileManager.StreamWriter(record, newFilePath);///await reikia

                        statusString = newFilePath;
                        break;                               
                    case ConsoleKey.F3 :
                        Console.Clear();
                        DbManager.CreateAndInsertLogsIntoDatabase(record);
                        statusString = "db updated";
                        break;
                    default:
                        break;
                }
            }
            while (consoleKey.Key != ConsoleKey.Escape);

        }

        private static void PrintMenu(string status)
        {
            Console.Clear();
            Console.WriteLine("Menu > Select and parse .CSV file");
            Console.WriteLine("\r\nPress your option\r\n");
            Console.WriteLine("'F1' - Read And Parse Files");
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
