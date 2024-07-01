using DataAccess.IRepo;
using Infrastructure.helpers.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Request;
using Models.DTOs.Response;
using Models.models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class AuthenticationRepo:IAuthenticationRepo
    {
        private readonly ApplicationDBContext _context;
        private readonly Encryption _encryption;

        public AuthenticationRepo(ApplicationDBContext context, Encryption encryption)
        {
            _context = context;
            _encryption = encryption;
        }
        public async Task<bool> UserRegister(RegisterRequest registerRequest)
        {
            try
            {

                string encryptedPassword = _encryption.Encrypt(registerRequest.Password);


                Users newUser = new Users
                {
                    userName = registerRequest.userName,
                    Email = registerRequest.Email,
                    passWord = encryptedPassword,
                    Mobile = registerRequest.Mopile,
                };


                await _context.users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> checkIfEmailOrPhoneExists(string _email, string _phone)
        {
            return await _context.users.AnyAsync(h => h.Email == _email || h.Mobile == _phone);
        }
        public string CheckPasswordStrength(string _password)
        {
            StringBuilder sb = new StringBuilder();

            if (_password.Length < 6)
                sb.Append("The password must be at least 6 characters long" + Environment.NewLine);
            if (!(Regex.IsMatch(_password, "[a-z]")
               && Regex.IsMatch(_password, "[A-Z]")
               && Regex.IsMatch(_password, "[0-9]")))
            {
                sb.Append("The password must be alphanumeric" + Environment.NewLine);
            }

            return sb.ToString();
        }

        #region Login
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            try
            {
                var user = await _context.users
                            .FirstOrDefaultAsync(h => h.Email == loginRequest.email_phone || h.Mobile == loginRequest.email_phone);

                if (user == null)
                {
                    return null;
                }

                Encryption encryption = new Encryption();
                if (encryption.Encrypt(loginRequest.password) != user.passWord)
                {
                    return null;
                }

                LoginResponse loginResponse = new LoginResponse();
                loginResponse.token = GenerateJwtToken(user);
                loginResponse.userDto = new UserDto()
                {
                    id = user.id,
                    userName = user.userName,
                    Email = user.Email,


                };
                user.token = loginResponse.token;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return loginResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        public async Task<int> CreateUser(RegisterRequest user)
        {
            try
            {
                Users newUser = new Users
                {
                    userName = user.userName,

                    Email = user.Email,
                    Mobile = user.Mopile,

                    isActive = true,
                    token = user.token
                };

                await _context.users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return newUser.id;
            }
            catch
            {
                return 0;
            }
        }
        public bool CheckRequestedObj(RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.userName) ||

                string.IsNullOrEmpty(registerRequest.Mopile) ||
                string.IsNullOrEmpty(registerRequest.Email) ||
                string.IsNullOrEmpty(registerRequest.Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string GenerateJwtToken(Users user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("InTheNameOfAllah...");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.Mobile)

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials,
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
