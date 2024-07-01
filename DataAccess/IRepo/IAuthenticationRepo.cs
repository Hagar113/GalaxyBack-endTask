using Models.DTOs.Request;
using Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IAuthenticationRepo
    {
        Task<bool> UserRegister(RegisterRequest loginRequest);
        bool CheckRequestedObj(RegisterRequest loginRequest);
        Task<bool> checkIfEmailOrPhoneExists(string _email, string _phone);
        string CheckPasswordStrength(string _password);
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
