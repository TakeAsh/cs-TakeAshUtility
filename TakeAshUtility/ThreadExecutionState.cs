using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TakeAshUtility {

    public static class ThreadExecutionState {

        /// <summary>
        /// Inform the system that it is in use.
        /// </summary>
        /// <param name="flags">The thread's execution requirements.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>If the function succeeds, the return value is the previous thread execution state.</item>
        /// <item>If the function fails, the return value is NULL.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// [SetThreadExecutionState function (Windows)](https://msdn.microsoft.com/en-us/library/windows/desktop/aa373208.aspx)
        /// </remarks>
        public static EXECUTION_STATE Set(EXECUTION_STATE flags) {
            return SetThreadExecutionState(flags);
        }

        /// <summary>
        /// Enables an application to inform the system that it is in use, 
        /// thereby preventing the system from entering sleep or turning off the display while the application is running.
        /// </summary>
        /// <param name="esFlags">The thread's execution requirements.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>If the function succeeds, the return value is the previous thread execution state.</item>
        /// <item>If the function fails, the return value is NULL.</item>
        /// </list>
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
    }

    [Flags]
    public enum EXECUTION_STATE : uint {

        /// <summary>
        /// Enables away mode. This value must be specified with ES_CONTINUOUS.
        /// Away mode should be used only by media-recording and media-distribution applications
        /// that must perform critical background processing on desktop computers while the computer appears to be sleeping.
        /// </summary>
        ES_AWAYMODE_REQUIRED = 0x00000040,

        /// <summary>
        /// Informs the system that the state being set should remain in effect until the next call that uses ES_CONTINUOUS
        /// and one of the other state flags is cleared.
        /// </summary>
        ES_CONTINUOUS = 0x80000000,

        /// <summary>
        /// Forces the display to be on by resetting the display idle timer.
        /// </summary>
        ES_DISPLAY_REQUIRED = 0x00000002,

        /// <summary>
        /// Forces the system to be in the working state by resetting the system idle timer.
        /// </summary>
        ES_SYSTEM_REQUIRED = 0x00000001,

        /// <summary>
        /// This value is not supported.
        /// If ES_USER_PRESENT is combined with other esFlags values,
        /// the call will fail and none of the specified states will be set.
        /// </summary>
        ES_USER_PRESENT = 0x00000004,
    }
}
