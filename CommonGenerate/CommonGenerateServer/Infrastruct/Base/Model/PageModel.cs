using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastruct.Base.Model
{
    public class PageModel<T>
    {
        //
        // 摘要:
        //     分页索引
        public int PageIndex { get; set; }

        //
        // 摘要:
        //     分页大小
        public int PageSize { get; set; }

        //
        // 摘要:
        //     总记录数
        public int TotalCount { get; set; }

        //
        // 摘要:
        //     总页数
        public int TotalPages { get; set; }

        //
        // 摘要:
        //     返回数据
        public List<T> DataList { get; set; }
    }
}
