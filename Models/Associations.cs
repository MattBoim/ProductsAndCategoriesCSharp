using System.ComponentModel.DataAnnotations;

namespace ProductsAndCatagories.Models
{
    public class Associations
    {
        [Key]
        public int AssociationId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set;}
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
