using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.WebHelper
{
    public class PagerHelper
    {
        /// <summary>
        /// 生成分页Html
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static string GerneratePagerString(int currentPage, int pageSize, int totalCount)
        {
            var redirectUrl = OperateHelper.Current.Request.Url.AbsolutePath;
            pageSize = pageSize <= 0 ? 5 : pageSize;

            //总页数
            int totalPages = (totalCount + pageSize - 1) / pageSize;

            //分页条容量，一个分页条显示6个页码
            int pageBarSize = 6;
            //分页条数
            int totalPageBars = (totalPages + pageBarSize - 1) / pageBarSize;
            //当前页在哪个分页条内，0表示第一个分页条
            int position = (currentPage - 1) / pageBarSize;
            //计算当前分页条第一个页码
            int start = position * pageBarSize + 1;

            //分页条容量，默认为pageBarSize，最后一个分页条计算实际页条容量
            int pageBarCapacity = pageBarSize;
            if (position == totalPageBars - 1) //最后一个分页条
                pageBarCapacity = totalPages - (totalPageBars - 1) * pageBarSize;


            StringBuilder pagerHtmlString = new StringBuilder();
            //处理首页
            pagerHtmlString.AppendFormat("<li class='prev-page'><a href='{0}?pageIndex={1}&pageSize={2}'>首页</a></li>",
                redirectUrl, 1, pageSize);
            //当前页不是首页，加上“上一页”
            if (currentPage > 1)
            {
                pagerHtmlString.AppendFormat(
                    "<li class='prev-page'><a href='{0}?pageIndex={1}&pageSize={2}'>上一页</a></li>", redirectUrl,
                    currentPage - 1, pageSize);
            }
            ;
            //数字页码
            for (int i = 0; i < pageBarCapacity; i++)
            {
                int j = start + i;
                if (j == currentPage)
                {
                    //当前页
                    pagerHtmlString.AppendFormat("<li class='active'><span>{0}</span></li>", j);
                    //当页容量为满页（6页）时，当前页为当前页码条最后一页显示…
                    if (pageBarCapacity == pageBarSize && currentPage == (start + pageBarSize - 1))
                    {
                        if (currentPage + 1 < totalPages)
                        {
                            pagerHtmlString.AppendFormat("<li><a href='{0}?pageIndex={1}&pageSize={2}'>{3}</a></li>",
                                redirectUrl, j + 1, pageSize, j + 1);
                            pagerHtmlString.Append("<li><span>...</span></li>");
                            pagerHtmlString.AppendFormat("<li><a href='{0}?pageIndex={1}&pageSize={2}'>{3}</a></li>",
                                redirectUrl, totalPages, pageSize, totalPages);
                        }
                    }
                }
                else
                {
                    pagerHtmlString.AppendFormat(
                        "<li><a href='{0}?pageIndex={1}&pageSize={2}'>{3}</a></li>", redirectUrl, j,
                        pageSize, j);
                }
            }

            //当前页不是最后一页，加上“下一页”
            if (currentPage < totalPages)
            {
                pagerHtmlString.AppendFormat(
                    "<li class='next-page'><a href='{0}?pageIndex={1}&pageSize={2}'>下一页</a></li>", redirectUrl,
                    currentPage + 1, pageSize);
            }
            //尾页
            pagerHtmlString.AppendFormat("<li class='next-page'><a href='{0}?pageIndex={1}&pageSize={2}'>尾页</a></li>",
                redirectUrl, totalPages, pageSize);
            //总页数显示
            pagerHtmlString.AppendFormat("<li><span>共-{0}-页</span></li> ", totalPages);

            return pagerHtmlString.ToString();
        }
    }
}
