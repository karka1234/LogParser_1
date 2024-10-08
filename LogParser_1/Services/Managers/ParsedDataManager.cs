﻿using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Query;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json.Linq;

namespace LogParser_1.Services.Managers
{
    internal class ParsedDataManager
    {
        public static List<Dictionary<string, object>> GetAndFilterRecords(List<Dictionary<string, object>> records)
        {
            Console.WriteLine("\r\nEnter your query (format: column_name = 'search_string' or select FROM column_name WHERE text='search_string'):");
            string query = Console.ReadLine();
            if (string.IsNullOrEmpty(query))
            {
                Console.WriteLine("Empty");
                return new List<Dictionary<string, object>>();
            }

            var pattern = @"(?i)(\w+)\s*=\s*'([^']*)'|select\s+FROM\s+(\w+)\s+WHERE\s+text\s*=\s*'([^']*)'";

            var parts = Regex.Split(query, @"\s+(AND|OR)\s+", RegexOptions.IgnoreCase).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>(records);

            bool useAnd = true;
            for (int i = 0; i < parts.Count; i++)
            {
                var part = parts[i].Trim();

                useAnd = DetermineLogicOperatorInQuerry(useAnd, part);

                var match = Regex.Match(part, pattern);
                if (!match.Success)
                {
                    Console.WriteLine($"Invalid condition in query: '{part}'.");
                    return new List<Dictionary<string, object>>();//return empty
                }

                string columnName = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[3].Value;
                string searchString = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[4].Value;

                if (!records.Any() || !records[0].ContainsKey(columnName))
                {
                    Console.WriteLine($"Column '{columnName}' not found.");
                    return new List<Dictionary<string, object>>();//return empty
                }

                results = GetResultQeerry(records, results, useAnd, columnName, searchString);
            }
            return results;
        }


        private static List<Dictionary<string, object>> GetResultQeerry(List<Dictionary<string, object>> records, List<Dictionary<string, object>> results, bool useAnd, string columnName, string searchString)
        {
            if (useAnd)
            {
                results = results
                                .Where(r => r[columnName]?.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList();
            }
            else
            {
                var orResults = records
                                    .Where(r => r[columnName]?.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList();
                results = results
                                .Union(orResults).ToList();
            }

            return results;
        }

        private static bool DetermineLogicOperatorInQuerry(bool currentUseAnd, string part)
        {
            if (part.Equals("AND", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (part.Equals("OR", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return currentUseAnd;
        }

        public static Dictionary<string, object> GetTotalRecordsCounter(List<Dictionary<string, object>> records)
        {
            Dictionary<string, object> additionalData = new Dictionary<string, object>();
            additionalData.Add("Counter", records.Count);
            return additionalData;
        }

        public static void PrintToConsoleListKeys(string header, List<Dictionary<string, object>> records)
        {
            List<string> data = new List<string>();
            Console.WriteLine(header);
            data = records[0].Keys
                                .Select(r => r + "; ").ToList();
            data.ForEach(d => { Console.Write(d); });
            Console.WriteLine();
        }

        public static Dictionary<object, int> GetStatiscticsBasedOnCol(List<Dictionary<string, object>> records)
        {
            Console.WriteLine("\r\nEnter collumn name : ");
            string columnName = Console.ReadLine();
            if (string.IsNullOrEmpty(columnName))
            {
                Console.WriteLine("Empty");
                return new Dictionary<object, int>();
            }

            if (!records.Any() || !records[0].ContainsKey(columnName))
            {
                Console.WriteLine($"Column '{columnName}' not found.");
                return new Dictionary<object, int>();//return empty
            }

            Dictionary<object, int> statisticResult = records
                                                            .GroupBy(r => r[columnName])
                                                            .Select(g => new
                                                            {
                                                                Value = g.Key,
                                                                Count = g.Count()
                                                            })
                                                            .OrderBy(x => x.Value)
                                                            .ToDictionary(x => x.Value, x => x.Count);
            return statisticResult;
        }

        public static void PrintStatiscticsToConsole(Dictionary<object, int> statisticResult)
        {            
            Console.WriteLine($"\r\n{"CollName",-50} | {"Count",-20}");
            foreach (var item in statisticResult)
            {
                Console.WriteLine($"{item.Key,-50}  {item.Value,-20}");
            }
        }


    }
}
