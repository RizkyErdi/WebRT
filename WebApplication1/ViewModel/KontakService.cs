using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel
{
    public class KontakService
    {
        public class KontakMessage
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public bool IsRead { get; set; }
        }

        private readonly DB_LATDataContext _context;

        private readonly string _connectionString = "Your_Connection_String";  // Gunakan connection string yang sesuai

        public KontakService(DB_LATDataContext context)
        {
            _context = context;
        }

        // Mendapatkan 5 pesan terbaru dari database menggunakan LINQ
        public List<KontakMessage> GetMessages()
        {
            var messages = _context.TBL_T_KONTAKs
                .OrderByDescending(m => m.CreatedDate)  // Urutkan berdasarkan tanggal terbaru
                .Take(5)  // Ambil 5 pesan terbaru
                .Select(m => new KontakMessage
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Message = m.Message,
                    CreatedDate = m.CreatedDate.HasValue && m.CreatedDate.Value >= new DateTime(1753, 1, 1) // Pastikan tanggal valid
                    ? m.CreatedDate.Value
                    : DateTime.Now,
                    CreatedBy = m.CreatedBy,
                    IsRead = m.IsRead
                })
                .ToList();

            return messages;
        }

        // Mendapatkan jumlah pesan yang belum dibaca menggunakan LINQ
        public int GetUnreadCount()
        {
            var unreadCount = _context.TBL_T_KONTAKs
                .Count(m => m.IsRead == false);  // Hitung pesan yang belum dibaca

            return unreadCount;
        }
        public void UpdateMessage(KontakMessage message)
        {
            var existingMessage = _context.TBL_T_KONTAKs.FirstOrDefault(m => m.Id == message.Id);
            if (existingMessage != null)
            {
                existingMessage.IsRead = true;
                _context.SubmitChanges();
            }
        }
    }
}