using App.Data.Entities;

namespace App.Mvc.Models
{
    public class ProductCartViewModel
    {
        public List<CartEntity> Carts { get; set; }
        public List<ProductEntity> Products { get; set; }
    }
}
