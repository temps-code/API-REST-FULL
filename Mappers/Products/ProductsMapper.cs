using prueba1.Dto.Products;
using prueba1.Models;

namespace prueba1.Mappers.Products
{
    public static class MapperProducts
    {
        public static ProductDto toProductDto(this Product product)
        {
            return new ProductDto()
            {
                id = product.Id,
                article = product.Name,
                description = product.Description,
                img = product.ImagePath,
                price = product.Price
            };
        }
    }
}
