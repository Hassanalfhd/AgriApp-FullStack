using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive{ get; set; }
        public string? Phone { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }  // هنا يقبل رفع صورة
    }

}
