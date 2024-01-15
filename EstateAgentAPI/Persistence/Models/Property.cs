namespace EstateAgentAPI.Persistence.Models
{
    public class Property
    {
        public int PropertyId { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Type { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms {  get; set; }
        public int Garden { get; set; }
        public double Price { get;set; }
        public string Status {  get; set; }
        public int SellerId { get; set; }
        public int? BuyerId { get; set; }
    }
}
