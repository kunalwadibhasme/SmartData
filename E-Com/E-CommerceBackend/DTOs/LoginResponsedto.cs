namespace E_CommerceBackend.DTOs
{
    public class LoginResponsedto
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
        public string? Token { get; set; }
    }
}
