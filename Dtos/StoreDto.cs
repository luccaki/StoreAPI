namespace StoreAPI.Dtos
{
    public record StoreDto(long Id, string Name, long CompanyId, IEnumerable<ProductDto> Products);
}