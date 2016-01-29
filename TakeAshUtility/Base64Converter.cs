using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TakeAshUtility {

    public static class Base64Converter {

        public static string ToBase64String<T>(this T obj)
            where T : struct {

            var size = Marshal.SizeOf(obj);
            var arr = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            try {
                Marshal.StructureToPtr(obj, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex) {
                Debug.Print(ex.GetAllMessages());
                return null;
            }
            finally {
                if (ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        public static T FromBase64String<T>(string text)
            where T : struct {

            if (String.IsNullOrEmpty(text)) {
                return default(T);
            }
            var size = Marshal.SizeOf(typeof(T));
            var arr = Convert.FromBase64String(text);
            var ptr = Marshal.AllocHGlobal(Math.Max(size, arr.Length));
            try {
                Marshal.Copy(arr, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            catch (Exception ex) {
                Debug.Print(ex.GetAllMessages());
                return default(T);
            }
            finally {
                if (ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }
    }
}
