using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Agricultural_For_CV_Shared.Dtos.UserDtos
{
    public class UserDto
    {

        public string fullName { get; set; } = string.Empty; 
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [MinLength(6)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty; // للـ Register فقط
        public int UserType { get; set; } // 1 = Admin, 2 = Farmer, 3 = Customer , 4= Agricultural Guide
        public string? Phone{ get; set; }


    }

}
