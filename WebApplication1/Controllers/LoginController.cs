using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
}

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        DB_LATDataContext dblat = new DB_LATDataContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        // Proses login
        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            try
            {
                bool cek = checkValidUser(username, password);

                if (cek == true)
                {
                    var loginuser = dblat.TBL_T_USERs.Where(f => f.USERNAME.Equals(username)).SingleOrDefault();
                    Session["username"] = loginuser.USERNAME;
                    Session["password"] = loginuser.PASSWORD;
                    
                    return Json(new { Status = true, Check = true/*, data = clsrent*/ }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = false, Check = false }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {
                return this.Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }  
        }
        public bool checkValidUser(string username, string pass)
        {
            bool iReturn = false;

            try
            {
                var check_login = dblat.TBL_T_USERs.Where(f => f.USERNAME.Equals(username)).SingleOrDefault();
                string usr = check_login.USERNAME;
                string password = check_login.PASSWORD;
                if (username == usr && pass == password)
                {
                    iReturn = true;
                }
                

            }
            catch (Exception ex)
            {
                var asd = ex.ToString();
                iReturn = false;
            }

            return iReturn;
        }
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
    }
}