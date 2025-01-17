﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Response
{
    public class GeneralResponse
    {
        public static GeneralResponse Create(HttpStatusCode statusCode, object result = null, string responseMsg = null)
        {
            return new GeneralResponse(statusCode, result, responseMsg);
        }
        public int statusCode { get; set; } = 200;
        public bool? isSuccess { get; set; } = false;
        public object? result { get; set; } = null;
        public string? msg { get; set; } = string.Empty;

        protected GeneralResponse(HttpStatusCode statusCode, object result = null, string responseMsg = null)
        {
            this.statusCode = (int)statusCode;
            isSuccess = (int)statusCode == 200 ? true : false;
            this.result = result;
            msg = responseMsg;
        }
    }
}

