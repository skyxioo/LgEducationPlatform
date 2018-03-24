using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lg.EducationPlatform.jqDataTableModel
{
    /// <summary>
    /// 2.0 服务器返回的数据格式
    /// jquery $('selector').datatable()插件 需要的数据格式
    /// 用于在服务端构建插件ajax需要的JSON数据格式
    /// </summary>
    public class jqRealTableModel<T>
    {
        /// <summary>
        /// 1.0 实际的总记录数
        /// </summary>
        public int iTotalRecords { get; set; }

        /// <summary>
        /// 2.0 查询过滤之后，实际的行数。
        /// </summary>
        public int iTotalDisplayRecords { get; set; }

        /// <summary>
        /// 3.0 来自客户端sEcho的没有变化的复制品
        /// </summary>
        public string sEcho{get; set;}

        /// <summary>
        /// 4.0 可选，以逗号分隔的列名
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// 5.0 实际使用的数据. 数组的数组，表格中的实际数据。 
        /// </summary>
        public List<T> aaData { get; set; }

    }
}
