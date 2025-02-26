using E_CommerceBackend.Database;
using E_CommerceBackend.DTOs;
using E_CommerceBackend.Models;
using E_CommerceBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using Dapper;
using System.Net.Mail;
using System.Net;
using E_CommerceBackend.Entities;

namespace E_CommerceBackend.Services
{
    public class RegisterService
    {
        private readonly EhrDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IDapperDbConnection _DapperdbConnection;
        private readonly IWebHostEnvironment _env;
        public RegisterService(EhrDbContext ehrDbContext, IConfiguration configuration, IDapperDbConnection dapperDbConnection, IWebHostEnvironment env)
        {
            _appDbContext = ehrDbContext;
            _configuration = configuration;
            _env = env;
            _DapperdbConnection = dapperDbConnection;
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
                var existingUser = await _appDbContext.PatientProvider.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
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

                // Generate Username and Password
                var generatedUsername = GenerateUsername(registerDto.FirstName, registerDto.LastName, registerDto.DateOfBirth, registerDto.UsertypeId);
                var generatedPassword = GenerateRandomPassword();

                // Create a new user object
                var newUser = new PatientProvider
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
                    City = registerDto.City,
                    PinCode = registerDto.PinCode,
                    Profileimage = savedImagePath, // Save the file path of the image
                    StateId = registerDto.StateId,
                    CountryId = registerDto.CountryId,
                    GenderId = registerDto.GenderId,
                    BloodGroupId = registerDto.BloodGroupId,
                    Qualification = registerDto?.Qualification,
                    SpecializationId = registerDto?.SpecializationId,
                    RegisterationNo = registerDto?.RegisterationNo,
                    VisitingCharge = registerDto?.VisitingCharge
                };

                // Save the user in the database
                await _appDbContext.PatientProvider.AddAsync(newUser);
                await _appDbContext.SaveChangesAsync();

                // Send welcome email
                var emailBody = $"{newUser.FirstName} {newUser.LastName}, you have successfully registered. " +
                                $"Your Username is {newUser.UserName}, and your Password is {generatedPassword}.";
                //EmailService emailService = new EmailService();
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

        public string GenerateUsername(string firstName, string lastName, DateTime dob, int Id)
        {
            // Format the date of birth as DDMMYY
            string dobFormatted = dob.ToString("ddMMyy");

            // Determine prefix based on Id
            string prefix = Id == 1 ? "PR_" : "PT_";

            // Generate the initial username
            string baseUsername = $"{prefix}{lastName.ToUpper()}{firstName[0].ToString().ToUpper()}{dobFormatted}";
            string username = baseUsername;

            // Check for uniqueness and append random digits if necessary
            Random random = new Random();
            while (_appDbContext.PatientProvider.Any(u => u.UserName == username))
            {
                string randomDigits = random.Next(10, 99).ToString();
                username = $"{baseUsername}{randomDigits}";
            }

            return username;
        }

        public string GenerateRandomPassword()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString(); // Ensures an 8-digit number
        }


