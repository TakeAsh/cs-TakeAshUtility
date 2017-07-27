using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeAshUtility;

namespace TakeAshUtility_Sample {

    class UnManagedDll_Sample {

        public static void Exec() {

            var user32 = new UnManagedDll("user32");
            var messageBoxA = user32.GetFunc<IntPtr, string, string, uint, int>("MessageBoxA");
            var ok = messageBoxA.SafeInvoke(IntPtr.Zero, "こんにちは世界！", "キャプション", (uint)0);

            var src = "あいうえおカキクケコ";
            var dst1 = src.ToKATAKANA1();
            var dst2 = src.ToHIRAGANA1();
            Console.WriteLine("src:\t" + src);
            Console.WriteLine("dst1:\t" + dst1);
            Console.WriteLine("dst2:\t" + dst2);
        }
    }
}
