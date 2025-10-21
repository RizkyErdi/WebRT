using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class KontakController : Controller
    {
        private DB_LATDataContext db = new DB_LATDataContext();

        // Menampilkan daftar pesan
        private KontakService _kontakService;
        private SidebarService _sidebarService;

        // Constructor dengan Dependency Injection Manual
        public KontakController()
        {
            // Membuat instance KontakService secara manual
            _sidebarService = new SidebarService();
            _kontakService = new KontakService(new DB_LATDataContext());
        }

        public ActionResult Index()
        {
            // Menyimpan URL GetMessages ke session
            string messagesUrl = Url.Action("GetMessages", "Kontak");
            Session["MessagesUrl"] = messagesUrl;  // Menyimpan URL di session

            return View();
        }

        // Action yang mengembalikan pesan dalam format JSON
        public JsonResult GetMessages()
        {
            var messages = _kontakService.GetMessages();  // Memanggil layanan untuk mendapatkan data
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        // Action untuk mendapatkan jumlah pesan yang belum dibaca
        public ActionResult GetUnreadCount()
        {
            var unreadCount = _kontakService.GetUnreadCount();
            return Json(unreadCount, JsonRequestBehavior.AllowGet);  // Mengembalikan jumlah pesan yang belum dibaca sebagai JSON
        }


        public ActionResult DaftarPesan(int id)
        {
            // Mendapatkan detail pesan berdasarkan ID
            var message = _kontakService.GetMessages().FirstOrDefault(m => m.Id == id);
            _kontakService.UpdateMessage(message);
            if (message == null)
            {
                //return Content("Pesan tidak ditemukan.");
                return HttpNotFound();  // Jika pesan tidak ditemukan
                message.IsRead = true;
                _kontakService.UpdateMessage(message); // Pastikan ada metode ini di service
            }
            //return Content($"<strong>Dari:</strong> {message.Name}<br><strong>Email:</strong> {message.Email}<br><strong>Isi:</strong> {message.Message}");
            return View("~/Views/Home/DaftarPesan.cshtml", message);  // Kirim detail pesan ke view
        }
    }
}