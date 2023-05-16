using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Storage.Utils
{
    internal static class CsvUtils
    {
        private static readonly CsvConfiguration _config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", Encoding = Encoding.UTF8 };

        internal static string Serialize<T>(T[] objList)
        {
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            string content = string.Empty;

            List<string> headers = new List<string>();
            foreach (PropertyInfo propInfo in props)
            {
                headers.Add(propInfo.Name);
            }

            content += $"{String.Join(',', headers)}\r\n";

            foreach (T obj in objList)
            {
                List<string> fields = new List<string>();
                foreach (PropertyInfo propInfo in props)
                {
                    string? value = propInfo.GetValue(obj)?.ToString();

                    fields.Add(value ?? string.Empty);
                }

                content += $"{String.Join(',', fields)}\r\n";
            }

            return content;
        }

        internal static T[]? Deserialize<T>(string csvContent)
        {
            using (StringReader strReader = new StringReader(csvContent))
            using (CsvReader reader = new CsvReader(strReader, _config))
            {
                return reader.GetRecords<T>().ToArray();
            }
        }
    }
}
