using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class ClsHome
    {
        DB_LATDataContext dblat = new DB_LATDataContext();

        public string username { get; set; }
        public String passwordhash { get; set; }
        public String password { get; set; }
        public String nik { get; set; }
        public String Role { get; set; }

        public void register()
        {
            dblat.Cusp_insertuser(username, password, nik, Role);
            
        }
    }
}