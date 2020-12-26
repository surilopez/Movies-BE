using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Movies_BE.Utilities
{
    public class LocalStorage : IStorageFile
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFiles(string filePath, string container)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(filePath);
            var directoryFile = Path.Combine(env.WebRootPath, container, fileName);

            if (File.Exists(directoryFile))
            {
                File.Delete(directoryFile);
            }

            return Task.CompletedTask; 
        }

        public async Task<string> EditFiles(string container, IFormFile file, string filePath)
        {
            await DeleteFiles(filePath, container);
            return await SaveFiles(container, file);
        }

        public async Task<string> SaveFiles(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, fileName);
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(filePath, content);
            }

            var actualURL = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var dbURL = Path.Combine(actualURL, container, fileName).Replace("\\", "/");
            return dbURL;
        }
    }
}
