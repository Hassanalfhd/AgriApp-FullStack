using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.UserDtos
{
    public class JwtUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string? UserType { get; set; }
    }


}
