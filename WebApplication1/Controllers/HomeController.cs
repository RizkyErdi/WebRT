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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.Configuration;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly SidebarService _sidebarService;

        public HomeController()
        {
            _sidebarService = (SidebarService)Activator.CreateInstance(typeof(SidebarService));
        }

        /// <summary>
        /// Fungsi helper untuk validasi token dan role.
        /// </summary>
        /// <param name="disallowedRoles">Daftar role yang tidak boleh mengakses halaman.</param>
        private ActionResult ValidateTokenAccess(params string[] disallowedRoles)
        {
            // Ambil token dari session
            var token = Session["token"] as string;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var secret = ConfigurationManager.AppSettings["JwtSecret"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                // Validasi token
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.FromMinutes(2)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                // Cek role yang tidak diizinkan
                if (disallowedRoles.Contains(userRole))
                {
                    return RedirectToAction("Index", "Home");
                }

                // ✅ Token valid dan role diizinkan
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Token validation error: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string userRole = Session["Role"].ToString();
            if (userRole != "Super Admin")
            {
                //TempData["AlertMessage"] = "Akses ditolak! Hanya Super Admin yang boleh mengakses.";
                return RedirectToAction("Index", "Home");
            }

            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

            return View(menus);
        }

        [HttpPost]
        public JsonResult Registrasi(ClsHome clsregister)
        {
            try
            {

                clsregister.register();


                return Json(new { Remarks = true, Message = "Success Upload" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Remarks = false, Message = "Error : " + e.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult test()
        {
            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

            return View(menus);
        }

        public ActionResult PengajuanSrt()
        {
            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

            return View(menus);
        }

        public ActionResult ApprovalSrt()
        {
            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

            return View(menus);
        }

        public ActionResult DataWargaUpload()
        {
            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

            return View(menus);
        }
        //public ActionResult About()
        //{
        //    if (Session["Role"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

        //    return View(menus);
        //}

        public ActionResult Logout()
        {
            Session.Clear(); // Hapus semua session
            Session.Abandon(); // Hentikan sesi pengguna
            return RedirectToAction("Index", "Home"); // Kembali ke halaman login
        }

        //public ActionResult InputAct()
        //{
        //    if (Session["Role"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    string userRole = Session["Role"].ToString();
        //    if (userRole == "Warga")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

        //    return View(menus);
        //}

        public ActionResult InputAct()
        {
            var result = ValidateTokenAccess("Warga"); // Role 'Warga' tidak boleh akses
            if (result != null)
                return result;

            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();
            return View(menus);
        }

        public ActionResult About()
        {
            var result = ValidateTokenAccess(); // Semua role diizinkan
            if (result != null)
                return result;

            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();
            return View(menus);
        }

    public ActionResult login()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Table()
        {
            var result = ValidateTokenAccess(); // semua role boleh
            if (result != null) return result;

            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();
            return View(menus);
        }

        public ActionResult InputIuran()
        {
            var result = ValidateTokenAccess("Super Admin", "Admin"); // hanya admin/super admin
            if (result != null) return result;

            ViewBag.Username = Session["username"].ToString();
            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();
            return View(menus);
        }

        public ActionResult UploadInfo()
        {
            var result = ValidateTokenAccess("Super Admin", "Admin");
            if (result != null) return result;

            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();
            return View(menus);
        }

        public ActionResult DaftarInfo()
        {
            var result = ValidateTokenAccess(); // semua role boleh
            if (result != null) return result;

            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();
            return View(menus);
        }

        public ActionResult Kontak()
        {
            var result = ValidateTokenAccess();
            if (result != null) return result;

            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();
            return View(menus);
        }

        public ActionResult DetailAct()
        {
            var result = ValidateTokenAccess();
            if (result != null) return result;

            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();
            return View(menus);
        }


        #region
        //public ActionResult Table()
        //{
        //    if (Session["Role"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

        //    return View(menus);
        //}

        //public ActionResult InputIuran()
        //{
        //    if (Session["Role"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    string userRole = Session["Role"].ToString();
        //    if (userRole != "Super Admin" && userRole != "Admin")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ViewBag.Username = Session["username"].ToString();

        //    List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

        //    return View(menus);
        //}

        //public ActionResult UploadInfo()
        //{
        //    if (Session["Role"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    string userRole = Session["Role"].ToString();
        //    if (userRole != "Super Admin" && userRole != "Admin")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

        //    return View(menus);
        //}

        //public ActionResult DaftarInfo()
        //{
        //    if (Session["Role"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    //string userRole = Session["Role"].ToString();
        //    //if (userRole != "Super Admin" || userRole != "Admin")
        //    //{
        //    //    return RedirectToAction("Index", "Home");
        //    //}

        //    List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

        //    return View(menus);
        //}

        //public ActionResult Kontak()
        //{
        //    if (Session["Role"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

        //    return View(menus);
        //}

        //public ActionResult DetailAct()
        //{
        //    if (Session["Role"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus();

        //    return View(menus);
        //}
        #endregion

        [HttpPost]
        //[AllowAnonymous]
        public JsonResult GetOrders()
        {
            try
            {
                InformasiIuran clsinformasiiuran = new InformasiIuran();
                var orders = clsinformasiiuran.GetstatusIuran().Select(o => new InformasiIuran
                {
                    //Id = o.Id,
                    Nama = o.Nama,
                    NIK = o.NIK,
                    Alamat = o.Alamat,
                    Status_Iuran = o.Status_Iuran,
                    Bulan_iuran = o.Bulan_Iuran,
                    Kontak = o.Kontak

                }).ToList();

                return Json(orders, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetOrders: " + ex.Message);
                return Json(new { error = "Terjadi kesalahan di server", details = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetBulan()
        {
            try
            {
                ClsInput ambilBulan = new ClsInput();
                var data = ambilBulan.BulanIuran();

                return Json(new { success = true, message = "Data retrieved successfully!", data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}