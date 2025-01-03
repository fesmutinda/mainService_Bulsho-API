using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mainService.Model
{
    public class WrongHeadersResponse
    {
        public string statusCode { get; set; }

        public string Description { get; set; }

        public bool Success { get; set; }
    }
}