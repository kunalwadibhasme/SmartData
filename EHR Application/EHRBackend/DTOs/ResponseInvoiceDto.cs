namespace E_CommerceBackend.DTOs
{
    public class ResponseInvoiceDto
    {
        public string invoiceId { get; set; }
        public string invoiceDate { get; set; }
        public double subTotal { get; set; }
        public string address { get; set; }
        public string zipcode { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public List<ProductInvoiceDto> productInvoices { get; set; }
    }
   
}
