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
        #region
        //public List<SidebarViewModel> GetSidebarMenus()
        //{
        //    if (HttpContext.Current.Session["Role"] == null)
        //        return new List<SidebarViewModel>();

        //    string userRole = HttpContext.Current.Session["Role"].ToString();

        //    List<SidebarViewModel> menus;

        //    if (userRole == "Super Admin")
        //    {
        //        // Super Admin bisa melihat semua menu
        //        menus = _context.SidebarMenus
        //            .Where(m => m.IsActive.GetValueOrDefault())
        //            .Select(m => new SidebarViewModel
        //            {
        //                Id = m.Id,
        //                Name = m.Name,
        //                Icon = m.Icon,
        //                Url = m.Url,
        //                ParentId = m.ParentId,
        //                HasChildren = _context.SidebarMenus.Any(sub => sub.ParentId == m.Id)
        //            })
        //            .ToList();
        //    }
        //    else if (userRole == "Admin")
        //    {
        //        // Admin bisa melihat semua menu KECUALI Register
        //        menus = _context.SidebarMenus
        //            .Where(m => m.IsActive.GetValueOrDefault() &&
        //                        m.Name != "Register" &&
        //                        m.Name != "Pages" &&
        //                        m.Name != "Components" &&
        //                        m.Name != "Utilities") // Admin tidak bisa melihat Register
        //            .Select(m => new SidebarViewModel
        //            {
        //                Id = m.Id,
        //                Name = m.Name,
        //                Icon = m.Icon,
        //                Url = m.Url,
        //                ParentId = m.ParentId,
        //                HasChildren = _context.SidebarMenus.Any(sub => sub.ParentId == m.Id)
        //            })
        //            .ToList();
        //    }
        //    else if (userRole == "Warga")
        //    {
        //        // Warga tidak bisa melihat Register & Input Iuran
        //        menus = _context.SidebarMenus
        //            .Where(m => m.IsActive.GetValueOrDefault() &&
        //                        m.Name != "Register" &&
        //                        m.Name != "Input Iuran" &&
        //                        m.Name != "Upload Pengumuman" &&
        //                        m.Name != "Input Kegiatan" &&
        //                        m.Name != "Pages" &&
        //                        m.Name != "Components" &&
        //                        m.Name != "Upload Data Warga" &&
        //                        m.Name != "Utilities" &&
        //                        m.Name != "Approval") // Warga tidak bisa melihat Register & Input Iuran
        //            .Select(m => new SidebarViewModel
        //            {
        //                Id = m.Id,
        //                Name = m.Name,
        //                Icon = m.Icon,
        //                Url = m.Url,
        //                ParentId = m.ParentId,
        //                HasChildren = _context.SidebarMenus.Any(sub => sub.ParentId == m.Id)
        //            })
        //            .ToList();
        //    }
        //    else
        //    {
        //        // Jika role tidak dikenali, tampilkan menu berdasarkan role di database
        //        menus = _context.SidebarMenus
        //            .Where(m => m.IsActive.GetValueOrDefault() && (m.Role == null || m.Role == userRole))
        //            .Select(m => new SidebarViewModel
        //            {
        //                Id = m.Id,
        //                Name = m.Name,
        //                Icon = m.Icon,
        //                Url = m.Url,
        //                ParentId = m.ParentId,
        //                HasChildren = _context.SidebarMenus.Any(sub => sub.ParentId == m.Id)
        //            })
        //            .ToList();
        //    }

        //    return menus;
        //}
        #endregion
        public List<SidebarViewModel> GetSidebarMenus()
        {
            if (HttpContext.Current.Session["Role"] == null)
                return new List<SidebarViewModel>();

            string userRole = HttpContext.Current.Session["Role"].ToString();

            // Ambil semua menu aktif, join dengan role
            var menus = (from menu in _context.SidebarMenus
                         join role in _context.SidebarMenuRoles
                             on menu.Id equals role.SidebarMenuId into menuRoles
                         from role in menuRoles.DefaultIfEmpty() // left join agar menu tanpa role tetap tampil
                         where menu.IsActive == true &&
                               (role.Role == null || role.Role == userRole) // filter sesuai role
                         select new SidebarViewModel
                         {
                             Id = menu.Id,
                             Name = menu.Name,
                             Icon = menu.Icon,
                             Url = menu.Url,
                             ParentId = menu.ParentId,
                             HasChildren = _context.SidebarMenus.Any(sub => sub.ParentId == menu.Id),
                             Role = role.Role
                         }).Distinct().ToList();

            return menus;
        }

    }
}