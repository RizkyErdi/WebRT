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
        public string NIK { get; set; }
        public string Alamat { get; set; }
        public string Status_Iuran { get; set; }
        public string Bulan_iuran { get; set; }
        public string Kontak { get; set; }

        public List<VSP_INFO_IURANResult> GetstatusIuran()
        {
            var statusiuran = dblat.VSP_INFO_IURAN().ToList();
            return statusiuran;
        }

    }
}