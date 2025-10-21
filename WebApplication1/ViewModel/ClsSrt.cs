using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class ClsSrt
    {
        DB_SMTPDataContext dbmail = new DB_SMTPDataContext();
        public string Nama { get; set; }
        public string NIK { get; set; }
        public string Alamat { get; set; }
        public string JenisSurat { get; set; }
        public string Tujuan { get; set; }
        public string Dokumen { get; set; }
        public int StatusId { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Createddate { get; set; }

        public string savesurat(HttpPostedFileBase fileUpload, string nik)
        {
            string dokumenPath = "";

            try
            {
                // Simpan file jika ada
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/Pengajuan/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string originalFileName = Path.GetFileName(fileUpload.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + originalFileName;
                    string fullPath = Path.Combine(folderPath, uniqueFileName);

                    fileUpload.SaveAs(fullPath);

                    // Set path relatif untuk disimpan ke database
                    dokumenPath = "/Uploads/Pengajuan/" + uniqueFileName;
                }
                string data_tujuan;

                if(Tujuan == null)
                {
                    data_tujuan = "";
                }
                else
                {
                    data_tujuan = Tujuan;
                }
                TBL_T_AJUKAN_SURAT datasurat = new TBL_T_AJUKAN_SURAT()
                {
                    
                    nama_lengkap = Nama,
                    nik = NIK,
                    alamat = Alamat,
                    jenis_surat = JenisSurat,
                    tujuan = data_tujuan,
                    nomor_surat = "Test//01//02//surat",
                    dokumen_path = dokumenPath,
                    status = 1,
                    email =  "rizky.erdiansyah@outlook.com",
                    created_by = "SYSTEM",
                    created_date = Createddate == default(DateTime) ? DateTime.Now : Createddate
                };

                dbmail.TBL_T_AJUKAN_SURATs.InsertOnSubmit(datasurat);
                dbmail.SubmitChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }

        public List<TBL_T_AJUKAN_SURAT> GetPendingSurat()
        {
            return dbmail.TBL_T_AJUKAN_SURATs
                .Where(s => s.status == 1)
                .OrderByDescending(s => s.created_date)
                .ToList();
        }

        public bool ApproveSurat(int id)
        {
            try
            {
                var surat = dbmail.TBL_T_AJUKAN_SURATs.FirstOrDefault(s => s.id == id);
                if (surat != null)
                {
                    surat.status = 2;
                    dbmail.SubmitChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool RejectSurat(int id, string reason)
        {
            try
            {
                var surat = dbmail.TBL_T_AJUKAN_SURATs.FirstOrDefault(s => s.id == id);
                if (surat != null)
                {
                    surat.status = 3;
                    //surat.catatan = reason;
                    dbmail.SubmitChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}