using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAppi.Extensions
{
    public class P24Extensions
    {
        private readonly IConfiguration _configuration;
        private bool isSandbox { get; set; }
        public P24Extensions(IConfiguration configuration)
        {
            _configuration = configuration;
            isSandbox = bool.Parse(_configuration["p24_is_sandbox"]);
        }
        public string GetP24RegisterUrl()
        {
            return isSandbox == true ? _configuration["p24_sb_register"] : _configuration["p24_prod_register"];
        }
        public string GetP24VerifyUrl()
        {
            return isSandbox == true ? _configuration["p24_sb_verify"] : _configuration["p24_prod_verify"];
        }
        public string GetP24RequestUrl()
        {
            return isSandbox == true ? _configuration["p24_sb_request"] : _configuration["p24_prod_request"];
        }
        public string GetP24Crc()
        {
            return isSandbox == true ? _configuration["p24_sb_crc"] : _configuration["p24_prod_crc"];
        }
    }
}
