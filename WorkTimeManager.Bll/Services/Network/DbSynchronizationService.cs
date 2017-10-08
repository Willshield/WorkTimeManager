using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Bll.Interfaces.Network;

namespace WorkTimeManager.Bll.Services.Network
{
    public class DbSynchronizationService : IDbSynchronizationService
    {
        private static DbSynchronizationService instance = null;
        DbSynchronizationService() { }
        public static DbSynchronizationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbSynchronizationService();
                    instance.PullAll(); //Todo: kell?
                }

                return instance;
            }
        }

        public void PullAll()
        {
            throw new NotImplementedException();
        }

        public void PushAll()
        {
            throw new NotImplementedException();
        }
    }
}
