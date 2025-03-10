using Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GeneratePasswordService : IGeneratePassword
    {
        public string GenerateRandomPassword(string firstName, string lastName)
        {
            Random random = new Random();

            // Get the first letter of the first and last names (in uppercase)
            string firstLetter = firstName.Length > 0 ? firstName[0].ToString().ToUpper() : "";
            string lastLetter = lastName.Length > 0 ? lastName[0].ToString().ToUpper() : "";

            // Generate a random 8-digit number
            string randomNumber = random.Next(10000000, 99999999).ToString();

            // Add a special character
            string specialChar = "!@#$%^&*()".ToCharArray()[random.Next(0, 10)].ToString();

            // Combine everything into a password
            string password = $"{firstLetter}{lastLetter}{randomNumber}{specialChar}";

            return password;
        }
    }

}
