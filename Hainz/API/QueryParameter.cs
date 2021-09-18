using System;
using System.Collections.Generic;
using System.Text;

namespace Hainz.API
{
    public class QueryParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public QueryParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
