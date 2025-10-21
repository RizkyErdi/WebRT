using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class SidebarViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public bool HasChildren { get; set; }
        public string Role { get; set; }
        public bool IsRead { get; set; }

    }
}