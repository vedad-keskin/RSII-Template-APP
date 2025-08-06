using System;
using System.Collections.Generic;
using System.Text;

namespace ManiFest.Model.SearchObjects
{
    public class UserSearchObject : BaseSearchObject
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int? GenderId { get; set; }
        public int? CityId { get; set; }
        public int? RoleId { get; set; }
    }
} 