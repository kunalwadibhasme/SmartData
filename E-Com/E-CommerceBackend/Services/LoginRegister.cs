
using E_CommerceBackend.Database;
using E_CommerceBackend.DTOs;
using E_CommerceBackend.Entities;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_CommerceBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
namespace E_CommerceBackend.Services
{
    
    public class LoginRegister
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IDapperDbConnection _DapperdbConnection;
        private readonly IWebHostEnvironment _env;


        public LoginRegister(AppDbContext appDbcontext, IConfiguration configuration, IDapperDbConnection DapperdbConnection, IWebHostEnvironment env)
        {
            _appDbContext = appDbcontext;
            _configuration = configuration;
            _DapperdbConnection = DapperdbConnection;
            _env = env;
        }

        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return new RegisterResponseDto
                {
                    Status = 400,
                    Message = "Invalid data",
                    Data = null
                };
            }

            try
            {
                // Check if the user already exists
                var existingUser = await _appDbContext.Usertable.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
                if (existingUser != null)
                {
                    return new RegisterResponseDto
                    {
                        Status = 400,
                        Message = "User already exists",
                        Data = null
                    };
                }

                // Decode the Base64 string for the profile image
                string savedImagePath = ImagePath(registerDto.Profileimage);
                //if (!string.IsNullOrEmpty(registerDto.Profileimage))
                //{
                //    var base64Image = registerDto.Profileimage;
                //    var imageBytes = Convert.FromBase64String(base64Image.Split(',')[1]); // Skip the prefix (e.g., "data:image/png;base64,")
                //    // Generate a unique file path and save the image
                //    var uniqueFileName = $"{Guid.NewGuid()}.png";
                //    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profileimages");
                //    savedImagePath = Path.Combine(folderPath, uniqueFileName);
                //    // Ensure the directory exists
                //    if (!Directory.Exists(folderPath))
                //    {
                //        Directory.CreateDirectory(folderPath);
                //    }
                //    await System.IO.File.WriteAllBytesAsync(savedImagePath, imageBytes);
                //}
                // Generate Username and Password
                var generatedUsername = GenerateUsername(registerDto.FirstName, registerDto.LastName, registerDto.DateOfBirth);
                var generatedPassword = GenerateRandomPassword();

                // Create a new user object
                var newUser = new Usertable
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    UsertypeId = registerDto.UsertypeId,
                    DateOfBirth = registerDto.DateOfBirth,
                    UserName = generatedUsername,
                    Password = BCrypt.Net.BCrypt.HashPassword(generatedPassword),
                    Mobile = registerDto.Mobile,
                    Address = registerDto.Address,
                    ZipCode = registerDto.ZipCode,
                    Profileimage = savedImagePath, // Save the file path of the image
                    StateId = registerDto.StateId,
                    CountryId = registerDto.CountryId
                };

                // Save the user in the database
                await _appDbContext.Usertable.AddAsync(newUser);
                await _appDbContext.SaveChangesAsync();

                // Send welcome email
                var emailBody = $"{newUser.FirstName} {newUser.LastName}, you have successfully registered. " +
                                $"Your Username is {newUser.UserName}, and your Password is {generatedPassword}.";
                await sendEmailAsync(newUser.Email, "Your Credentials", emailBody);

                // Return success response
                return new RegisterResponseDto
                {
                    Status = 200,
                    Message = "User Registered Successfully",
                    Data = newUser
                };
            }
            catch (Exception ex)
            {
                // Return error response
                return new RegisterResponseDto
                {
                    Status = 500,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                };
            }
        }


        public string ImagePath(IFormFile image)
        {
            if (image != null)
            {
                var folderPath = Path.Combine(_env.WebRootPath, "profile_images");
                // Ensure the directory exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // Generate a unique file name
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);
                // Save the file asynchronously
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream); // Synchronous operation for this case
                }
                // Return relative path
                return $"/profile_images/{fileName}";
            }
            return $"/profile_images/default.jpg";
        }


        
        public async Task<RegisterResponseDto> UpdateUserAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return new RegisterResponseDto
                {
                    Status = 400,
                    Message = "Invalid data",
                    Data = null
                };
            }

            try
            {
                // Check if the user already exists
                var existingUser = await _appDbContext.Usertable.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
                
                // Decode the Base64 string for the profile image
                string savedImagePath = ImagePath(registerDto.Profileimage);
                //if (!string.IsNullOrEmpty(registerDto.Profileimage))
                //{
                //    var base64Image = registerDto.Profileimage;
                //    var imageBytes = Convert.FromBase64String(base64Image.Split(',')[1]); // Skip the prefix (e.g., "data:image/png;base64,")

                //    // Generate a unique file path and save the image
                //    var uniqueFileName = $"{Guid.NewGuid()}.png";
                //    var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/profileimages");
                //    savedImagePath = Path.Combine(folderPath, uniqueFileName);

                //    // Ensure the directory exists
                //    if (!Directory.Exists(folderPath))
                //    {
                //        Directory.CreateDirectory(folderPath);
                //    }

                //    await System.IO.File.WriteAllBytesAsync(savedImagePath, imageBytes);
                //}


                existingUser.FirstName = registerDto.FirstName;
                existingUser.LastName = registerDto.LastName;
                existingUser.DateOfBirth = registerDto.DateOfBirth;
                existingUser.Mobile = registerDto.Mobile;
                existingUser.Address = registerDto.Address;
                existingUser.ZipCode = registerDto.ZipCode;
                if (savedImagePath != null)
                { existingUser.Profileimage = savedImagePath; }// Save the file path of the image
                existingUser.StateId = registerDto.StateId;
                existingUser.CountryId = registerDto.CountryId;
               

                // Save the user in the database
                _appDbContext.Usertable.Update(existingUser);
                await _appDbContext.SaveChangesAsync();

               
                // Return success response
                return new RegisterResponseDto
                {
                    Status = 200,
                    Message = "User Updated Successfully",
                    Data = existingUser
                };
            }
            catch (Exception ex)
            {
                // Return error response
                return new RegisterResponseDto
                {
                    Status = 500,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                };
            }
        }


       

        private async Task sendEmailAsync(string email, string subject, string body)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string fromEmail = "kwadibhasme2002@gmail.com"; // Use an environment variable for security
            string fromPassword = "ants hldz hqtk fzmx";  // Use an environment variable for security

            try
            {
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true; // Enable TLS encryption
                    client.Credentials = new NetworkCredential(fromEmail, fromPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false, // Set to true if you want to send HTML emails
                    };

                    mailMessage.To.Add(email);

                    // Send email asynchronously
                    await client.SendMailAsync(mailMessage);
                }
                Console.WriteLine($"OTP email sent to {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
        }















        [HttpPost("forgetpassword")]
        public async Task<RegisterResponseDto> ForgetPasswordAsync(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return new RegisterResponseDto
                {
                    Status = 400,
                    Message = "Enter Email Id",
                    Data = null
                };
            }

            try
            {
                // Check if the user already exists
                var existingUser = await _appDbContext.Usertable.FirstOrDefaultAsync(u => u.Email == Email);
                if (existingUser == null)
                {
                    return new RegisterResponseDto
                    {
                        Status = 404,
                        Message = "User not found.",
                        Data = null
                    };
                }

                // Generate a new password and update the user record
                var generatedPassword = GenerateRandomPassword();
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(generatedPassword);

                _appDbContext.Usertable.Update(existingUser);
                await _appDbContext.SaveChangesAsync();

                // Send email with new credentials
                var emailBody = $"{existingUser.FirstName} {existingUser.LastName}, your password has been changed successfully. " +
                                $"Your username is {existingUser.UserName}, and your new password is {generatedPassword}.";
                await sendEmailAsync(existingUser.Email, "Your Credentials", emailBody);

                // Return success response
                return new RegisterResponseDto
                {
                    Status = 200,
                    Message = "Password reset successfully.",
                    Data = existingUser
                };
            }
            catch (Exception ex)
            {
                // Return error response
                return new RegisterResponseDto
                {
                    Status = 500,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                };
            }
        }





        public string GenerateUsername(string firstName, string lastName, string dob)
        {
            // Format the date of birth as DDMMYY
            DateTime parsedDate = DateTime.ParseExact(dob, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            // Convert the DateTime object to the desired format
            string formattedDate = parsedDate.ToString("dd/MM/yyyy"); 
            string[] dobParts = formattedDate.Split('-');
            if (dobParts.Length != 3)
            {
                throw new ArgumentException("Invalid DOB format. Expected format: DD/MM/YYYY.");
            }

            string dobFormatted = $"{dobParts[0]}{dobParts[1]}{dobParts[2].Substring(2)}";

            // Generate the username in the required format
            string username = $"EC_{lastName.ToUpper()}{firstName[0].ToString().ToUpper()}{dobFormatted}";
            bool exists = _appDbContext.Usertable.Any(u => u.UserName == username);
            Random random = new Random(); 
            while (exists) {
                string randomDigits = random.Next(10, 99).ToString(); 
                username = $"EC_{lastName.ToUpper()}{firstName[0].ToString().ToUpper()}{dobFormatted}{randomDigits}"; 
                exists = _appDbContext.Usertable.Any(u => u.UserName == username); }
            return username;
        }

        public string GenerateRandomPassword()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString(); // Ensures an 8-digit number
        }




        /////Login /////////
        public async Task<LoginResponsedto> LoginUserAsync(Logindto logindto)
        {
            var getUser = await _appDbContext.Usertable.Where(u => u.UserName == logindto.UserName).FirstOrDefaultAsync();
            if (getUser == null || !BCrypt.Net.BCrypt.Verify(logindto.Password, getUser.Password))
            {
                return new LoginResponsedto
                {
                    Status = 404,
                    Message = "Email Or Password Invalid",
                    Data = "Wrong Email or password"
                };
            }
            
            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();


            // Save OTP to database
            
            await _appDbContext.StoreOtp.AddAsync(new StoreOtp { Email = getUser.Email, Code = otp,
                Expiration = DateTime.Now.AddMinutes(5) });
            await _appDbContext.SaveChangesAsync();

            // Send OTP via email
            await SendOtpEmailAsync(getUser.Email, otp);
             var user = await _appDbContext.Usertable.FirstOrDefaultAsync(u => u.Email == getUser.Email);
            var usertype = await _appDbContext.Usertype
            .Where(u => u.typeId == user.UsertypeId)
            .Select(u => u.usertype)
            .ToListAsync();
            var token = await Task.FromResult(GenerateJWTToken(user.Email, usertype));

            return new LoginResponsedto
            {
                Status = 200,
                Message = "OTP sent to email. Please verify.",
                Data = getUser,
                Token = token
            };

            
        }

        //Sending Logging Verification OTP to User
        private async Task SendOtpEmailAsync(string email, string otp)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string fromEmail = "kwadibhasme2002@gmail.com"; // Use an environment variable for security
            string fromPassword = "ants hldz hqtk fzmx";  // Use an environment variable for security

            try
            {
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true; // Enable TLS encryption
                    client.Credentials = new NetworkCredential(fromEmail, fromPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = "Your OTP Code",
                        Body = $"Your OTP code is: {otp}. It is valid for the next 5 minutes.",
                        IsBodyHtml = false, // Set to true if you want to send HTML emails
                    };

                    mailMessage.To.Add(email);

                    // Send email asynchronously
                    await client.SendMailAsync(mailMessage);
                }
                Console.WriteLine($"OTP email sent to {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
        }


        // Validate OTP for the logged-in user
        public async Task<LoginResponsedto> VerifyOtpAsync(OtpDto otpdto)
        {
            //var otpdetail = await _appDbContext.StoreOtp.FirstOrDefaultAsync(u => u.Email == otpdto.Email);
            var otpdetail = await _appDbContext.StoreOtp.Where(u => u.Email == otpdto.Email)
                            .OrderByDescending(u => u.Expiration)
                            .FirstOrDefaultAsync();
            var user = await _appDbContext.Usertable.FirstOrDefaultAsync(u => u.Email == otpdto.Email);
            var usertype = await _appDbContext.Usertype
            .Where(u => u.typeId == user.UsertypeId)
            .Select(u=> u.usertype)
            .ToListAsync();
            if (otpdetail.Code == "")
            {
                return new LoginResponsedto
                {
                    Status = 404,
                    Message = "Otp Expired",
                };
            }
            if (otpdetail.Code == otpdto.Otp && otpdetail.Expiration.HasValue && otpdetail.Expiration.Value > DateTime.UtcNow)
            {
                // Clear OTP after successful verification
                var otpDetails = await _appDbContext.StoreOtp.Where(u => u.Email == otpdto.Email).ToListAsync();
                _appDbContext.StoreOtp.RemoveRange(otpDetails);
                await _appDbContext.SaveChangesAsync();
                // Generate JWT token
                //var token = await Task.FromResult(GenerateJWTToken(user.Email, usertype));
                return new LoginResponsedto
                {
                    Status = 200,
                    Message="Login successful",
                    
                };
            }
            else
            {
                return new LoginResponsedto
                {
                    Status = 400,
                    Message = "Invalid Otp or Otp Expired",
                };
            }
        }

        // For generating JWT Token
        public string GenerateJWTToken(string email, IList<string> usertype)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            };
            foreach (var role in usertype)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        /////Change Password////////
        
        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePassworddto changePassworddto)
        {
            var getUser = await _appDbContext.Usertable.FirstOrDefaultAsync(u => u.Email == changePassworddto.email);

            if (getUser == null)
            {
                return new ChangePasswordResponse
                {
                    Status = 404,
                    Message = "UserName Invalid",
                };
            }
            else
            {
                getUser.Password = BCrypt.Net.BCrypt.HashPassword(changePassworddto.Password);
                _appDbContext.Usertable.Update(getUser);
                await _appDbContext.SaveChangesAsync();
                return new ChangePasswordResponse
                {
                    Status = 200,
                    Message = "Password Change Successfully"
                };
            }
        }
        ///////////Select UserType State Country//////////

        //public async Task<Usertable> GetuserbyId(int id)
        //{
        //    return await _appDbContext.Usertable.FirstOrDefaultAsync(u => u.UsertableId == id);
        //}

        public async Task<Usertable> GetuserbyId(int id)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Usertable WHERE UsertableId = @UsertableId";

                var result = await db.QueryFirstOrDefaultAsync<Usertable>(sql, new { UsertableId = id });
                return result;
            }
        }


        //public async Task<List<Usertype>> GetUserType()
        //{
        //    return await _appDbContext.Usertype.ToListAsync();
        //}
        public async Task<List<Usertype>> GetUserType()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Usertype";
                var result = await db.QueryAsync<Usertype>(sql);
                return result.ToList();
            }
        }

        //public async Task<List<Country>> GetCountry()
        //{
        //    return await _appDbContext.Country.ToListAsync();
        //}

        public async Task<List<Country>> GetCountry()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Country";
                var result = await db.QueryAsync<Country>(sql);
                return result.ToList();
            }
        }


        //public async Task<List<State>> GetState(int id)     //returns state based on the id of country selected above
        //{
        //    return await _appDbContext.State.Where(state => state.CountryId == id).ToListAsync();
        //}
        public async Task<List<State>> GetState(int id)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM State WHERE CountryId = @CountryId";
                var result = await db.QueryAsync<State>(sql, new { CountryId = id });
                return result.ToList();
            }
        }




    }
}



