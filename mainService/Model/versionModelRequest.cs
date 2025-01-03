using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mainService.Model
{
    public class versionModelRequest
    {
        public string versionNumber { get; set; }
        public string clientCode { get; set; }
    }

    public class versionModelResponse
    {
        public bool status { get; set; }
        public string Description { get; set; }
    }
}