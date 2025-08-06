using System;
using System.Collections.Generic;
using System.Text;

namespace ManiFest.Model.SearchObjects
{
    public class BaseSearchObject
    {
        public string? FTS { get; set; }
        public int? Page { get; set; } = 0;
        public int? PageSize { get; set; } = 30;
        public bool IncludeTotalCount { get; set; } = false;
        public bool RetrieveAll { get; set; } = false;
    }
} 