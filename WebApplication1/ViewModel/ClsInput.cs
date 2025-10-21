using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel
{
    public class ClsInput
    {
        DB_LATDataContext dblat = new DB_LATDataContext();

        public string Nama { get; set; }
        public String NIK { get; set; }
        public String Alamat { get; set; }
        public String StatusIuran { get; set; }
        public String Bulan { get; set; }
        public String Kontak { get; set; }

        public DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public DateTime Updated_Date { get; set; }
        public string Updated_By { get; set; }

        public string TipePengumuman { get; set; }
        public int TahunPengumuman { get; set; }
        public string NamaFile { get; set; }
        public string TanggalUpload { get; set; }
        public string UploadedBy { get; set; }

        [Required(ErrorMessage = "Nama wajib diisi.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email wajib diisi.")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pesan wajib diisi.")]
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;


        public void clsinput(string username)
        {
            var existingData = dblat.TBL_T_INFORMASI_IURANs
                                             .FirstOrDefault(x => x.NIK == NIK && x.Bulan_Iuran == Bulan);
            if (existingData != null)
            {
                // Update data yang sudah ada
                existingData.Nama = Nama.ToUpper();
                existingData.Alamat = Alamat.ToUpper();
                existingData.Status_Iuran = StatusIuran.ToUpper();
                existingData.Kontak = Kontak;
                existingData.Upated_Date = DateTime.Now;
                existingData.Updated_By = username;
            }
            else
            {
                TBL_T_INFORMASI_IURAN dataiuran = new TBL_T_INFORMASI_IURAN()
                {
                    Nama = Nama.ToUpper(),
                    NIK = NIK,
                    Alamat = Alamat.ToUpper(),
                    Status_Iuran = StatusIuran.ToUpper(),
                    Bulan_Iuran = Bulan.ToUpper(),
                    Kontak = Kontak,
                    Created_Date = System.DateTime.Now.Date,
                    Created_By = username,
                    Upated_Date = System.DateTime.Now,
                    Updated_By = username
                };
                try
                {
                    dblat.TBL_T_INFORMASI_IURANs.InsertOnSubmit(dataiuran);
                    dblat.SubmitChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            TBL_H_INFORMASI_IURAN ditahist = new TBL_H_INFORMASI_IURAN()
            {
                Nama = Nama.ToUpper(),
                NIK = NIK,
                Alamat = Alamat.ToUpper(),
                Status_Iuran = StatusIuran.ToUpper(),
                Bulan_Iuran = Bulan.ToUpper(),
                Kontak = Kontak,
                Created_Date = System.DateTime.Now.Date,
                Created_By = username,
                Upated_Date = System.DateTime.Now,
                Updated_By = username
            };
            try
            {
                dblat.TBL_H_INFORMASI_IURANs.InsertOnSubmit(ditahist); ;
                dblat.SubmitChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public List<TBL_M_MONTH> BulanIuran()
        {
            var Bulan = dblat.TBL_M_MONTHs.ToList();

            return Bulan;
        }
        public static object ProsesUpload(HttpPostedFileBase file, string tipePengumuman, int tahunPengumuman, string username)
        {
            if (file == null || file.ContentLength == 0)
            {
                return new { Remarks = false, Message = "File PDF harus dipilih." };
            }

            string path = HttpContext.Current.Server.MapPath("~/Uploads/Pengumuman/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(path, fileName);
            file.SaveAs(filePath);

            // Simpan ke database
            using (var db = new DB_LATDataContext())
            {
                TBL_T_PENGUMUMAN pengumuman = new TBL_T_PENGUMUMAN
                {
                    tipe_pengumuman = tipePengumuman,
                    tahun_pengumuman = tahunPengumuman.ToString(),
                    nama_file = "/Uploads/Pengumuman/" + fileName,
                    tanggal_upload = DateTime.Now,
                    uploaded_by = username
                };
                db.TBL_T_PENGUMUMANs.InsertOnSubmit(pengumuman);
                db.SubmitChanges();
            }

            return new { Remarks = true, Message = "Pengumuman berhasil diunggah." };
        }
        public static List<object> GetPengumuman(int? tahun, string tipe)
        {
            try
            {
                using (var db = new DB_LATDataContext())
                {
                    var query = db.TBL_T_PENGUMUMANs.AsQueryable();

                    if (tahun.HasValue)
                    {
                        query = query.Where(p => p.tahun_pengumuman == tahun.Value.ToString());
                    }
                    if (!string.IsNullOrEmpty(tipe))
                    {
                        query = query.Where(p => p.tipe_pengumuman == tipe);
                    }

                    return query.Select(p => new
                    {
                        p.tipe_pengumuman,
                        p.tahun_pengumuman,
                        p.nama_file
                    }).ToList<object>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Terjadi kesalahan: " + ex.Message);
            }
        }
        public bool SaveToDatabase(string username)
        {
            try
            {
                using (var db = new DB_LATDataContext()) // Pastikan ApplicationDbContext sudah diatur
                {
                    var contactMessage = new TBL_T_KONTAK
                    {
                        Name = this.Name,
                        Email = this.Email,
                        Message = this.Message,
                        CreatedDate = DateTime.Now,
                        CreatedBy = username,
                        IsRead = this.IsRead
                    };

                    db.TBL_T_KONTAKs.InsertOnSubmit(contactMessage);
                    db.SubmitChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SimpanKegiatan(TBL_T_KEGIATAN kegiatan, string filePath, string username)
        {
            try
            {
                kegiatan.Undangan = filePath;
                kegiatan.CreatedBy = username;
                kegiatan.CreatedDate = DateTime.Now;

                dblat.TBL_T_KEGIATANs.InsertOnSubmit(kegiatan);
                dblat.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
        public List<TBL_T_KEGIATAN> GetKegiatan(string nama, DateTime? start, DateTime? end)
        {
            var query = dblat.TBL_T_KEGIATANs.AsQueryable();

            if (!string.IsNullOrEmpty(nama))
            {
                query = query.Where(k => k.NamaKegiatan == nama);
            }

            if (start.HasValue)
            {
                query = query.Where(k => k.Tanggal >= start.Value);
            }

            if (end.HasValue)
            {
                query = query.Where(k => k.Tanggal <= end.Value);
            }

            return query.ToList();
        }
    }
}