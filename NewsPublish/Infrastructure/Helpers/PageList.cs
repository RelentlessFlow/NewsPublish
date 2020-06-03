using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewsPublish.Infrastructure.Helpers
{
    /// <summary>
    /// 自定义分页类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        // 当前页
        public int CurrentPage { get; private set; }
        // 总页数
        public int TotalPages { get; private set; }
        // 单页显示的数据条目数量
        public int PageSize { get; private set; }
        // 记录的总数
        public int TotalCount { get; private set; }
        // 是否有前一页
        public bool HasPrevious => CurrentPage > 1;
        // 是否有后一页
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double) pageSize);
            AddRange(items);
        }

        public async static Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber -1) * pageSize)
                .Take(pageSize).ToListAsync();
            return new PagedList<T>(items,count,pageNumber, pageSize);
        }
    }
}