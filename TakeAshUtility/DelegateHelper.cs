using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {

    public static class DelegateHelper {

        private static readonly MethodInfo miMakeNewCustomDelegate = typeof(Expression).Assembly
            .GetType("System.Linq.Expressions.Compiler.DelegateHelpers")
            .GetMethod("MakeNewCustomDelegate", BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly Func<Type[], Type> MakeNewCustomDelegate = (Func<Type[], Type>)Delegate.CreateDelegate(
            typeof(Func<Type[], Type>),
            miMakeNewCustomDelegate
        );

        /// <summary>
        /// Create non-generic delegate type
        /// </summary>
        /// <param name="returnType">The type of return value.</param>
        /// <param name="parameters">The types of parameters.</param>
        /// <returns>The type of non-generic delete.</returns>
        /// <remarks>
        /// - [C# GetDelegateForFunctionPointer with generic delegate - Stack Overflow](https://stackoverflow.com/questions/26699394/)
        /// </remarks>
        public static Type NewDelegateType(Type returnType, params Type[] parameters) {
            var offset = parameters.Length;
            Array.Resize(ref parameters, offset + 1);
            parameters[offset] = returnType;
            return MakeNewCustomDelegate(parameters);
        }

        public static TResult SafeInvoke<TResult>(this Func<TResult> fnc) {
            return fnc == null ?
                default(TResult) :
                fnc();
        }

        public static TResult SafeInvoke<T, TResult>(this Func<T, TResult> fnc, T x) {
            return fnc == null ?
                default(TResult) :
                fnc(x);
        }

        public static TResult SafeInvoke<T1, T2, TResult>(this Func<T1, T2, TResult> fnc, T1 x1, T2 x2) {
            return fnc == null ?
                default(TResult) :
                fnc(x1, x2);
        }

        public static TResult SafeInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> fnc, T1 x1, T2 x2, T3 x3) {
            return fnc == null ?
                default(TResult) :
                fnc(x1, x2, x3);
        }

        public static TResult SafeInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> fnc, T1 x1, T2 x2, T3 x3, T4 x4) {
            return fnc == null ?
                default(TResult) :
                fnc(x1, x2, x3, x4);
        }

        public static TResult SafeInvoke<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> fnc, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5) {
            return fnc == null ?
                default(TResult) :
                fnc(x1, x2, x3, x4, x5);
        }

        public static TResult SafeInvoke<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> fnc, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6) {
            return fnc == null ?
                default(TResult) :
                fnc(x1, x2, x3, x4, x5, x6);
        }

        public static TResult SafeInvoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> fnc, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6, T7 x7) {
            return fnc == null ?
                default(TResult) :
                fnc(x1, x2, x3, x4, x5, x6, x7);
        }

        public static TResult SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> fnc, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6, T7 x7, T8 x8) {
            return fnc == null ?
                default(TResult) :
                fnc(x1, x2, x3, x4, x5, x6, x7, x8);
        }

        public static void SafeInvoke(this Action act) {
            if(act == null) {
                return;
            }
            act();
        }

        public static void SafeInvoke<T>(this Action<T> act, T x) {
            if(act == null) {
                return;
            }
            act(x);
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> act, T1 x1, T2 x2) {
            if(act == null) {
                return;
            }
            act(x1, x2);
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> act, T1 x1, T2 x2, T3 x3) {
            if(act == null) {
                return;
            }
            act(x1, x2, x3);
        }

        public static void SafeInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> act, T1 x1, T2 x2, T3 x3, T4 x4) {
            if(act == null) {
                return;
            }
            act(x1, x2, x3, x4);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> act, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5) {
            if(act == null) {
                return;
            }
            act(x1, x2, x3, x4, x5);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> act, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6) {
            if(act == null) {
                return;
            }
            act(x1, x2, x3, x4, x5, x6);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> act, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6, T7 x7) {
            if(act == null) {
                return;
            }
            act(x1, x2, x3, x4, x5, x6, x7);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> act, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6, T7 x7, T8 x8) {
            if(act == null) {
                return;
            }
            act(x1, x2, x3, x4, x5, x6, x7, x8);
        }
    }
}
