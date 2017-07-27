using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TakeAshUtility;

namespace TakeAshUtility_Sample {

    /// <summary>
    /// Locale Dependent Mapping Flags.
    /// </summary>
    /// <remarks>
    /// - WinNls.h
    /// </remarks>
    [Flags]
    public enum LCMAP : uint {
        /// <summary>
        /// lower case letters
        /// </summary>
        LOWERCASE = 0x00000100,

        /// <summary>
        /// UPPER CASE LETTERS
        /// </summary>
        UPPERCASE = 0x00000200,

        /// <summary>
        /// Title Case Letters
        /// </summary>
        TITLECASE = 0x00000300,

        /// <summary>
        /// WC sort key (normalize)
        /// </summary>
        SORTKEY = 0x00000400,

        /// <summary>
        /// byte reversal
        /// </summary>
        BYTEREV = 0x00000800,

        /// <summary>
        /// map katakana to hiragana
        /// </summary>
        HIRAGANA = 0x00100000,

        /// <summary>
        /// map hiragana to katakana
        /// </summary>
        KATAKANA = 0x00200000,

        /// <summary>
        /// map double byte to single byte
        /// </summary>
        HALFWIDTH = 0x00400000,

        /// <summary>
        /// map single byte to double byte
        /// </summary>
        FULLWIDTH = 0x00800000,

        /// <summary>
        /// use linguistic rules for casing
        /// </summary>
        LINGUISTIC_CASING = 0x01000000,

        /// <summary>
        /// map traditional chinese to simplified chinese
        /// </summary>
        SIMPLIFIED_CHINESE = 0x02000000,

        /// <summary>
        /// map simplified chinese to traditional chinese
        /// </summary>
        TRADITIONAL_CHINESE = 0x04000000,
    }

    public static class LCMapString {

        private static readonly UnManagedDll _kernel32 = new UnManagedDll("kernel32");
        private static readonly Func<IntPtr, uint, IntPtr, int, IntPtr, int, IntPtr, IntPtr, IntPtr, int> _lcMapStringEx = _kernel32.GetFunc<IntPtr, uint, IntPtr, int, IntPtr, int, IntPtr, IntPtr, IntPtr, int>("LCMapStringEx");

        public static string Convert_UnManagedDll(LCMAP mapFlags, string src) {
            var srcPtr = Marshal.StringToHGlobalUni(src);
            var len = _lcMapStringEx.SafeInvoke(IntPtr.Zero, (uint)mapFlags, srcPtr, -1, IntPtr.Zero, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            var dstSize = len * 2;
            var dstPtr = Marshal.AllocHGlobal(dstSize);
            _lcMapStringEx.SafeInvoke(IntPtr.Zero, (uint)mapFlags, srcPtr, -1, dstPtr, dstSize, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            var dst = dstPtr.ToStringUni();
            Marshal.FreeHGlobal(srcPtr);
            Marshal.FreeHGlobal(dstPtr);
            return dst;
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int LCMapStringEx(IntPtr lpLocaleName, uint dwMapFlags, string lpSrcStr, int cchSrc, StringBuilder lpDestStr, int cchDest, IntPtr lpVersionInformation, IntPtr lpReserved, IntPtr sortHandle);

        public static string Convert_DllImport(LCMAP mapFlags, string src) {
            var len = LCMapStringEx(IntPtr.Zero, (uint)mapFlags, src, -1, null, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            var dst = new StringBuilder(len);
            LCMapStringEx(IntPtr.Zero, (uint)mapFlags, src, -1, dst, dst.Capacity, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            return dst.ToString();
        }

        public static string ToKATAKANA1(this string src) {
            return Convert_UnManagedDll(LCMAP.KATAKANA, src);
        }

        public static string ToHIRAGANA1(this string src) {
            return Convert_UnManagedDll(LCMAP.HIRAGANA, src);
        }
    }
}
