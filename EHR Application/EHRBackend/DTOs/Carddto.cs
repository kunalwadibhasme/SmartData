using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.DTOs
{
    public class Carddto
    {
        public long CardNumber { get; set; }
        public string ExpirtDate { get; set; }
        public int CVV { get; set; }
    }
}
