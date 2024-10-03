using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Query;

namespace LogParser_1.Services
{
    internal static class ProcessData
    {
        public static List<Dictionary<string, object>> ParseLogCsv(string filePath)
        {
            var records = new List<Dictionary<string, object>>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var rows = csv.GetRecords<dynamic>().ToList();
                foreach (var row in rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (var property in (IDictionary<string, object>)row)//key header, value is value
                    {
                        dict[property.Key] = property.Value;
                    }
                    records.Add(dict);
                }
            }
            return records;
        }



        public static List<Dictionary<string, object>> ProcessQuery(List<Dictionary<string, object>> records, string query)
        {
            var match = Regex.Match(query, @"(?i)select\s+FROM\s+(\w+)\s+WHERE\s+text\s*=\s*'([^']*)'|(\w+)\s*=\s*'([^']*)'");// (\w+) key  '([^']*)' value
            /*
            if (!match.Success)
            {
                return "Invalid query format.";
            }*/

            string columnName = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[3].Value;
            string searchString = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[4].Value;

           /* if (!records.Any() || !records[0].ContainsKey(columnName))
            {
                return $"Column '{columnName}' not found.";
            }*/

            List<Dictionary<string, object>> results = records
                .Where(r => r[columnName]?.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList();

            return results;
        }


        public static List<string> GetHeaders(List<Dictionary<string, object>> records)
        { 
            List<string> headers = new List<string>();
            headers = records[0].Keys.ToList();
            return headers;
        }

        public static void PrintToConsoleList(string header, List<string> data)
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
