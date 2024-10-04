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

namespace LogParser_1.UI
{
    internal static class MenuManager
    {       
        public static void Menu()
        {
            List<Dictionary<string, object>> record = new List<Dictionary<string, object>>();
            string statusString = "";
            ConsoleKeyInfo readConsole;            
            Console.Clear();
            do
            {
                Console.Clear();
                PrintChooseMenuWithStatus(statusString);
                readConsole = Console.ReadKey();
                switch (readConsole.Key)
                {
                    case ConsoleKey.F1:////perkelt i klases viduje esancius dalykus kad funkcionalumas issiskaidytu
                        string filePath = @"C:\Users\karsi\source\repos\LogParser_1\LogParser_1\DATA\20220601182758.csv";// GetGoodFilePath();
                        record.AddRange(ProcessData.ParseLogCsv(filePath)); // multi files
                        statusString = $"File succesfully parsed : {record.Count} current data rows in memory";
                        break;
                    case ConsoleKey.F2:
                        if (!(record.Count > 0))
                        {
                            statusString = "No data to play with";
                            break;
                        }
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("'ESC' - To exit\r\n");                           

                            PrintToConsoleList("Existing collumns", GetHeaders(record));

                            List<Dictionary<string, object>> result = ProcessData.ProcessQuery(record);

                            Dictionary<string, object> additionalData = new Dictionary<string, object>();
                            additionalData.Add("Counter", result.Count);
                            result.Add(additionalData);                           

                            if (result.Count > 100)
                            {
                                Console.WriteLine("\r\nTotal amount of results : " + result.Count);
                                Console.WriteLine("Do you really want to print ? 'ENTER' - To print ");
                                if (Console.ReadKey().Key == ConsoleKey.Enter)
                                {
                                    var jsonResult = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
                                    Console.WriteLine(jsonResult);
                                }
                            }                           
                            
                            Console.WriteLine("\r\n\nAnyKey to Contunue\r\n");
                            Console.ReadKey(true);
                        }
                        while (readConsole.Key != ConsoleKey.Escape);
                        break;

                    default:
                        break;
                }
            }
            while (readConsole.Key != ConsoleKey.Escape);

        }


        private static void PrintChooseMenuWithStatus(string status)
        {
            Console.WriteLine("LOG PARSER");
            Console.WriteLine("\r\nPress your option\r\n");
            Console.WriteLine("'F1' - Select and parse .CSV file");
            Console.WriteLine("'F2' - Search in parsed data");
            Console.WriteLine("\r\n'ESC' - To exit\r\n");
            if(status != "")
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

        private static List<string> GetHeaders(List<Dictionary<string, object>> records)
        {
            List<string> headers = new List<string>();
            headers = records[0].Keys.ToList();
            return headers;
        }

        private static void PrintToConsoleList(string header, List<string> data)
        {
            Console.WriteLine(header);
            foreach (var item in data)
            {
                Console.Write(item + "; ");
            }
            Console.WriteLine();
        }

    }
}
