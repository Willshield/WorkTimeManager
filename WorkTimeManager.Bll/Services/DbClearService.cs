using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.LocalDB.Context;

namespace WorkTimeManager.Bll.Services
{
    public class DbClearService : IDbClearService
    {

        private static DbClearService instance = null;
        DbClearService() { }
        public static DbClearService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbClearService();
                }

                return instance;
            }
        }
        
        public async Task ClearDb()
        {
            using (var db = new WorkTimeContext())
            {
                db.WorkTimes.RemoveRange(db.WorkTimes);
                db.Issues.RemoveRange(db.Issues);
                db.Projects.RemoveRange(db.Projects);
                await db.SaveChangesAsync();
            }
        }
    }
}
