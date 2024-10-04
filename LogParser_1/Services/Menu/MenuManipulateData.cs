using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogParser_1.Services.Menu
{
    internal class MenuManipulateData : MenuActions
    {
        public override void Action(List<Dictionary<string, object>> record, out string statusString)///////prideti galimybe saugoti irasusi i db ir i json
        {
            do
            {
                
                Console.WriteLine("'ESC' - To exit\r\n");
                PrintToConsoleList("Existing collumns", GetHeaders(record));

                List<Dictionary<string, object>> result = ParsedDataManager.GetAndProcessQuery(record);

                if (result.Any())
                {
                    Console.WriteLine("\r\nTotal amount of results : " + result.Count);
                    Console.WriteLine("Do you really want to print ? 'ENTER' - To print ");
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        string jsonString = FileManager.ObjToJsonString(result);
                        Console.WriteLine(jsonString);
                    }
                }

                Console.WriteLine("\r\n\nAnyKey to Contunue\r\n");                
                Console.ReadKey(true);
            }
            while (Console.ReadKey().Key != ConsoleKey.Escape);
            statusString = "";
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
