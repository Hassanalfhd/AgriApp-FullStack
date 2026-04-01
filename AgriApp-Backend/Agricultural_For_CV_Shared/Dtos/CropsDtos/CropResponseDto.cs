using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Dtos.CropsDtos
{
    public class CropResponseDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int OwnerId { get; set; }
        public string? ImagePath { get; set; }
        public string ?CategoryName { get; set; }
        public string? Username { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
