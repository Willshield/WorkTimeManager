using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace WorkTimeManager.Services
{
    public class PopupService
    {
        public PopupService() { }

        public static readonly string YES = "Yes";
        public static readonly string NO = "No";
        public static readonly string CANCEL = "Cancel";

        public MessageDialog GetDefaultAskDialog(string message, string title, bool cancelBtnNeeded)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand(YES, null));
            dialog.Commands.Add(new UICommand(NO, null));
            if (cancelBtnNeeded)
            {
                dialog.Commands.Add(new UICommand(CANCEL, null));
                dialog.CancelCommandIndex = 2;
            }
            else
            {
                dialog.CancelCommandIndex = 1;
            }
            dialog.DefaultCommandIndex = 0;
            return dialog;
        }

        public MessageDialog GetDefaultNotification(string message, string title)
        {
            return new MessageDialog(message, title);
        }

    }
}
