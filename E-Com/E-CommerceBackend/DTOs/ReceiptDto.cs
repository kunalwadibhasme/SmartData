namespace E_CommerceBackend.DTOs
{
    public class ReceiptDto
    {
        public string InVoiceId { get; set; }
        public List<string> ProductCodes { get; set; }
        public List<float> SellingPrices { get; set; }
        public List<int> Quantities { get; set; }
        public string InVoiceDate { get; set; }
        public float TotalPrice { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryZipCode { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
}
