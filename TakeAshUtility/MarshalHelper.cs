using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TakeAshUtility.Native;

namespace TakeAshUtility {

    public static class MarshalHelper {

        public static GCHandle ToGCHandle(this object obj) {
            return GCHandle.Alloc(obj);
        }

        public static IntPtr ToIntPtr(this GCHandle handle) {
            return GCHandle.ToIntPtr(handle);
        }

        /// <summary>
        /// Get the managed object from an unmanaged block of memory through GCHandle.
        /// </summary>
        /// <typeparam name="T">The type of the managed object.</typeparam>
        /// <param name="ptr">The pointer to the object.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>The managed object, if conversion succeed.</item>
        /// <item>The defaut value of the T type, if conversion fail.</item>
        /// </list>
        /// </returns>
        public static T To<T>(this IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                return default(T);
            }
            var handle = GCHandle.FromIntPtr(ptr);
            return handle.Target is T ?
                (T)handle.Target :
                default(T);
        }

        /// <summary>
        /// Set the managed structure to an unmanaged block of memory.
        /// </summary>
        /// <typeparam name="T">The type of the managed structure.</typeparam>
        /// <param name="ptr">A pointer to an unmanaged block of memory, which must be allocated before this method is called.</param>
        /// <param name="structure">A managed object that holds the data to be marshaled. This object must be a structure or an instance of a formatted class.</param>
        /// <param name="fDeleteOld">
        /// true to call the Marshal.DestroyStructure method on the ptr parameter before this method copies the data.
        /// The block must contain valid data.
        /// Note that passing false when the memory block already contains data can lead to a memory leak.
        /// </param>
        public static void Write<T>(this IntPtr ptr, T structure, bool fDeleteOld) {
            if(ptr == IntPtr.Zero) {
                return;
            }
            Marshal.StructureToPtr(structure, ptr, fDeleteOld);
        }

        /// <summary>
        /// Get the managed structure from an unmanaged block of memory.
        /// </summary>
        /// <typeparam name="T">The type of object to be created. This object must represent a formatted class or a structure.</typeparam>
        /// <param name="ptr">A pointer to an unmanaged block of memory.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>The managed structure, if conversion succeed.</item>
        /// <item>The defaut value of the T type, if conversion fail.</item>
        /// </list>
        /// </returns>
        public static T Read<T>(this IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                return default(T);
            }
            var obj = Marshal.PtrToStructure(ptr, typeof(T));
            return obj is T ?
                (T)obj :
                default(T);
        }

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">A handle to the DLL module that contains the function or variable.</param>
        /// <param name="procName">The function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns>
        /// If the function succeeds, the return value is the address of the exported function or variable.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        public static IntPtr ToProcAddress(this IntPtr hModule, string procName) {
            return hModule == IntPtr.Zero || String.IsNullOrEmpty(procName) ?
                IntPtr.Zero :
                NativeMethods.GetProcAddress(hModule, procName);
        }

        /// <summary>
        /// Converts an unmanaged function pointer to a delegate.
        /// </summary>
        /// <param name="hMethod">The unmanaged function pointer to be converted.</param>
        /// <param name="delegateType">The type of the delegate to be returned.</param>
        /// <returns>A delegate instance that can be cast to the appropriate delegate type.</returns>
        public static Delegate ToDelegate(this IntPtr hMethod, Type delegateType) {
            return hMethod == IntPtr.Zero || delegateType == null ?
                null :
                Marshal.GetDelegateForFunctionPointer(hMethod, delegateType);
        }

        /// <summary>
        /// Copies all characters up to the first null character from an unmanaged ANSI string to a managed String, 
        /// and widens each ANSI character to Unicode.
        /// </summary>
        /// <param name="ptr">The address of the first character of the unmanaged string.</param>
        /// <returns>A managed string that holds a copy of the unmanaged ANSI string. 
        /// If ptr is null, the method returns a null.</returns>
        public static string ToStringAnsi(this IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                return null;
            }
            return Marshal.PtrToStringAnsi(ptr);
        }

        /// <summary>
        /// Allocates a managed String, copies a specified number of characters from an unmanaged ANSI string into it, 
        /// and widens each ANSI character to Unicode.
        /// </summary>
        /// <param name="ptr">The address of the first character of the unmanaged string.</param>
        /// <param name="len">The byte count of the input string to copy.</param>
        /// <returns>A managed string that holds a copy of the native ANSI string if the value of the ptr parameter is not null; 
        /// otherwise, this method returns null.</returns>
        public static string ToStringAnsi(this IntPtr ptr, int len) {
            if(ptr == IntPtr.Zero) {
                return null;
            }
            return Marshal.PtrToStringAnsi(ptr, len);
        }

        /// <summary>
        /// Allocates a managed String and copies all characters up to the first null character from a string stored in unmanaged memory into it.
        /// </summary>
        /// <param name="ptr">For Unicode platforms, the address of the first Unicode character. 
        /// Or for ANSI plaforms, the address of the first ANSI character.</param>
        /// <returns>A managed string that holds a copy of the unmanaged string if the value of the ptr parameter is not null; 
        /// otherwise, this method returns null.</returns>
        public static string ToStringAuto(this IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                return null;
            }
            return Marshal.PtrToStringAuto(ptr);
        }

        /// <summary>
        /// Allocates a managed String and copies the specified number of characters from a string stored in unmanaged memory into it.
        /// </summary>
        /// <param name="ptr">For Unicode platforms, the address of the first Unicode character. 
        /// Or for ANSI plaforms, the address of the first ANSI character.</param>
        /// <param name="len">The number of characters to copy.</param>
        /// <returns>A managed string that holds a copy of the native string if the value of the ptr parameter is not null; 
        /// otherwise, this method returns null.</returns>
        public static string ToStringAuto(this IntPtr ptr, int len) {
            if(ptr == IntPtr.Zero) {
                return null;
            }
            return Marshal.PtrToStringAuto(ptr, len);
        }

        /// <summary>
        /// Allocates a managed String and copies a binary string (BSTR) stored in unmanaged memory into it.
        /// </summary>
        /// <param name="ptr">The address of the first character of the unmanaged string.</param>
        /// <returns>A managed string that holds a copy of the unmanaged string.</returns>
        public static string ToStringBSTR(this IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                return null;
            }
            return Marshal.PtrToStringBSTR(ptr);
        }

        /// <summary>
        /// Allocates a managed String and copies all characters up to the first null character from an unmanaged Unicode string into it.
        /// </summary>
        /// <param name="ptr">The address of the first character of the unmanaged string.</param>
        /// <returns>A managed string that holds a copy of the unmanaged string if the value of the ptr parameter is not null; 
        /// otherwise, this method returns null.</returns>
        public static string ToStringUni(this IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                return null;
            }
            return Marshal.PtrToStringUni(ptr);
        }

        /// <summary>
        /// Allocates a managed String and copies a specified number of characters from an unmanaged Unicode string into it.
        /// </summary>
        /// <param name="ptr">The address of the first character of the unmanaged string.</param>
        /// <param name="len">The number of Unicode characters to copy.</param>
        /// <returns>A managed string that holds a copy of the unmanaged string if the value of the ptr parameter is not null; 
        /// otherwise, this method returns null.</returns>
        public static string ToStringUni(this IntPtr ptr, int len) {
            if(ptr == IntPtr.Zero) {
                return null;
            }
            return Marshal.PtrToStringUni(ptr, len);
        }
    }
}
