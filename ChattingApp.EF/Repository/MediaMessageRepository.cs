using ChattingApp.CORE.DTO.ResultDTO;
using ChattingApp.CORE.Interface;
using CORE.Entities;
using EF.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.EF.Repository
{
    public class MediaMessageRepository : IMediaMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImagesForMessageMedia");
        public MediaMessageRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IntResult> Add(IFormFile media, int messageId)
        {
            var filePath=chickFilePath(media);
            if (string.IsNullOrEmpty(filePath.Id))
            {
                return new IntResult { Message = filePath.Message };
            }
            var mediaMessage = new MessageMedia(filePath.Id, messageId);
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            _context.MessageMedias.Add(mediaMessage);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    await _context.SaveChangesAsync();
                    using (var stream = new FileStream(filePath.Id, FileMode.Create))
                    {
                        await media.CopyToAsync(stream);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    if (File.Exists(filePath.Id))
                    {
                        File.Delete(filePath.Id);
                    }
                    return new IntResult() { Message = ex.Message };
                }
            }
            return new IntResult() { Id = mediaMessage.Id };
        }
        StringResult chickFilePath(IFormFile file)
        {
            if (file is null)
            {
                return new StringResult { Message = "No file provided." };
            }
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_storagePath, fileName);
            return new StringResult { Id = filePath };
        }
    }
}
