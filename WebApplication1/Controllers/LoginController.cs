using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;
using WebApplication1.Helpers;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Configuration;

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

        // === Fungsi hashing SHA256 ===
        private byte[] ComputeHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // === Fungsi verifikasi password ===
        private bool VerifyPassword(string enteredPassword, byte[] storedHash)
        {
            var enteredHash = ComputeHash(enteredPassword);
            return enteredHash.SequenceEqual(storedHash);
        }

        // === Generate JWT Token ===
        private string GenerateJwtToken(string username, string role)
        {
            var secret = ConfigurationManager.AppSettings["JwtSecret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("role", role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "yourapp",
                audience: "yourapp",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // === LOGIN ===
        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            try
            {
                var user = dblat.TBL_T_USERs.FirstOrDefault(f => f.USERNAME == username);

                if (user == null)
                {
                    return Json(new { Status = false, Message = "Invalid username or password" });
                }

                byte[] storedHash = user.PASSWORD_HASH.ToArray();

                if (VerifyPassword(password, storedHash))
                {
                    string token = GenerateJwtToken(user.USERNAME, user.Role);

                    // Simpan di session jika perlu
                    Session["username"] = user.USERNAME;
                    Session["nik"] = user.NIK;
                    Session["role"] = user.Role;
                    Session["token"] = token;

                    return Json(new { Status = true, Message = "Login successful", Token = token });
                }
                else
                {
                    return Json(new { Status = false, Message = "Invalid username or password" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.Message });
            }
        }

        //[HttpPost]
        //public JsonResult Login(string username, string password)
        //{
        //    try
        //    {
        //        bool cek = checkValidUser(username, password);

        //        if (cek == true)
        //        {
        //            var loginuser = dblat.TBL_T_USERs.Where(f => f.USERNAME.Equals(username)).SingleOrDefault();
        //            Session["username"] = loginuser.USERNAME;
        //            Session["password"] = loginuser.PASSWORD;
        //            Session["nik"] = loginuser.NIK;
        //            Session["Role"] = loginuser.Role;

        //            return Json(new { Status = true, Check = true/*, data = clsrent*/ }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { Status = false, Check = false }, JsonRequestBehavior.AllowGet);
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        return this.Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
        //    }  
        //}

        //public bool checkValidUser(string username, string pass)
        //{
        //    bool iReturn = false;

        //    try
        //    {
        //        var check_login = dblat.TBL_T_USERs.Where(f => f.USERNAME.Equals(username)).SingleOrDefault();
        //        string usr = check_login.USERNAME;
        //        string password = check_login.PASSWORD;
        //        if (username == usr && pass == password)
        //        {
        //            iReturn = true;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        var asd = ex.ToString();
        //        iReturn = false;
        //    }

        //    return iReturn;
        //}
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
    }
}