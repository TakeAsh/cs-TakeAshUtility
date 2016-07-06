using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    [AttributeUsage(AttributeTargets.Enum)]
    public class HexDigitAttribute :
        Attribute {

        public HexDigitAttribute(int digit) {
            Digit = digit;
            Format = "X" + digit;
        }

        public int Digit { get; private set; }
        public string Format { get; private set; }
    }
}
