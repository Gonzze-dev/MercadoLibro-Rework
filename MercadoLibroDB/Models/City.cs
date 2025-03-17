namespace MercadoLibroDB.Models
{
    public class City
    {
        public int PostalCode { get; set; }
        public required string Name { get; set; }
        public required int ProvinceId { get; set; }
        public Province? Province { get; set; }
    }
}
