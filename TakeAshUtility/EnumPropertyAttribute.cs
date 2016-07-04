using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TakeAshUtility {

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class EnumPropertyAttribute :
        Attribute {

        static Regex regKeyValuePair = new Regex(@"(?<key>[A-Za-z_][A-Za-z0-9_]*)\s*:\s*(?<quote>['""])(?<value>[^'""]*)\k<quote>");
        static Regex regHex = new Regex(@"\\[uU](?<uni>[0-9A-Fa-f]{4})|\\[xX](?<hex>[0-9A-Fa-f]{2}([0-9A-Fa-f]{2})?)");

        private Dictionary<string, string> _enumProperties;

        public EnumPropertyAttribute(string keyValuePairs) {
            _enumProperties = regKeyValuePair.Matches(keyValuePairs)
                .OfType<Match>()
                .ToDictionary(
                    m => m.Groups["key"].Value,
                    m => DecodeHex(m.Groups["value"].Value)
                );
        }

        public string this[string key] {
            get {
                return _enumProperties.ContainsKey(key) ?
                    _enumProperties[key] :
                    null;
            }
            set { _enumProperties[key] = value; }
        }

        public bool ContainsKey(string key) {
            return _enumProperties.ContainsKey(key);
        }

        private string DecodeHex(string value) {
            if (String.IsNullOrEmpty(value)) {
                return value;
            }
            return regHex.Replace(
                value,
                new MatchEvaluator(
                    hex => !String.IsNullOrEmpty(hex.Groups["uni"].Value) ?
                        ((char)Convert.ToInt32(hex.Groups["uni"].Value, 16)).ToString() :
                        ((char)Convert.ToInt32(hex.Groups["hex"].Value, 16)).ToString()
                )
            );
        }
    }
}
