using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using XeonComputers.Services.Contracts;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;

namespace XeonComputers.Services
{
    public class PaymentService : IPaymentService
    {
        public PaymentService(IConfiguration Configuration)
        {
            this.SecretKey = Configuration["Authentication:Epay:Secret"];
            this.Min = Configuration["Authentication:Epay:Min"];
        }

        private string SecretKey { get; set; }

        private string Min { get; set; }

        public string Encoded { get; set; }

        public string Invoice { get; set; } = "123456788"; // TODO:

        public string ExpDate { get; set; } = "01.08.2020"; // TODO:


        public string EPay(decimal sum, string description)
        {
            string data = $"MIN={Min}\nINVOICE={this.Invoice}\nAMOUNT={sum}\nEXP_TIME={this.ExpDate}\nDESCR={description}";

            this.Encoded = Base64Encode(data);

            return HmacSha1(this.Encoded, SecretKey);
        }

        private string HmacSha1(string encoded, string secret)
        {
            var enc = Encoding.ASCII;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secret));
            hmac.Initialize();

            byte[] buffer = enc.GetBytes(encoded);
            return BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}