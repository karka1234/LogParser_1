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

        public static List<Dictionary<string, object>> ProcessQuery(List<Dictionary<string, object>> records)
        {
            Console.WriteLine("Enter your query (format: column_name = 'search_string' or select FROM column_name WHERE text='search_string'):");
            string query = Console.ReadLine();

            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>(records);

            var pattern = @"(?i)(\w+)\s*=\s*'([^']*)'|select\s+FROM\s+(\w+)\s+WHERE\s+text\s*=\s*'([^']*)'";

            var parts = Regex.Split(query, @"\s+(AND|OR)\s+", RegexOptions.IgnoreCase).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            if (parts.Count == 0)
            {
                Console.WriteLine("Invalid query format.");
                return results;
            }

            bool useAnd = true;

            for (int i = 0; i < parts.Count; i++)
            {
                var part = parts[i].Trim();

                if (part.Equals("AND", StringComparison.OrdinalIgnoreCase))
                {
                    useAnd = true;
                    continue;
                }

                if (part.Equals("OR", StringComparison.OrdinalIgnoreCase))
                {
                    useAnd = false;
                    continue;
                }

                var match = Regex.Match(part, pattern);

                if (!match.Success)
                {
                    Console.WriteLine($"Invalid condition in query: '{part}'.");
                    return new List<Dictionary<string, object>>();
                }

                string columnName = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[3].Value;
                string searchString = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[4].Value;

                if (!records.Any() || !records[0].ContainsKey(columnName))
                {
                    Console.WriteLine($"Column '{columnName}' not found.");
                    return new List<Dictionary<string, object>>();
                }

                if (useAnd)
                {
                    results = results.Where(r => r[columnName]?.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList();
                }
                else
                {
                    var orResults = records.Where(r => r[columnName]?.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList();
                    results = results.Union(orResults).ToList();
                }
            }
            return results;
        }



    }
}
