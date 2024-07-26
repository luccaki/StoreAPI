namespace StoreAPI.Dtos
{
    public record CompanyDto(long Id, string Name, string Owner, IEnumerable<StoreDto> Stores);
}