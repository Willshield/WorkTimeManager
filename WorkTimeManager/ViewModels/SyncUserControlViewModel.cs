using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.Bll.Services.Network;

namespace WorkTimeManager.ViewModels
{
    class SyncUserControlViewModel
    {
        IDbSynchronizationService Syncer;
        public DelegateCommand PushCommand { get; }
        public SyncUserControlViewModel()
        {
            Syncer = DbSynchronizationService.Instance;
            PushCommand = new DelegateCommand(Push);
        }
        public void Push()
        {
            Syncer.PushAll();
        }
    }
}
