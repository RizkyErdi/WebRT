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


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
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
        private readonly SidebarService _sidebarService;
        public ActionResult Sidebar()
        {
            List<SidebarViewModel> menus = _sidebarService.GetSidebarMenus() ?? new List<SidebarViewModel>();
            return PartialView("_Sidebar", menus);
        }
        public ActionResult test()
        {
            DB_LATDataContext dblat = new DB_LATDataContext();
            var menus = dblat.SidebarMenus
                             .Where(m => (bool)m.IsActive)
                             .Select(m => new SidebarViewModel
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 Icon = m.Icon,
                                 Url = m.Url,
                                 ParentId = m.ParentId
                             })
                             .ToList() ?? new List<SidebarViewModel>();

            return View(menus);
            //return View();
            //var menus = dblat.SidebarMenus.Where(m => (bool)m.IsActive).ToList();
            //ViewBag.SidebarMenus = menus; // Jangan sampai null
            //return View();
        }
        public ActionResult About()
        {
            //if(Session["username"] == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            DB_LATDataContext dblat = new DB_LATDataContext();
            var menus = dblat.SidebarMenus
                             .Where(m => (bool)m.IsActive)
                             .Select(m => new SidebarViewModel
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 Icon = m.Icon,
                                 Url = m.Url,
                                 ParentId = m.ParentId
                             })
                             .ToList() ?? new List<SidebarViewModel>();

            return View(menus);
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Hapus semua session
            Session.Abandon(); // Hentikan sesi pengguna
            return RedirectToAction("Index", "Home"); // Kembali ke halaman login
        }

        public ActionResult InformasiKK()
        {
            //if (Session["username"] == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            DB_LATDataContext dblat = new DB_LATDataContext();
            var menus = dblat.SidebarMenus
                             .Where(m => (bool)m.IsActive)
                             .Select(m => new SidebarViewModel
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 Icon = m.Icon,
                                 Url = m.Url,
                                 ParentId = m.ParentId
                             })
                             .ToList() ?? new List<SidebarViewModel>();

            return View(menus);
        }
        public ActionResult login()
        {
            //if (Session["username"] == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            return View();
        }
        public ActionResult Table()
        {
            DB_LATDataContext dblat = new DB_LATDataContext();
            var menus = dblat.SidebarMenus
                             .Where(m => (bool)m.IsActive)
                             .Select(m => new SidebarViewModel
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 Icon = m.Icon,
                                 Url = m.Url,
                                 ParentId = m.ParentId
                             })
                             .ToList() ?? new List<SidebarViewModel>();

            return View(menus);
        }

        public ActionResult InputIuran()
        {
            DB_LATDataContext dblat = new DB_LATDataContext();
            var menus = dblat.SidebarMenus
                             .Where(m => (bool)m.IsActive)
                             .Select(m => new SidebarViewModel
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 Icon = m.Icon,
                                 Url = m.Url,
                                 ParentId = m.ParentId
                             })
                             .ToList() ?? new List<SidebarViewModel>();

            return View(menus);
        }

        // Download Template Excel
        public ActionResult DownloadTemplate()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Template");
                ws.Cell(1, 1).Value = "Nama";
                ws.Cell(1, 2).Value = "Alamat";
                ws.Cell(1, 3).Value = "Status Iuran (Lunas/Belum Lunas)";
                ws.Cell(1, 4).Value = "Kontak (Hanya Angka)";

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Template_Iuran.xlsx");
                }
            }
        }

        #region upload
        // Upload & Process Excel
        //[HttpPost]
        //public ActionResult UploadExcel(HttpPostedFileBase file)
        //{
        //    DB_LATDataContext dblat = new DB_LATDataContext();

        //    if (file == null || file.ContentLength == 0)
        //    {
        //        TempData["Error"] = "File tidak ditemukan!";
        //        return RedirectToAction("Index");
        //    }

        //    try
        //    {
        //        using (XLWorkbook wb = new XLWorkbook(file.InputStream))
        //        {
        //            var ws = wb.Worksheet(1);
        //            var rows = ws.RangeUsed().RowsUsed().Skip(1); // Lewati header
        //            List<ClsInput> dataList = new List<ClsInput>();

        //            foreach (var row in rows)
        //            {
        //                string nama = row.Cell(1).GetString().Trim();
        //                string alamat = row.Cell(2).GetString().Trim();
        //                string statusIuran = row.Cell(3).GetString().Trim();
        //                string kontak = row.Cell(4).GetString().Trim();

        //                // Validasi
        //                if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(alamat) ||
        //                    (statusIuran != "Lunas" && statusIuran != "Belum Lunas") ||
        //                    !kontak.All(char.IsDigit))
        //                {
        //                    TempData["Error"] = "Format data salah. Periksa kembali file Anda!";
        //                    return RedirectToAction("Index");
        //                }

        //                dataList.Add(new ClsInput
        //                {
        //                    Nama = nama,
        //                    Alamat = alamat,
        //                    StatusIuran = statusIuran,
        //                    Kontak = kontak,
        //                    Created_Date = DateTime.Now,
        //                    Updated_Date = DateTime.Now
        //                });
        //            }

        //            var dataToInsert = dataList.Select(item => new TBL_T_INFORMASI_IURAN
        //            {
        //                Nama = item.Nama,
        //                Alamat = item.Alamat,
        //                Status_Iuran = item.StatusIuran, // Pastikan nama properti sesuai
        //                Kontak = item.Kontak,
        //                Created_Date = item.Created_Date,
        //                Created_By = "SYSTEM",
        //                Upated_Date = item.Updated_Date,
        //                Updated_By = "SYSTEM"
        //            }).ToList();

        //            dblat.TBL_T_INFORMASI_IURANs.InsertAllOnSubmit(dataToInsert);
        //            dblat.SubmitChanges();
        //            TempData["Success"] = "Data berhasil diupload!";
        //        }
        //        TempData["Success"] = "Data berhasil diupload!";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "Upload gagal: " + ex.Message;
        //    }

        //    return RedirectToAction("InputIuran");
        //}
        #endregion upload

        [HttpPost]
        public JsonResult UploadExcel(HttpPostedFileBase file)
        {
            DB_LATDataContext db = new DB_LATDataContext();
            if (file == null || file.ContentLength == 0)
            {
                return Json(new { success = false, message = "File tidak boleh kosong!" });
            }

            try
            {
                using (var workbook = new XLWorkbook(file.InputStream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header

                    List<TBL_T_INFORMASI_IURAN> dataList = new List<TBL_T_INFORMASI_IURAN>();

                    foreach (var row in rows)
                    {
                        var data = new TBL_T_INFORMASI_IURAN
                        {
                            Nama = row.Cell(1).Value.ToString(),
                            Alamat = row.Cell(2).Value.ToString(),
                            Status_Iuran = row.Cell(3).Value.ToString(),
                            Kontak = row.Cell(4).Value.ToString(),
                            Created_Date = DateTime.Now,
                            Created_By = "SYSTEM",
                            Upated_Date = DateTime.Now,
                            Updated_By = "SYSTEM"
                        };

                        dataList.Add(data);
                    }

                    db.TBL_T_INFORMASI_IURANs.InsertAllOnSubmit(dataList);
                    db.SubmitChanges();

                    return Json(new { success = true, message = "Data berhasil diupload!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }


        [HttpPost]
        //[AllowAnonymous]
        public JsonResult GetOrders()
        {
            try
            {
                InformasiIuran clsinformasiiuran = new InformasiIuran();
                var orders = clsinformasiiuran.GetstatusIuran().Select(o => new InformasiIuran
                {
                    Id = o.Id,
                    Nama = o.Nama,
                    Alamat = o.Alamat,
                    Status_Iuran = o.Status_Iuran,
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
    }
}