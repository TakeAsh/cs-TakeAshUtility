using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TakeAshUtility.Native {

    public static class NativeMethods {

        private const string _kernel32 = "kernel32";

        [DllImport(_kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport(_kernel32, SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">A handle to the DLL module that contains the function or variable.</param>
        /// <param name="lpProcName">The function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns>
        /// If the function succeeds, the return value is the address of the exported function or variable.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport(_kernel32, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = false)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        /// <summary>
        /// Retrieves a string from the specified section in an initialization file.
        /// </summary>
        /// <param name="lpAppName">The name of the section containing the key name. If this parameter is NULL, the GetPrivateProfileString function copies all section names in the file to the supplied buffer.</param>
        /// <param name="lpKeyName">The name of the key whose associated string is to be retrieved. If this parameter is NULL, all key names in the section specified by the lpAppName parameter are copied to the buffer specified by the lpReturnedString parameter.</param>
        /// <param name="lpDefault">A default string. If the lpKeyName key cannot be found in the initialization file, GetPrivateProfileString copies the default string to the lpReturnedString buffer. If this parameter is NULL, the default is an empty string, "".</param>
        /// <param name="lpReturnedString">A pointer to the buffer that receives the retrieved string.</param>
        /// <param name="nSize">The size of the buffer pointed to by the lpReturnedString parameter, in characters.</param>
        /// <param name="lpFileName">The name of the initialization file. If this parameter does not contain a full path to the file, the system searches for the file in the Windows directory.</param>
        /// <returns>The return value is the number of characters copied to the buffer, not including the terminating null character.</returns>
        [DllImport(_kernel32, SetLastError = true)]
        public static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName
        );
    }
}
