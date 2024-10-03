using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using LogParser_1.UI;
using System.Globalization;

namespace LogParser_1
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            MenuManager.Menu();




            /*

            string filePath = @"C:\Users\karsi\source\repos\LogParser_1\LogParser_1\DATA\20220601182758.csv";//read from console




            var records = ParseLogCsv(filePath);

            foreach (var log in records)
            {
                Console.WriteLine("Log Entry:");
                foreach (var kvp in log)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
                Console.WriteLine();
            }
            */
            //Console.WriteLine(records.Count);
        }






    }
}
