using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.Core
{

    /// <summary>
    /// String value custom attribute for enumeraions
    /// </summary>
    public class StringCustomAttribute : System.Attribute
    {
        private readonly string _value;

        public StringCustomAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
}
