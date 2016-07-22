using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
            if (ptr == IntPtr.Zero) {
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
            if (ptr == IntPtr.Zero) {
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
            if (ptr == IntPtr.Zero) {
                return default(T);
            }
            var obj = Marshal.PtrToStructure(ptr, typeof(T));
            return obj is T ?
                (T)obj :
                default(T);
        }
    }
}
