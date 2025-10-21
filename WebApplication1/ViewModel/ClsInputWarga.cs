using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.ViewModel
{
    public class ClsInputWarga
    {
        DB_LATDataContext dblat = new DB_LATDataContext();

        public int Id { get; set; }
        public string NIK { get; set; }
        public string NoKK { get; set; }
        public string Nama { get; set; }
        public string TempatLahir { get; set; }
        public DateTime TglLahir { get; set; }
        public string JenisKelamin { get; set; }
        public string Alamat { get; set; }
        public string RT { get; set; }
        public string RW { get; set; }
        public string Kelurahan { get; set; }
        public string Kecamatan { get; set; }
        public string Agama { get; set; }
        public string StatusKawin { get; set; }
        public string Pekerjaan { get; set; }
        public string Kewarganegaraan { get; set; }
        public string NoHp { get; set; }
        public string Email { get; set; }
        public DateTime TglDaftar { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public String InputDataWarga(string username)
        {
            TBL_T_WARGA existingData = dblat.TBL_T_WARGAs
    .FirstOrDefault(x => x.NIK == NIK && x.NoKK == NoKK);

            if (existingData != null)
            {
                // Update data yang sudah ada
                existingData.Nama = Nama;
                existingData.TempatLahir = TempatLahir;
                existingData.TanggalLahir = TglLahir;
                existingData.JenisKelamin = JenisKelamin;
                existingData.Alamat = Alamat;
                existingData.RT = RT;
                existingData.RW = RW;
                existingData.Kelurahan = Kelurahan;
                existingData.Kecamatan = Kecamatan;
                existingData.Agama = Agama;
                existingData.StatusPerkawinan = StatusKawin;
                existingData.Pekerjaan = Pekerjaan;
                existingData.Kewarganegaraan = Kewarganegaraan;
                existingData.NoHP = NoHp;
                existingData.Email = Email;
                existingData.TanggalTerdaftar = TglDaftar;
                existingData.UpdatedBy = username;
                existingData.UpdatedDate = DateTime.Now;
            }
            else
            {
                // Insert data baru
                TBL_T_WARGA datawarga = new TBL_T_WARGA()
                {
                    NIK = NIK,
                    NoKK = NoKK,
                    Nama = Nama,
                    TempatLahir = TempatLahir,
                    TanggalLahir = TglLahir,
                    JenisKelamin = JenisKelamin,
                    Alamat = Alamat,
                    RT = RT,
                    RW = RW,
                    Kelurahan = Kelurahan,
                    Kecamatan = Kecamatan,
                    Agama = Agama,
                    StatusPerkawinan = StatusKawin,
                    Pekerjaan = Pekerjaan,
                    Kewarganegaraan = Kewarganegaraan,
                    NoHP = NoHp,
                    Email = Email,
                    TanggalTerdaftar = TglDaftar,
                    CreatedBy = username,
                    CreatedDate = DateTime.Now,
                };

                dblat.TBL_T_WARGAs.InsertOnSubmit(datawarga);
            }

            try
            {
                dblat.SubmitChanges();
                return "Data berhasil disimpan.";
            }
            catch (Exception ex)
            {
                return "Terjadi kesalahan saat menyimpan data: " + ex.Message;
            }
        }
        public static List<TBL_T_WARGA> GetAllData()
        {
            using (var db = new DB_LATDataContext())
            {
                return db.TBL_T_WARGAs.OrderBy(x => x.Nama).ToList();
            }
        }
    }
}