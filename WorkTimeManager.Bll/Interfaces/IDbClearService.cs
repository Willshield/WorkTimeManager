using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Bll.Interfaces
{
    public interface IDbClearService
    {
        Task ClearDb();
    }
}
