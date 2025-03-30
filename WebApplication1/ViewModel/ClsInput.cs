using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class ClsInput
    {
        DB_LATDataContext dblat = new DB_LATDataContext();

        public string Nama { get; set; }
        public String Alamat { get; set; }
        public String StatusIuran { get; set; }
        public String Kontak { get; set; }

        public DateTime Created_Date { get; set; } = DateTime.Now;
        public string Created_By { get; set; } = "SYSTEM";
        public DateTime Updated_Date { get; set; } = DateTime.Now;
        public string Updated_By { get; set; } = "SYSTEM";

        public void clsinput()
        {
            TBL_T_INFORMASI_IURAN dataiuran = new TBL_T_INFORMASI_IURAN()
            {
                Nama = Nama,
                Alamat = Alamat,
                Status_Iuran = StatusIuran,
                Kontak = Kontak,
                Created_Date = System.DateTime.Now.Date,
                Created_By = "SYSTEM",
                Upated_Date = System.DateTime.Now,
                Updated_By = "SYSTEM"
            };

            try
            {
                dblat.TBL_T_INFORMASI_IURANs.InsertOnSubmit(dataiuran);
                dblat.SubmitChanges();
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}