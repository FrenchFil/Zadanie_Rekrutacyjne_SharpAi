using System.Text.Json.Serialization;

namespace Zadanie_Rekrutacyjne_SharpAi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]
        public List<Order> Orders { get; set; } = new();
    }
}
