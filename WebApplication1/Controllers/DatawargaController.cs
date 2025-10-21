using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;
using ClosedXML.Excel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    public class DatawargaController : Controller
    {
        private readonly SidebarService _sidebarService;
        public DatawargaController()
        {
            _sidebarService = (SidebarService)Activator.CreateInstance(typeof(SidebarService));
        }
        // GET: Datawarga
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListDataWarga()
        {
            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

            return View(menus);
        }

        public JsonResult GetDataWarga()
        {
            var result = ClsInputWarga.GetAllData();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}