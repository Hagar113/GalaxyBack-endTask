﻿using DataAccess;
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
        public async Task<GeneralResponse> Register(RegisterRequest registerRequest)
        {
            GeneralResponse response = GeneralResponse.Create(HttpStatusCode.OK, null, "");
            string errorDateEntered = "Please make sure you have entered all data";

            if (registerRequest == null)
            {
                response = GeneralResponse.Create(HttpStatusCode.BadRequest, null, "data not found");
                return response;
            }

            if (!_authProvider.AuthenticationRepo.CheckRequestedObj(registerRequest))
            {
                response = GeneralResponse.Create(HttpStatusCode.BadRequest, null, "The data is incorrect");
                return response;
            }

            if (await _authProvider.AuthenticationRepo.checkIfEmailOrPhoneExists(registerRequest.Email, registerRequest.Mobile))
            {
                response = GeneralResponse.Create(HttpStatusCode.BadRequest, null, "Email or phone number has been used before");
                return response;
            }

            var validation = _authProvider.AuthenticationRepo.CheckPasswordStrength(registerRequest.Password);
            if (!string.IsNullOrEmpty(validation))
            {
                response = GeneralResponse.Create(HttpStatusCode.BadRequest, null, validation);
                return response;
            }

            var registered = await _authProvider.AuthenticationRepo.UserRegister(registerRequest);
            if (registered)
            {
                response = GeneralResponse.Create(HttpStatusCode.OK, null, "successfully registered");
            }
            else
            {
                response = GeneralResponse.Create(HttpStatusCode.InternalServerError, null, "An error occurred during registration");
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<GeneralResponse> Login([FromBody] LoginRequest loginRequest)
        {
            GeneralResponse response = GeneralResponse.Create(HttpStatusCode.OK, null, "");

            if (loginRequest == null)
            {
                response = GeneralResponse.Create(HttpStatusCode.BadRequest, null, "data not found");
                return response;
            }

            var result = await _authProvider.AuthenticationRepo.Login(loginRequest);
            if (result == null)
            {
                response = GeneralResponse.Create(HttpStatusCode.NotFound, null, "user not found");
                return response;
            }

            response = GeneralResponse.Create(HttpStatusCode.OK, result, "logged successfully");
            return response;
        }
    }
}
