using System;
using System.Collections.Generic;

namespace RESTDentalService.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNum)
        {
            Data = items;
            TotalItemsCount = totalCount;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        }
    }
}
