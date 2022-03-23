using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Util
{
    public static class ObjectDumper
    {
        public static string DumpProperties(object obj, Func<PropertyInfo, bool> selector, string prefix = "")
        {
            var properties = new Dictionary<string, object>();
            var type = obj.GetType();
            foreach(var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!selector(property)) continue;

                object value = property.GetValue(obj);
                if (IsSimpleType(value))
                    properties.Add(prefix + property.Name, value);
                else
                    properties.Add(property.Name, $"\n{{\n{DumpProperties(value, p => true, "\t")}}}");
            }
            StringBuilder dump = new StringBuilder();
            int longestProperty = properties.Keys.Max(x => x.Length);
            foreach (var propertyName in properties.Keys)
                dump.AppendLine($"{$"{propertyName}:".PadRight(longestProperty + 1)} {properties[propertyName]}");
            return dump.ToString();
        }

        private static bool IsSimpleType(object obj)
        {
            return Double.TryParse(obj.ToString(), out _) ||
                   obj is string ||
                   obj is bool;
        }
    }
}
