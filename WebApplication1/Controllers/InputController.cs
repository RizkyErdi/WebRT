using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using WebApplication1.ViewModel;
using ClosedXML.Excel;


namespace WebApplication1.Controllers
{
    public class InputController : Controller
    {
        // GET: Input
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[Route("UploadIuran")]
        public JsonResult UploadIuran(ClsInput clsInput)
        {
            try
            {

                clsInput.clsinput();


                return Json(new { Remarks = true, Message = "Success Upload" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Remarks = false, Message = "Error : " + e.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}