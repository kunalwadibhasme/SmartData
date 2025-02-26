namespace CustomerMicroService.Dtos
{
    public class AddCustomer
    {
        public required string Name { get; set; }
        public required string Contact { get; set; }
        public required string City { get; set; }
        public required string Email { get; set; }
    }
}
