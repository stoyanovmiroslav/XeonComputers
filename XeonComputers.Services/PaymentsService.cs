using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using XeonComputers.Services.Contracts;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace XeonComputers.Services
{
    public class PaymentsService : IPaymentsService
    {
        public PaymentsService(IConfiguration Configuration)
        {
            this.SecretKey = Configuration["Authentication:Epay:Secret"];
            this.Min = Configuration["Authentication:Epay:Min"];
        }

        private string SecretKey { get; set; }

        private string Min { get; set; }

        public string Encoded { get; set; }

        public string GetEncodedData(decimal amount, string description, string expDate, string invoice)
        {
            string data = $"MIN={Min}\nINVOICE={invoice}\nAMOUNT={amount}\nEXP_TIME={expDate}\nDESCR={description}";

            this.Encoded = Base64Encode(data);

            return this.HmacSha1(this.Encoded, SecretKey);
        }

        public string GetDencodedData(string encoded, string checksum)
        {
            var checkSumCalc = HmacSha1(encoded, SecretKey);

            if (checkSumCalc == checksum)
            {
                var data = Base64Decode(encoded);
                return data;
            }

            return null;
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