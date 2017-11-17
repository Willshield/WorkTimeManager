using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Model.Enums
{
    public enum DataLoadInterval
    {
        [DisplayName("Worktimes from last week")]
        IsLastWeek,
        [DisplayName("Worktimes from last two week")]
        IsLastTwoWeek,
        [DisplayName("Worktimes from last month")]
        IsLastMonth,
        [DisplayName("Worktimes from last two month")]
        IsLastTwoMonth,
        [DisplayName("Worktimes from last year")]
        IsLastYear,
        [DisplayName("All worktimes")]
        IsAll
    }
}