using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WorkTimeManager.Bll;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Services
{
    public class TrackingSafeStarterService
    {
        private readonly PopupService popupService;

        public TrackingSafeStarterService()
        {
            popupService = new PopupService();
        }

        public async Task<bool> AskStartTracking(Issue issue)
        {
            var tracker = TrackerService.Instance;
            if (!tracker.HasPendingTrack)
            {
                tracker.StartTracking(issue);
                return true;
            }
            else
            {
                MessageDialog dialog = popupService.GetDefaultAskDialog("You are already tracking an issue. Do you want to save its time? Select cancel if you want to stay tracking.",
                                                                        "Already tracking an issue", true);
                var cmd = await dialog.ShowAsync();
                if (cmd.Label == PopupService.CANCEL)
                {
                    return false;
                }
                else if (cmd.Label == PopupService.YES)
                {
                    await tracker.StopAndSaveTracking();
                    tracker.StartTracking(issue);
                    return true;
                }
                else if (cmd.Label == PopupService.NO)
                {
                    tracker.AbortTracking();
                    tracker.StartTracking(issue);
                    return true;
                }
            }
            return false;
        } 
    }
}
