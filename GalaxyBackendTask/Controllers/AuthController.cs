using DataAccess;
using DataProvider.IProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Request;
using Models.DTOs.Response;
using System.Net;

namespace GalaxyBackendTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IAuthProvider _authProvider;

        public AuthController(ApplicationDBContext context, IAuthProvider authProvider)
        {
            _context = context;
            _authProvider = authProvider;
        }
        [HttpPost("Register")]
        public async Task<BaseResponse> Register(RegisterRequest registerRequest)
        {
            BaseResponse response = BaseResponse.Create(HttpStatusCode.OK, null, "");
            string errorDateEntered = "Please make sure you have entered all data";

            if (registerRequest == null)
            {
                response = BaseResponse.Create(HttpStatusCode.BadRequest, null, "data not found");
                return response;
            }

            if (!_authProvider.AuthenticationRepo.CheckRequestedObj(registerRequest))
            {
                response = BaseResponse.Create(HttpStatusCode.BadRequest, null, "The data is incorrect");
                return response;
            }

            if (await _authProvider.AuthenticationRepo.checkIfEmailOrPhoneExists(registerRequest.Email, registerRequest.Mopile))
            {
                response = BaseResponse.Create(HttpStatusCode.BadRequest, null, "Email or phone number has been used before");
                return response;
            }

            var validation = _authProvider.AuthenticationRepo.CheckPasswordStrength(registerRequest.Password);
            if (!string.IsNullOrEmpty(validation))
            {
                response = BaseResponse.Create(HttpStatusCode.BadRequest, null, validation);
                return response;
            }

            var registered = await _authProvider.AuthenticationRepo.UserRegister(registerRequest);
            if (registered)
            {
                response = BaseResponse.Create(HttpStatusCode.OK, null, "successfully registered");
            }
            else
            {
                response = BaseResponse.Create(HttpStatusCode.InternalServerError, null, "An error occurred during registration");
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            var result = await _authProvider.AuthenticationRepo.Login(loginRequest);
            if (result == null)
            {
                return NotFound(new { Message = "user not found" });
            }

            return Ok(result);
        }
    }
}
