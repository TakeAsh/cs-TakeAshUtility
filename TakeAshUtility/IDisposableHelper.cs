using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public static class IDisposableHelper {

        public static void SafeDispose(this IDisposable disposable) {
            if (disposable == null) {
                return;
            }
            disposable.Dispose();
        }

        public static void SafeDispose<T>(this IEnumerable<T> disposables, Action<T> predispose = null)
            where T : IDisposable {

            if (disposables.SafeCount() == 0) {
                return;
            }
            if (predispose == null) {
                disposables.Where(disposable => disposable != null)
                    .ForEach(disposable => disposable.Dispose());
            } else {
                disposables.Where(disposable => disposable != null)
                    .ForEach(disposable => {
                        predispose(disposable);
                        disposable.Dispose();
                    });
            }
        }
    }
}
