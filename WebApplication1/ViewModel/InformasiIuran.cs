using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class InformasiIuran
    {
        DB_LATDataContext dblat = new DB_LATDataContext();
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string Status_Iuran { get; set; }
        public string Kontak { get; set; }

        public List<TBL_T_INFORMASI_IURAN> GetstatusIuran()
        {
            var statusiuran = dblat.TBL_T_INFORMASI_IURANs.ToList();
            return statusiuran;
        }

    }
}