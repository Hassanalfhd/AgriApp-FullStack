using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Enums;

namespace Agricultural_For_CV_Shared.Dtos.UserDtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string UserType { get; set; }
        public bool IsActive{ get; set; }

        public string? ImageFile { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
       
    
    }



}
