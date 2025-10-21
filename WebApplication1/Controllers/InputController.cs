using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;
using ClosedXML.Excel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;


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
            var username = Session["username"].ToString();
            try
            {

                clsInput.clsinput(username);


                return Json(new { Remarks = true, Message = "Success Upload" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Remarks = false, Message = "Error : " + e.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        // Download Template Excel
        public ActionResult DownloadTemplate()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Template");
                ws.Cell(1, 1).Value = "Nama";
                ws.Cell(1, 2).Value = "NIK";
                ws.Cell(1, 3).Value = "Alamat";
                ws.Cell(1, 4).Value = "Status Iuran (Lunas/Belum Lunas)";
                ws.Cell(1, 5).Value = "Bulan Iuran (Bulan Tahun)";
                ws.Cell(1, 6).Value = "Kontak (Hanya Angka)";
                ws.Column(2).Style.NumberFormat.Format = "@";
                ws.Column(6).Style.NumberFormat.Format = "@";
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Template_Iuran.xlsx");
                }
            }
        }

        #region upload
        // Upload & Process Excel
        //[HttpPost]
        //public ActionResult UploadExcel(HttpPostedFileBase file)
        //{
        //    DB_LATDataContext dblat = new DB_LATDataContext();

        //    if (file == null || file.ContentLength == 0)
        //    {
        //        TempData["Error"] = "File tidak ditemukan!";
        //        return RedirectToAction("Index");
        //    }

        //    try
        //    {
        //        using (XLWorkbook wb = new XLWorkbook(file.InputStream))
        //        {
        //            var ws = wb.Worksheet(1);
        //            var rows = ws.RangeUsed().RowsUsed().Skip(1); // Lewati header
        //            List<ClsInput> dataList = new List<ClsInput>();

        //            foreach (var row in rows)
        //            {
        //                string nama = row.Cell(1).GetString().Trim();
        //                string alamat = row.Cell(2).GetString().Trim();
        //                string statusIuran = row.Cell(3).GetString().Trim();
        //                string kontak = row.Cell(4).GetString().Trim();

        //                // Validasi
        //                if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(alamat) ||
        //                    (statusIuran != "Lunas" && statusIuran != "Belum Lunas") ||
        //                    !kontak.All(char.IsDigit))
        //                {
        //                    TempData["Error"] = "Format data salah. Periksa kembali file Anda!";
        //                    return RedirectToAction("Index");
        //                }

        //                dataList.Add(new ClsInput
        //                {
        //                    Nama = nama,
        //                    Alamat = alamat,
        //                    StatusIuran = statusIuran,
        //                    Kontak = kontak,
        //                    Created_Date = DateTime.Now,
        //                    Updated_Date = DateTime.Now
        //                });
        //            }

        //            var dataToInsert = dataList.Select(item => new TBL_T_INFORMASI_IURAN
        //            {
        //                Nama = item.Nama,
        //                Alamat = item.Alamat,
        //                Status_Iuran = item.StatusIuran, // Pastikan nama properti sesuai
        //                Kontak = item.Kontak,
        //                Created_Date = item.Created_Date,
        //                Created_By = "SYSTEM",
        //                Upated_Date = item.Updated_Date,
        //                Updated_By = "SYSTEM"
        //            }).ToList();

        //            dblat.TBL_T_INFORMASI_IURANs.InsertAllOnSubmit(dataToInsert);
        //            dblat.SubmitChanges();
        //            TempData["Success"] = "Data berhasil diupload!";
        //        }
        //        TempData["Success"] = "Data berhasil diupload!";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "Upload gagal: " + ex.Message;
        //    }

        //    return RedirectToAction("InputIuran");
        //}
        #endregion upload

        [HttpPost]
        public JsonResult UploadExcel(HttpPostedFileBase file)
        {
            DB_LATDataContext db = new DB_LATDataContext();
            if (file == null || file.ContentLength == 0)
            {
                return Json(new { success = false, message = "File tidak boleh kosong!" });
            }

            try
            {
                using (var workbook = new XLWorkbook(file.InputStream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header

                    List<TBL_T_INFORMASI_IURAN> dataList = new List<TBL_T_INFORMASI_IURAN>();
                    List<TBL_H_INFORMASI_IURAN> datahist = new List<TBL_H_INFORMASI_IURAN>();
                    List<string> errorMessages = new List<string>();

                    string bulanIuranPattern = @"^(Januari|Februari|Maret|April|Mei|Juni|Juli|Agustus|September|Oktober|November|Desember)\s\d{4}$";

                    bool isAnyDataRow = false;

                    foreach (var row in rows)
                    {
                        string nama = row.Cell(1).Value.ToString().Trim();
                        string nik = row.Cell(2).Value.ToString().Trim();
                        string alamat = row.Cell(3).Value.ToString().Trim();
                        string statusIuran = row.Cell(4).Value.ToString().Trim();
                        string bulanIuran = row.Cell(5).Value.ToString().Trim();
                        string nomorHp = row.Cell(6).Value.ToString().Trim(); // Ambil nomor HP dari kolom 4

                        if (string.IsNullOrEmpty(nama) && string.IsNullOrEmpty(nik) && string.IsNullOrEmpty(alamat) && string.IsNullOrEmpty(statusIuran) && string.IsNullOrEmpty(bulanIuran) && string.IsNullOrEmpty(nomorHp))
                        {
                            continue; // Skip baris kosong
                        }
                        isAnyDataRow = true; // Mark that there's valid data
                        if (string.IsNullOrEmpty(nama))
                        {
                            errorMessages.Add($"❌ Nama tidak boleh kosong (baris {row.RowNumber()})");
                        }
                        // Validasi NIK
                        if (string.IsNullOrEmpty(nik))
                        {
                            errorMessages.Add($"❌ NIK tidak boleh kosong (baris {row.RowNumber()})");
                        }
                        else if (nik.Length != 16)
                        {
                            errorMessages.Add($"❌ NIK harus 16 digit (baris {row.RowNumber()})");
                        }
                        else if (!Regex.IsMatch(nik, @"^\d{16}$"))
                        {
                            errorMessages.Add($"❌ NIK hanya boleh angka (baris {row.RowNumber()})");
                        }
                        // Validasi Alamat
                        if (string.IsNullOrEmpty(alamat))
                        {
                            errorMessages.Add($"❌ Alamat tidak boleh kosong (baris {row.RowNumber()})");
                        }
                        // Validasi Status Iuran
                        if (string.IsNullOrEmpty(statusIuran))
                        {
                            errorMessages.Add($"❌ Status Iuran harus diisi (baris {row.RowNumber()})");
                        }
                        else if (statusIuran != "Lunas" && statusIuran != "Belum Lunas")
                        {
                            errorMessages.Add($"❌ Status Iuran hanya bisa 'Lunas' atau 'Belum Lunas' PERHATIKAN BESAR KECIL HURUF!(baris {row.RowNumber()})");
                        }
                        // Validasi Bulan Iuran
                        if (string.IsNullOrEmpty(bulanIuran))
                        {
                            errorMessages.Add($"❌ Bulan Iuran harus diisi (baris {row.RowNumber()})");
                        }
                        else if (!Regex.IsMatch(bulanIuran, bulanIuranPattern))
                        {
                            errorMessages.Add($"❌ Bulan Iuran harus dalam format 'Bulan Tahun' (contoh: Januari 2024) PERHATIKAN BESAR KECIL HURUF!(baris {row.RowNumber()})");
                        }
                        // Validasi Kontak
                        if (string.IsNullOrEmpty(nomorHp))
                        {
                            errorMessages.Add($"❌ Kontak tidak boleh kosong (baris {row.RowNumber()})");
                        }
                        else if (!Regex.IsMatch(nomorHp, @"^\d+$"))
                        {
                            errorMessages.Add($"❌ Kontak hanya boleh berisi angka (baris {row.RowNumber()})");
                        }
                        else if (nomorHp.StartsWith("00"))
                        {
                            errorMessages.Add($"❌ Kontak tidak boleh diawali dengan '00' (baris {row.RowNumber()})");
                        }
                        else if (nomorHp.Length < 10 || nomorHp.Length > 13)
                        {
                            errorMessages.Add($"❌ Kontak harus memiliki 10-13 digit (baris {row.RowNumber()})");
                        }
                        else if (!nomorHp.StartsWith("0"))
                        {
                            errorMessages.Add($"❌ Kontak harus diawali dengan '0' (baris {row.RowNumber()})");
                        }
                        //else if (!IsValidPhoneNumber(nomorHp))
                        //{
                        //    errorMessages.Add($"❌ No HP tidak valid: {nomorHp} (baris {row.RowNumber()})");
                        //}

                        // Jika ada error, lewati baris ini
                        if (errorMessages.Count > 0) continue;

                        // Cek jika data dengan NIK dan Bulan Iuran sudah ada
                        var existingData = db.TBL_T_INFORMASI_IURANs
                                             .FirstOrDefault(x => x.NIK == nik && x.Bulan_Iuran == bulanIuran);

                        if (existingData != null)
                        {
                            // Update data yang sudah ada
                            existingData.Nama = nama.ToUpper();
                            existingData.Alamat = alamat.ToUpper();
                            existingData.Status_Iuran = statusIuran.ToUpper();
                            existingData.Kontak = nomorHp;
                            existingData.Upated_Date = DateTime.Now;
                            existingData.Updated_By = "SYSTEM";
                        }
                        else
                        {
                            // Insert data baru
                            var data = new TBL_T_INFORMASI_IURAN
                            {
                                Nama = nama.ToUpper(),
                                NIK = nik,
                                Alamat = alamat.ToUpper(),
                                Status_Iuran = statusIuran.ToUpper(),
                                Bulan_Iuran = bulanIuran.ToUpper(),
                                Kontak = nomorHp,
                                Created_Date = DateTime.Now,
                                Created_By = Session["username"].ToString(),
                                Upated_Date = DateTime.Now,
                                Updated_By = Session["username"].ToString()
                            };

                            dataList.Add(data);
                        }
                        var hist = new TBL_H_INFORMASI_IURAN
                        {
                            Nama = nama.ToUpper(),
                            NIK = nik,
                            Alamat = alamat.ToUpper(),
                            Status_Iuran = statusIuran.ToUpper(),
                            Bulan_Iuran = bulanIuran.ToUpper(),
                            Kontak = nomorHp,
                            Created_Date = DateTime.Now,
                            Created_By = Session["username"].ToString(),
                            Upated_Date = DateTime.Now,
                            Updated_By = Session["username"].ToString()
                        };
                        datahist.Add(hist);

                    }
                    if (!isAnyDataRow)
                    {
                        return Json(new { success = false, message = "❌ Tidak ada data valid yang ditemukan." });
                    }
                    if (errorMessages.Count > 0)
                    {
                        return Json(new { success = false, message = string.Join("\n", errorMessages) });
                    }

                    db.TBL_T_INFORMASI_IURANs.InsertAllOnSubmit(dataList);
                    db.TBL_H_INFORMASI_IURANs.InsertAllOnSubmit(datahist);
                    db.SubmitChanges();

                    return Json(new { success = true, message = "Data berhasil diupload!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }
        [HttpPost]
        public JsonResult UploadPengumuman(HttpPostedFileBase file, string tipe_pengumuman, int tahun_pengumuman)
        {
            var username = Session["username"] != null ? Session["username"].ToString() : "Guest";
            try
            {
                var result = ClsInput.ProsesUpload(file, tipe_pengumuman, tahun_pengumuman, username);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = "Terjadi kesalahan: " + ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetPengumuman(int? tahun, string tipe)
        {
            try
            {
                var result = ClsInput.GetPengumuman(tahun, tipe);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Submit(ClsInput model)
        {
            if (ModelState.IsValid)
            {
                var username = Session["username"] != null ? Session["username"].ToString() : "Guest";
                bool isSaved = model.SaveToDatabase(username);
                if (isSaved)
                {
                    return Json(new { success = true, message = "Pesan berhasil dikirim!" });
                }
                else
                {
                    return Json(new { success = false, message = "Terjadi kesalahan saat menyimpan data." });
                }
            }
            return Json(new { success = false, message = "Validasi gagal, periksa kembali input Anda." });
        }

        [HttpPost]
        public ActionResult Create(TBL_T_KEGIATAN model, HttpPostedFileBase Undangan)
        {
            if (ModelState.IsValid)
            {
                string filePath = null;

                if (Undangan != null && Undangan.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Undangan.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    Undangan.SaveAs(savePath);
                    filePath = "/Uploads/" + fileName;
                }
                var username = Session["username"] != null ? Session["username"].ToString() : "Guest";

                ClsInput clsInput = new ClsInput();
                bool result = clsInput.SimpanKegiatan(model, filePath, username);

                return Json(new { success = result });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public JsonResult GetKegiatan(string nama, DateTime? start, DateTime? end)
        {
            ClsInput clsInput = new ClsInput();
            var kegiatanList = clsInput.GetKegiatan(nama, start, end);

            var result = kegiatanList.Select(k => new
            {
                k.NamaKegiatan,
                Tanggal = k.Tanggal.ToString("yyyy-MM-dd"),
                k.Lokasi,
                k.Deskripsi,
                k.Undangan
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SimpanWarga(ClsInputWarga data)
        {
            var username = Session["username"] != null ? Session["username"].ToString() : "Guest";
            var result = data.InputDataWarga(username);
            if (result != "")
            {
                return Json(new { Remarks = false, Message = "System Error : " + result });
            }
            return Json(new { Remarks = true, Message = "Success submit form" });
        }
        public ActionResult DownloadTemplateWarga()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Template Warga");

                // Header kolom
                string[] headers = new string[]
                {
            "NIK", "NoKK", "Nama", "TempatLahir", "TanggalLahir", "JenisKelamin", "Alamat",
            "RT", "RW", "Kelurahan", "Kecamatan", "Agama", "StatusPerkawinan",
            "Pekerjaan", "Kewarganegaraan", "NoHP", "Email", "TanggalTerdaftar"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                }

                // Format kolom A dan B sebagai teks
                worksheet.Column(1).Style.NumberFormat.Format = "@"; // NIK
                worksheet.Column(2).Style.NumberFormat.Format = "@"; // NoKK
                worksheet.Column(8).Style.NumberFormat.Format = "@";
                worksheet.Column(9).Style.NumberFormat.Format = "@"; 

                // Format Tanggal
                worksheet.Column(5).Style.DateFormat.Format = "yyyy-mm-dd";  // TanggalLahir
                worksheet.Column(18).Style.DateFormat.Format = "yyyy-mm-dd"; // TanggalTerdaftar


                // Format NoHP jadi teks (biar tidak hilang angka 0 di depan)
                worksheet.Column(16).Style.NumberFormat.Format = "@";

                // Dropdown untuk StatusPerkawinan (kolom ke-13)
                var listSheet = workbook.Worksheets.Add("ListData");

                // Dropdown StatusPerkawinan (A1:A4)
                listSheet.Cell("A1").Value = "Belum Menikah";
                listSheet.Cell("A2").Value = "Menikah";
                listSheet.Cell("A3").Value = "Mati Hidup";
                listSheet.Cell("A4").Value = "Mati Cerai";

                // Dropdown JenisKelamin (B1:B2)
                listSheet.Cell("B1").Value = "Laki-Laki";
                listSheet.Cell("B2").Value = "Perempuan";

                // Data Agama (C1:C6)
                listSheet.Cell("C1").Value = "Islam";
                listSheet.Cell("C2").Value = "Katolik";
                listSheet.Cell("C3").Value = "Protestan";
                listSheet.Cell("C4").Value = "Budha";
                listSheet.Cell("C5").Value = "Hindu";
                listSheet.Cell("C6").Value = "Konghucu";

                listSheet.Visibility = XLWorksheetVisibility.VeryHidden;

                // === Validasi dropdown StatusPerkawinan ===
                var statusRange = listSheet.Range("A1:A4");
                var statusValidation = worksheet.Range("M2:M1000").CreateDataValidation();
                statusValidation.IgnoreBlanks = true;
                statusValidation.InCellDropdown = true;
                statusValidation.AllowedValues = XLAllowedValues.List;
                statusValidation.List(statusRange);

                // === Validasi dropdown JenisKelamin ===
                var genderRange = listSheet.Range("B1:B2");
                var genderValidation = worksheet.Range("F2:F1000").CreateDataValidation();
                genderValidation.IgnoreBlanks = true;
                genderValidation.InCellDropdown = true;
                genderValidation.AllowedValues = XLAllowedValues.List;
                genderValidation.List(genderRange);

                // Agama (kolom L / kolom ke-12)
                var agamaRange = listSheet.Range("C1:C6");
                var agamaValidation = worksheet.Range("L2:L1000").CreateDataValidation();
                agamaValidation.IgnoreBlanks = true;
                agamaValidation.InCellDropdown = true;
                agamaValidation.AllowedValues = XLAllowedValues.List;
                agamaValidation.List(agamaRange);

                // Validasi panjang 16 karakter untuk NIK
                var nikValidation = worksheet.Range("A2:A1000").CreateDataValidation();
                nikValidation.AllowedValues = XLAllowedValues.TextLength;
                nikValidation.TextLength.Between(16, 16);
                nikValidation.ShowErrorMessage = true;
                nikValidation.ErrorTitle = "Input Salah";
                nikValidation.ErrorMessage = "NIK harus terdiri dari 16 digit angka tanpa huruf/simbol.";
                nikValidation.ShowInputMessage = true;
                nikValidation.InputTitle = "Format NIK";
                nikValidation.InputMessage = "Masukkan 16 digit angka tanpa huruf/simbol.";

                // Validasi panjang 16 karakter untuk NoKK
                var nokkValidation = worksheet.Range("B2:B1000").CreateDataValidation();
                nokkValidation.AllowedValues = XLAllowedValues.TextLength;
                nokkValidation.TextLength.Between(16, 16);
                nokkValidation.ShowErrorMessage = true;
                nokkValidation.ErrorTitle = "Input Salah";
                nokkValidation.ErrorMessage = "No KK harus terdiri dari 16 digit angka tanpa huruf/simbol.";
                nokkValidation.ShowInputMessage = true;
                nokkValidation.InputTitle = "Format No KK";
                nokkValidation.InputMessage = "Masukkan 16 digit angka tanpa huruf/simbol.";

                // Autofit kolom
                worksheet.Columns().AdjustToContents();

                // Output file
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Template_Data_Warga.xlsx");
                }
            }
        }

        [HttpPost]
        public ActionResult UploadDataWarga(HttpPostedFileBase file)
        {
            DB_LATDataContext db = new DB_LATDataContext();
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    using (var workbook = new XLWorkbook(file.InputStream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed().Skip(1); // Skip header row

                        foreach (var row in rows)
                        {
                            var nik = row.Cell(1).GetValue<string>();
                            var noKK = row.Cell(2).GetValue<string>();
                            var nama = row.Cell(3).GetValue<string>();
                            var tempatLahir = row.Cell(4).GetValue<string>();
                            var tanggalLahir = row.Cell(5).GetValue<DateTime>();
                            var jenisKelamin = row.Cell(6).GetValue<string>();
                            var alamat = row.Cell(7).GetValue<string>();
                            var rt = row.Cell(8).GetValue<string>();
                            var rw = row.Cell(9).GetValue<string>();
                            var kelurahan = row.Cell(10).GetValue<string>();
                            var kecamatan = row.Cell(11).GetValue<string>();
                            var agama = row.Cell(12).GetValue<string>();
                            var statusPerkawinan = row.Cell(13).GetValue<string>();
                            var pekerjaan = row.Cell(14).GetValue<string>();
                            var kewarganegaraan = row.Cell(15).GetValue<string>();
                            var noHp = row.Cell(16).GetValue<string>();
                            var email = row.Cell(17).GetValue<string>();
                            var tanggalTerdaftar = row.Cell(18).GetValue<DateTime>();

                            // Validasi data sesuai kriteria
                            if (string.IsNullOrWhiteSpace(nik) || nik.Length != 16 || !nik.All(char.IsDigit))
                            {
                                return Json(new { Message = "NIK harus terdiri dari 16 angka dan tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(noKK) || noKK.Length != 16 || !noKK.All(char.IsDigit))
                            {
                                return Json(new { Message = "No KK harus terdiri dari 16 angka dan tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(nama))
                            {
                                return Json(new { Message = "Nama tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(tempatLahir))
                            {
                                return Json(new { Message = "Tempat Lahir tidak boleh kosong." });
                            }

                            if (tanggalLahir == default(DateTime))
                            {
                                return Json(new { Message = "Tanggal Lahir tidak boleh kosong atau tidak valid." });
                            }

                            if (string.IsNullOrWhiteSpace(jenisKelamin) || !new[] { "Laki-Laki", "Perempuan" }.Contains(jenisKelamin))
                            {
                                return Json(new { Message = "Jenis Kelamin tidak boleh kosong dan harus Laki-Laki atau Perempuan." });
                            }

                            if (string.IsNullOrWhiteSpace(alamat))
                            {
                                return Json(new { Message = "Alamat tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(rt))
                            {
                                return Json(new { Message = "RT tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(rw))
                            {
                                return Json(new { Message = "RW tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(kelurahan))
                            {
                                return Json(new { Message = "Kelurahan tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(kecamatan))
                            {
                                return Json(new { Message = "Kecamatan tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(agama) || !new[] { "Islam", "Katolik", "Protestan", "Hindu", "Budha", "Konghucu" }.Contains(agama))
                            {
                                return Json(new { Message = "Agama tidak boleh kosong dan harus salah satu dari Islam, Katolik, Protestan, Hindu, Budha, Konghucu." });
                            }

                            if (string.IsNullOrWhiteSpace(statusPerkawinan) || !new[] { "Menikah", "Belum Menikah", "Cerai Hidup", "Cerai Mati" }.Contains(statusPerkawinan))
                            {
                                return Json(new { Message = "Status Perkawinan tidak boleh kosong dan harus salah satu dari Menikah, Belum Menikah, Cerai Hidup, Cerai Mati." });
                            }

                            if (string.IsNullOrWhiteSpace(pekerjaan))
                            {
                                return Json(new { Message = "Pekerjaan tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(kewarganegaraan))
                            {
                                return Json(new { Message = "Kewarganegaraan tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(noHp) || noHp.Length < 10 || noHp.Length > 13 || !noHp.All(char.IsDigit))
                            {
                                return Json(new { Message = "No HP harus terdiri dari 10-13 digit angka dan tidak boleh kosong." });
                            }

                            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                            {
                                return Json(new { Message = "Email tidak valid atau tidak boleh kosong." });
                            }

                            if (tanggalTerdaftar == default(DateTime))
                            {
                                return Json(new { Message = "Tanggal Terdaftar tidak boleh kosong atau tidak valid." });
                            }

                            var username = Session["username"] != null ? Session["username"].ToString() : "Guest";
                            // Cek apakah NIK dan NoKK sudah ada di database
                            var existingWarga = db.TBL_T_WARGAs.SingleOrDefault(w => w.NIK == nik && w.NoKK == noKK);

                            if (existingWarga != null)
                            {
                                // Jika data sudah ada, lakukan update
                                existingWarga.Nama = nama;
                                existingWarga.TempatLahir = tempatLahir;
                                existingWarga.TanggalLahir = tanggalLahir;
                                existingWarga.JenisKelamin = jenisKelamin;
                                existingWarga.Alamat = alamat;
                                existingWarga.RT = rt;
                                existingWarga.RW = rw;
                                existingWarga.Kelurahan = kelurahan;
                                existingWarga.Kecamatan = kecamatan;
                                existingWarga.Agama = agama;
                                existingWarga.StatusPerkawinan = statusPerkawinan;
                                existingWarga.Pekerjaan = pekerjaan;
                                existingWarga.Kewarganegaraan = kewarganegaraan;
                                existingWarga.NoHP = noHp;
                                existingWarga.Email = email;
                                existingWarga.TanggalTerdaftar = tanggalTerdaftar;
                                existingWarga.UpdatedBy = username;
                                existingWarga.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                // Jika data tidak ada, lakukan insert
                                var warga = new TBL_T_WARGA
                                {
                                    NIK = nik,
                                    NoKK = noKK,
                                    Nama = nama,
                                    TempatLahir = tempatLahir,
                                    TanggalLahir = tanggalLahir,
                                    JenisKelamin = jenisKelamin,
                                    Alamat = alamat,
                                    RT = rt,
                                    RW = rw,
                                    Kelurahan = kelurahan,
                                    Kecamatan = kecamatan,
                                    Agama = agama,
                                    StatusPerkawinan = statusPerkawinan,
                                    Pekerjaan = pekerjaan,
                                    Kewarganegaraan = kewarganegaraan,
                                    NoHP = noHp,
                                    Email = email,
                                    TanggalTerdaftar = tanggalTerdaftar,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = username
                                };

                                // Menyimpan data baru ke database
                                db.TBL_T_WARGAs.InsertOnSubmit(warga);
                            }
                        }

                        // Simpan perubahan ke database
                        db.SubmitChanges();
                    }

                    return Json(new { Message = "Data berhasil diupload dan disimpan!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Terjadi kesalahan: " + ex.Message });
                }
            }
            return Json(new { Message = "File tidak valid." });
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
