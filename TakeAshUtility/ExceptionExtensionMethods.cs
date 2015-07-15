using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class ExceptionExtensionMethods {

        public static string GetAllMessages(this Exception ex) {
            if (ex == null) {
                return null;
            }
            var messages = new List<string>();
            while (ex != null) {
                messages.Add(ex.Message);
                ex = ex.InnerException;
            }
            return String.Join("\n", messages);
        }
    }
}
