using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace LogParser_1.Services.Managers
{
    internal class FileManager
    {
        public static List<Dictionary<string, object>> ParseLogCsv(string filePath)////jei ikeliam skirtingus faiilus reik ziuret kad tuos pacius collumn naudotu
        {
            var records = new List<Dictionary<string, object>>();
            try
            {
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
            }
            catch (Exception ex)
            {
                Exeptions.ExeptionsHandler(ex);
            }
            return records;
        }

        public static void StreamWriter(List<Dictionary<string, object>> records, string filePath)
        {
            records.Add(ParsedDataManager.GetTotalRecordsCounter(records));

            string jsonString = ObjToJsonString(records);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                Exeptions.ExeptionsHandler(ex);
            }
        }

        public static string ObjToJsonString(List<Dictionary<string, object>> records)
        {
            string result = "";
            try
            {
                result = JsonSerializer.Serialize(records, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (Exception ex)
            {
                Exeptions.ExeptionsHandler(ex);
            }
            return result;
        }
    }
}
