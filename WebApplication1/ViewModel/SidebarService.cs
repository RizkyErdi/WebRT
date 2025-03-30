using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.ViewModel
{
    public class SidebarService
    {
        private readonly DB_LATDataContext _context;

        public SidebarService()
        {
            _context = new DB_LATDataContext();
        }
        public List<SidebarViewModel> GetSidebarMenus()
        {
            var menus = _context.SidebarMenus
                .Where(m => (bool)m.IsActive)
                .Select(m => new SidebarViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Icon = m.Icon,
                    Url = m.Url,
                    ParentId = m.ParentId,
                    HasChildren = _context.SidebarMenus.Any(sub => sub.ParentId == m.Id)
                })
                .ToList();

            return menus ?? new List<SidebarViewModel>(); // Pastikan tidak null
        }
    }
}