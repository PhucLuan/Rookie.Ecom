using System;
using System.Collections.Generic;
using System.Text;

namespace Rookie.Ecom.Contracts.Setting
{
    public class ServiceSettings
    {
        public const string IdentityService = "IdentityService";
        public string ExternalUrl { get; set; }
        public string Url { get; set; }
    }
}
