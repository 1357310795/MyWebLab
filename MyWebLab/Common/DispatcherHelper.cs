using System;
using System.Security.Permissions;
using System.Windows.Threading;

namespace USTCORi.WebLabClient.Common
{
    // Token: 0x02000018 RID: 24
    public static class DispatcherHelper
    {
        // Token: 0x060000D0 RID: 208 RVA: 0x0000865C File Offset: 0x0000685C
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(DispatcherHelper.ExitFrames), frame);
            try
            {
                Dispatcher.PushFrame(frame);
            }
            catch (InvalidOperationException)
            {
            }
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x000086AC File Offset: 0x000068AC
        private static object ExitFrames(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
}
