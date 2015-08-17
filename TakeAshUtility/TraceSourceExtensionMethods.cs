using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.Logging;

namespace TakeAshUtility {

    public static class TraceSourceExtensionMethods {

        const string DefaultDelimiter = "\t";

        public static void TraceEventWithTimeAndMethodName(
            this TraceSource source,
            TraceEventType eventType,
            string message,
            int id = 0
        ) {
            if (source == null) {
                return;
            }
            var listenr = source.Listeners
                .OfType<FileLogTraceListener>()
                .FirstOrDefault();
            var delimiter = listenr != null ?
                listenr.Delimiter :
                DefaultDelimiter;
            source.TraceEvent(
                eventType,
                id,
                String.Join(delimiter, new[] {
                    DateTime.Now.ToString("s"),
                    new StackFrame(1).GetMethod().Name,
                    message,
                })
            );
        }
    }
}
