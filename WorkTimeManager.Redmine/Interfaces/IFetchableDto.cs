using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Redmine.Interfaces
{
    public interface IFetchableDto<T>
    {
        List<T> ToEntityList();

        int getTotalCount();

        int getFetchedCount();
    }
}
