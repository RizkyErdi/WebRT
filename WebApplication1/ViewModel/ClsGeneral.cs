using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class ClsGeneral
    {
        DB_SMTPDataContext dbmail = new DB_SMTPDataContext();
        DB_LATDataContext dbdata = new DB_LATDataContext();
        public string SendMailApprover(string jenissurat, string nik, string nama, string alamat, string tujuan, string linkapproval, string nomorinduk)
        {
            //var webAppLink = System.Configuration.ConfigurationManager.AppSettings["WebApp_Link"].ToString();
            try
            {
                var email = dbdata.TBL_T_WARGAs.Where(w => w.NIK == nomorinduk).Select(w => w.Email).FirstOrDefault();
                // Menyusun subject dan body email langsung di dalam kode
                string subject = "Pengajuan Surat";
                string body = "Dear Pengurus RT005 Regensi 1,<br/><br/>" +
                              "Terdapat pengajuan surat <b>" + jenissurat + "</b> yang memerlukan approval dari Ketua RT yang diajukan oleh:<br/>" +
                              "<ul>" +
                              "<li><b>NIK:</b> " + nik + "</li>" +
                              "<li><b>Nama:</b> " + nama + "</li>" +
                              "<li><b>Alamat:</b> " + alamat + "</li>" +
                              "<li><b>Tujuan:</b> " + tujuan + "</li>" +
                              "</ul>" +
                              "<br/>Untuk melakukan approval, silakan klik link di bawah ini:<br/>" +
                              "<a href='" + linkapproval + "'>Klik untuk Approve</a><br/><br/>" +
                              "Terima kasih.";

                // Kirim email
                bool result = SendEmail(subject, body, email);

                if (!result)
                {
                    return "Gagal mengirim email.";
                }

                return ""; // Success
            }
            catch (Exception ex)
            {
                return "Send Email Error: " + ex.ToString();
            }
        }
        private bool SendEmail(string subject, string body, string email)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Port = 587,
                    Credentials = new NetworkCredential("noreplaytest15@gmail.com", "qykuzwmazdlukwrg"),
                    EnableSsl = true
                };

                MailMessage message = new MailMessage
                {
                    From = new MailAddress("noreplaytest15@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.CC.Add(email);
                message.To.Add("cdetective72@gmail.com");

                smtp.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Email error: " + ex.Message);
                return false;
            }
        }
    }
}