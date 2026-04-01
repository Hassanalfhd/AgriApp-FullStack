using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Results;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IImageService
    {
        Task<Result<string>> SaveImageAsync(IFormFile file, string folder);
        //Task<string> SaveImageAsync(IFormFile file, string basefolder);
        string GetFullUrl(string? relativePath);
         Result<bool> DeleteImage(string? relativePath);
    }
}
