using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agricultural_For_CV_Shared.Enums;


namespace Agricultural_For_CV_DAL.Entities
{

    [Table("Products")]
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; } = decimal.Zero;

        [Required]
        public int QuantityInStock { get; set; }

        [Required]
        public int CropTypeId { get; set; }


        [ForeignKey("CropTypeId")]
        public Crop? Crops {  get; set; }            
        
        [Required]
        public int QuantityTypeId { get; set; }

        [ForeignKey("QuantityTypeId")]
        public QuantityTypes? QuantityTypes { get; set; } = null;

        public bool IsDeleted { get; set; } = false;

        [Required]
        public int CreatedBy { get; set; }


        [ForeignKey("CreatedBy")]
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ?UpdatedAt { get; set; }

        public enProductStatus Status  {get; set; } = enProductStatus.Pending;

        //relation  with its images

        [MaxLength(5)]
        public ICollection<ProductsImages> ProductsImages { get; set; } = new List<ProductsImages>();
    
        public ICollection<OrderDetail>? OrderDetails { get;set; } 

    }
}
