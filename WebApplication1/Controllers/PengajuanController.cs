using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class PengajuanController : Controller
    {
        // GET: Pengajuan
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveSurat(ClsSrt clsinput, HttpPostedFileBase dokumen)
        {
            var nik = Session["nik"] != null ? Session["nik"].ToString() : "3216073101000001";
            var result = clsinput.savesurat(dokumen, nik);

            if (result != "")
            {
                return Json(new { Remarks = false, Message = "System Error: " + result });
            }

            ClsGeneral clsGeneral = new ClsGeneral();
            var sendmailresult = clsGeneral.SendMailApprover(clsinput.JenisSurat, clsinput.NIK, clsinput.Nama, clsinput.Alamat, clsinput.Tujuan, "https://www.google.com/", nik);
            if (sendmailresult != "")
            {
                return Json(new { Remarks = false, Message = "System Error : " + sendmailresult });
            }

            return Json(new { Remarks = true, Message = "Success submit form" });
        }

        public JsonResult GetListSurat()
        {
            var list = new ClsSrt().GetPendingSurat(); // ambil data dari class
            //Console.WriteLine("Jumlah data: " + list.Count);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveSurat(int id)
        {
            var result = new ClsSrt().ApproveSurat(id);
            return Json(new { Message = result ? "Surat berhasil di-approve." : "Gagal approve surat." });
        }

        [HttpPost]
        public JsonResult RejectSurat(int id, string reason)
        {
            var result = new ClsSrt().RejectSurat(id, reason);
            return Json(new { Message = result ? "Surat berhasil di-reject." : "Gagal reject surat." });
        }
    }
}