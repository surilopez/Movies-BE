using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Movies_BE.Utilities
{
    public interface IStorageFile
    {
        Task DeleteFiles(string filePath, string container);
        Task<string> EditFiles(string container, IFormFile file, string filePath);
        Task<string> SaveFiles(string container, IFormFile file);
    }
}