using System.Collections.Generic;

namespace ManiFest.Model.Responses
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int? TotalCount { get; set; }
    }
} 
