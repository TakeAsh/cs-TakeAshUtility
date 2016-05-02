using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class ConsoleEx {

        /// <summary>
        /// Non blocking ReadKey
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item>default(ConsoleKey), if no key is pressed.</item>
        /// <item>ConsoleKey, if some key is pressed.</item>
        /// </list>
        /// </returns>
        public static ConsoleKey ReadKey() {
            return !Console.KeyAvailable ?
                default(ConsoleKey) :
                Console.ReadKey(true).Key;
        }
    }
}
