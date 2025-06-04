namespace Zadanie_Rekrutacyjne_SharpAi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