        public async Task sendEmailAsync(string email, string subject, string body)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string fromEmail = _configuration["Smtp:From"]; // Use an environment variable for security
            string fromPassword = _configuration["Smtp:Password"]; // Use an environment variable for security

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
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                // Log the exception details for debugging
                throw;
            }
        }



        public async Task<RegisterResponseDto> UpdateUserAsync(UpdateUserDto registerDto)
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
                var existingUser = await _appDbContext.PatientProvider.FirstOrDefaultAsync(u => u.Email == registerDto.Email);

                // Decode the Base64 string for the profile image
                //string savedImagePath = ImagePath(registerDto.Profileimage);



                existingUser.FirstName = registerDto.FirstName;
                existingUser.LastName = registerDto.LastName;
                existingUser.DateOfBirth = registerDto.DateOfBirth;
                existingUser.Mobile = registerDto.Mobile;
                existingUser.Address = registerDto.Address;
                existingUser.City = registerDto.City;
                existingUser.PinCode = registerDto.PinCode;
                //if (savedImagePath != null)
                //{ existingUser.Profileimage = savedImagePath; }// Save the file path of the image
                existingUser.StateId = registerDto.StateId;
                existingUser.CountryId = registerDto.CountryId;
                existingUser.GenderId = registerDto.GenderId;
                existingUser.BloodGroupId = registerDto.BloodGroupId;
                existingUser.Qualification = registerDto?.Qualification;
                existingUser.SpecializationId = registerDto?.SpecializationId;
                existingUser.RegisterationNo = registerDto?.RegisterationNo;
                existingUser.VisitingCharge = registerDto?.VisitingCharge;


                // Save the user in the database
                _appDbContext.PatientProvider.Update(existingUser);
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
                var existingUser = await _appDbContext.PatientProvider.FirstOrDefaultAsync(u => u.Email == Email);
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

                _appDbContext.PatientProvider.Update(existingUser);
                await _appDbContext.SaveChangesAsync();

                // Send email with new credentials
                var emailBody = $"{existingUser.FirstName} {existingUser.LastName}, your password has been changed successfully. " +
                                $"Your username is {existingUser.UserName}, and your new password is {generatedPassword}.";
                //EmailService emailService = new EmailService();
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


        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePassworddto changePassworddto)
        {
            var getUser = await _appDbContext.PatientProvider.FirstOrDefaultAsync(u => u.Email == changePassworddto.email);

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
                _appDbContext.PatientProvider.Update(getUser);
                await _appDbContext.SaveChangesAsync();
                return new ChangePasswordResponse
                {
                    Status = 200,
                    Message = "Password Change Successfully"
                };
            }
        }


        /////Login /////////
        public async Task<LoginResponsedto> LoginUserAsync(Logindto logindto)
        {
            var getUser = await _appDbContext.PatientProvider.Where(u => u.UserName == logindto.UserName).FirstOrDefaultAsync();
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

            await _appDbContext.StoreOtp.AddAsync(new StoreOtp
            {
                Email = getUser.Email,
                Code = otp,
                Expiration = DateTime.Now.AddMinutes(5)
            });
            await _appDbContext.SaveChangesAsync();

            // Send OTP via email
            //await SendOtpEmailAsync(getUser.Email, otp);
            var emailBody = $"{getUser.FirstName} {getUser.LastName}, your Login OTP Code is {otp}. ";
            //EmailService emailService = new EmailService();
            await sendEmailAsync(getUser.Email, "Your OTP Code", emailBody);

            var user = await _appDbContext.PatientProvider.FirstOrDefaultAsync(u => u.Email == getUser.Email);
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

        // Validate OTP for the logged-in user
        public async Task<LoginResponsedto> VerifyOtpAsync(OtpDto otpdto)
        {
            //var otpdetail = await _appDbContext.StoreOtp.FirstOrDefaultAsync(u => u.Email == otpdto.Email);
            var otpdetail = await _appDbContext.StoreOtp.Where(u => u.Email == otpdto.Email)
                            .OrderByDescending(u => u.Expiration)
                            .FirstOrDefaultAsync();
            var user = await _appDbContext.PatientProvider.FirstOrDefaultAsync(u => u.Email == otpdto.Email);
            var usertype = await _appDbContext.Usertype
            .Where(u => u.typeId == user.UsertypeId)
            .Select(u => u.usertype)
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
                    Message = "Login successful",

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

        ///////////Select UserType State Country//////////

        public async Task<PatientProvider> GetuserbyId(int id)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM PatientProvider WHERE Id = @id";

                var result = await db.QueryFirstOrDefaultAsync<PatientProvider>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<List<Usertype>> GetUserType()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Usertype";
                var result = await db.QueryAsync<Usertype>(sql);
                return result.ToList();
            }
        }

        public async Task<List<Gender>> GetGender()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Gender";
                var result = await db.QueryAsync<Gender>(sql);
                return result.ToList();
            }
        }


        public async Task<List<BloodGroup>> GetBloodGroup()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM BloodGroup";
                var result = await db.QueryAsync<BloodGroup>(sql);
                return result.ToList();
            }
        }

        public async Task<List<Country>> GetCountry()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Country";
                var result = await db.QueryAsync<Country>(sql);
                return result.ToList();
            }
        }

        public async Task<List<State>> GetState(int id)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM State WHERE CountryId = @CountryId";
                var result = await db.QueryAsync<State>(sql, new { CountryId = id });
                return result.ToList();
            }
        }


        public async Task<List<Specialization>> GetSpecialization()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Specialization";
                var result = await db.QueryAsync<Specialization>(sql);
                return result.ToList();
            }
        }




    }
}
