using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using TakeAshUtility.Native;

namespace TakeAshUtility {

    /// <summary>
    /// Unmanaged dll wrapper class
    /// </summary>
    /// <remarks>
    /// - [C#|アンマネージドDLLを動的にロードする | 貧脚レーサーのサボり日記](http://anis774.net/codevault/loadlibrary.html)
    /// - [C#でLoadLibraryを使用してアンマネージDLLを使用する - wata_d's diary](http://hwada.hatenablog.com/entry/20070919/1190166134)
    /// - [C# GetDelegateForFunctionPointer with generic delegate - Stack Overflow](https://stackoverflow.com/questions/26699394/)
    /// - [c# - Cannot cast delegate to a generic type T - Stack Overflow](https://stackoverflow.com/questions/35321586/)
    /// </remarks>
    public class UnManagedDll :
        IDisposable {

        public UnManagedDll(string fileName) {
            this.InitializeDisposeFinalizePattern();
            ModuleHandle = NativeMethods.LoadLibrary(fileName);
        }

        public IntPtr ModuleHandle { get; private set; }

        public bool Loaded {
            get { return ModuleHandle != IntPtr.Zero; }
        }

        public T GetProcDelegate<T>(string method)
            where T : class { // T : Delegate
            this.ThrowExceptionIfDisposed();
            var methodHandle = ModuleHandle.ToProcAddress(method);
            return Marshal.GetDelegateForFunctionPointer(methodHandle, typeof(T)) as T;
        }

        public Func<TResult> GetFunc<TResult>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(TResult));
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return () => (TResult)nonGenericDelegate.DynamicInvoke(null);
        }

        public Func<T, TResult> GetFunc<T, TResult>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(TResult), new[] { typeof(T) });
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return (T x) => (TResult)nonGenericDelegate.DynamicInvoke(new object[] { x });
        }

        public Func<T1, T2, TResult> GetFunc<T1, T2, TResult>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(TResult), new[] { typeof(T1), typeof(T2) });
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return (T1 x1, T2 x2) => (TResult)nonGenericDelegate.DynamicInvoke(new object[] { x1, x2 });
        }

        public Func<T1, T2, T3, TResult> GetFunc<T1, T2, T3, TResult>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(TResult), new[] { typeof(T1), typeof(T2), typeof(T3) });
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return (T1 x1, T2 x2, T3 x3) => (TResult)nonGenericDelegate.DynamicInvoke(new object[] { x1, x2, x3 });
        }

        public Func<T1, T2, T3, T4, TResult> GetFunc<T1, T2, T3, T4, TResult>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(TResult), new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return (T1 x1, T2 x2, T3 x3, T4 x4) => (TResult)nonGenericDelegate.DynamicInvoke(new object[] { x1, x2, x3, x4 });
        }

        public Action GetAction(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(void));
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return () => nonGenericDelegate.DynamicInvoke();
        }

        public Action<T> GetAction<T>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(void), new[] { typeof(T) });
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return (T x) => nonGenericDelegate.DynamicInvoke(new object[] { x });
        }

        public Action<T1, T2> GetAction<T1, T2>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(void), new[] { typeof(T1), typeof(T2) });
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return (T1 x1, T2 x2) => nonGenericDelegate.DynamicInvoke(new object[] { x1, x2 });
        }

        public Action<T1, T2, T3> GetAction<T1, T2, T3>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(void), new[] { typeof(T1), typeof(T2), typeof(T3) });
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return (T1 x1, T2 x2, T3 x3) => nonGenericDelegate.DynamicInvoke(new object[] { x1, x2, x3 });
        }

        public Action<T1, T2, T3, T4> GetAction<T1, T2, T3, T4>(string method) {
            this.ThrowExceptionIfDisposed();
            var delegateType = DelegateHelper.NewDelegateType(typeof(void), new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
            var nonGenericDelegate = ModuleHandle.ToProcAddress(method).ToDelegate(delegateType);
            if(nonGenericDelegate == null) { return null; }
            return (T1 x1, T2 x2, T3 x3, T4 x4) => nonGenericDelegate.DynamicInvoke(new object[] { x1, x2, x3, x4 });
        }

        #region Dispose Finalize パターン

        /// <summary>
        /// 既にDisposeメソッドが呼び出されているかどうかを表します。
        /// </summary>
        private bool disposed;

        /// <summary>
        /// TakeAshUtility.UnManagedDll によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose() {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        /// <summary>
        /// TakeAshUtility.UnManagedDll クラスのインスタンスがGCに回収される時に呼び出されます。
        /// </summary>
        ~UnManagedDll() {
            this.Dispose(false);
        }

        /// <summary>
        /// TakeAshUtility.UnManagedDll によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。 </param>
        protected virtual void Dispose(bool disposing) {
            if(this.disposed) {
                return;
            }
            this.disposed = true;

            if(disposing) {
                // マネージ リソースの解放処理をこの位置に記述します。
            }
            // アンマネージ リソースの解放処理をこの位置に記述します。
            NativeMethods.FreeLibrary(ModuleHandle);
            ModuleHandle = IntPtr.Zero;
        }

        /// <summary>
        /// 既にDisposeメソッドが呼び出されている場合、例外をスローします。
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">既にDisposeメソッドが呼び出されています。</exception>
        protected void ThrowExceptionIfDisposed() {
            if(this.disposed) {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        /// <summary>
        /// Dispose Finalize パターンに必要な初期化処理を行います。
        /// </summary>
        private void InitializeDisposeFinalizePattern() {
            this.disposed = false;
        }

        #endregion
    }
}
